using UnityEngine;

namespace ARDiabetes
{
    /// <summary>
    /// Monta un visor 3D fuera de pantalla: instancia el modelo, lo auto-encuadra y lo renderiza
    /// a una RenderTexture con su propia cámara y luz (aislado por layer). El resultado se muestra
    /// en un RawImage. Sustituye al fondo de cámara real de AR hasta integrar ARCore.
    /// </summary>
    public class ModelViewer : MonoBehaviour
    {
        public RenderTexture Texture { get; private set; }
        Spinner spinner;
        Camera cam;
        Transform holder;
        int Layer; // una layer DISTINTA por instancia (ver Create) — antes era una const compartida

        /// <summary>slot 0/1/2 (uno por libro) -> layer 2/3/4, para que cada visor renderice SOLO su
        /// propio modelo. Los 3 rigs comparten la misma posición en el mundo (todos lejos de la
        /// escena real) — sin layers distintas, la cámara de cada visor terminaba renderizando los
        /// 3 modelos superpuestos (bug real encontrado 2026-07-23: el páncreas se veía "multicolor"
        /// porque en realidad eran el páncreas + el plato + el glucómetro apilados en el mismo punto).</summary>
        public static ModelViewer Create(GameObject modelPrefab, Color tint, int slot = 0)
        {
            var rig = new GameObject("Model3DRig");
            rig.transform.position = new Vector3(0, -500, 0); // lejos, además solo se ve por RT
            var mv = rig.AddComponent<ModelViewer>();
            mv.Layer = 2 + slot;
            mv.Build(modelPrefab, tint);
            return mv;
        }

        /// <summary>
        /// Material Standard (Built-in; los materiales URP del prototipo se verían rosados).
        /// Sin paleta, todo el modelo se tiñe del color del libro. Con paleta (los modelos animados
        /// del cliente: cada pieza toma su color por UV de una textura de colores), se usa esa
        /// textura sin teñir.
        /// </summary>
        public static void AplicarMateriales(GameObject model, Color tint, float brillo, Texture paleta = null)
        {
            var sh = Shader.Find("Standard");
            if (sh == null) return;
            if (paleta == null && ColoresPropios(model)) { ConservarColores(model, sh, brillo); return; }
            var mat = paleta != null ? new Material(sh) { color = Color.white, mainTexture = paleta }
                                     : new Material(sh) { color = tint };
            mat.SetFloat("_Glossiness", brillo);
            foreach (var r in model.GetComponentsInChildren<Renderer>())
            {
                var mats = new Material[r.sharedMaterials.Length];
                for (int i = 0; i < mats.Length; i++) mats[i] = mat;
                r.sharedMaterials = mats;
            }
        }

        // Los modelos de los libros Fisiológico y Clínico (Fisio_*.fbx, Clin_*.fbx) no traen texturas:
        // cada pieza tiene su color en el material del FBX. Se conservan esos colores en vez de teñir
        // todo con el color del libro.
        static bool ColoresPropios(GameObject model) => model.name.StartsWith("Fisio_") || model.name.StartsWith("Clin_");

        /// <summary>Material Standard en modo Fade guardado como asset (lo asigna AppBootstrap): las
        /// piezas semitransparentes (el agua del lavado de manos) se copian de él. Con un material
        /// creado por código el build podría no traer la variante transparente del shader.</summary>
        public static Material MaterialTransparente;

        static void ConservarColores(GameObject model, Shader sh, float brillo)
        {
            foreach (var r in model.GetComponentsInChildren<Renderer>())
            {
                var mats = new Material[r.sharedMaterials.Length];
                for (int i = 0; i < mats.Length; i++)
                {
                    var orig = r.sharedMaterials[i];
                    var color = orig != null && orig.HasProperty("_Color") ? orig.color : Color.white;
                    bool transparente = color.a < .99f && MaterialTransparente != null;
                    mats[i] = transparente ? new Material(MaterialTransparente) { color = color } : new Material(sh) { color = color };
                    mats[i].SetFloat("_Glossiness", brillo);
                }
                r.sharedMaterials = mats;
            }
        }

        /// <summary>Tamaño (lado mayor, en metros) del modelo sobre el marcador: ~9 cm, salvo los muy
        /// alargados (la secuencia de la insulina es una tira de estaciones), que con 9 cm quedaban
        /// diminutos: esos van a 15 cm.</summary>
        public static float TamanoEnAR(Bounds b)
        {
            var s = b.size;
            float mayor = Mathf.Max(s.x, s.y, s.z);
            float medio = s.x + s.y + s.z - mayor - Mathf.Min(s.x, s.y, s.z);
            return medio > 0f && mayor / medio > 2.5f ? 0.15f : 0.09f;
        }

        /// <summary>
        /// Caja que ocupa el modelo. En los animados la pose de reposo del FBX no se parece a lo
        /// que se ve (piezas que aparecen, caen o se escalan), así que se junta la caja de varios
        /// momentos del clip y se vuelve al inicio.
        /// </summary>
        public static bool Limites(GameObject model, out Bounds b)
        {
            b = default;
            var rends = model.GetComponentsInChildren<Renderer>();
            if (rends.Length == 0) return false;
            var anim = model.GetComponentInChildren<Animation>();
            var clip = anim != null ? anim.clip : null;
            bool hay = false;
            int muestras = clip != null ? 8 : 1;
            for (int i = 0; i < muestras; i++)
            {
                if (clip != null) clip.SampleAnimation(anim.gameObject, clip.length * i / muestras);
                foreach (var r in rends)
                {
                    if (!r.enabled || r.bounds.size == Vector3.zero) continue;
                    if (!hay) { b = r.bounds; hay = true; } else b.Encapsulate(r.bounds);
                }
            }
            if (clip != null) clip.SampleAnimation(anim.gameObject, 0f);
            return hay;
        }

        /// <summary>Reproduce en bucle el clip Legacy del modelo, si trae uno.</summary>
        public static void IniciarAnimacion(GameObject model)
        {
            var anim = model.GetComponentInChildren<Animation>();
            if (anim == null || anim.clip == null) return;
            // El de autocuidado es interactivo: su componente decide cuándo corre y cuándo se detiene el clip.
            if (Autocuidado.Corresponde(model)) { if (model.GetComponent<Autocuidado>() == null) model.AddComponent<Autocuidado>(); return; }
            anim.cullingType = AnimationCullingType.AlwaysAnimate;
            anim.wrapMode = WrapMode.Loop;
            anim.Play(anim.clip.name);
        }

        GameObject prefabActual, modelo;
        Texture paletaActual;

        /// <summary>Cambia el modelo del visor (p. ej. al tema elegido); no hace nada si ya es ese.</summary>
        public void MostrarModelo(GameObject prefab, Color tint, Texture paleta = null)
        {
            if (prefab == null || holder == null || (prefab == prefabActual && paleta == paletaActual)) return;
            ColocarModelo(prefab, tint, paleta);
        }

        void ColocarModelo(GameObject prefab, Color tint, Texture paleta = null)
        {
            if (modelo != null) Destroy(modelo);
            prefabActual = prefab;
            paletaActual = paleta;
            var rotacion = holder.localRotation;
            holder.localRotation = Quaternion.identity;
            holder.localScale = Vector3.one;

            GameObject model = prefab != null ? Instantiate(prefab) : GameObject.CreatePrimitive(PrimitiveType.Capsule);
            modelo = model;
            model.transform.SetParent(holder, false);
            model.transform.localPosition = Vector3.zero;
            model.transform.localRotation = Quaternion.identity;
            model.transform.localScale = Vector3.one; // el prefab puede traer escala no uniforme; normalizar antes de medir bounds
            // Los del libro Clínico están armados para verse desde arriba (sobre la página): en el visor
            // se inclinan hacia la cámara. El de autocuidado además se gira para leer su calendario.
            if (prefab != null && prefab.name.StartsWith("Clin_"))
                model.transform.localRotation = Quaternion.Euler(Autocuidado.Corresponde(prefab) ? -60f : -35f, 0f, 0f)
                    * Quaternion.Euler(0f, Autocuidado.Corresponde(prefab) ? 180f : 0f, 0f);
            SetLayer(model, Layer);
            StripEmbeddedCamerasAndLights(model);

            AplicarMateriales(model, tint, 0.25f, paleta);

            // Auto-encuadre: centrar y escalar a ~2 unidades (uniforme, ya con localScale=1 arriba).
            if (Limites(model, out var b))
            {
                model.transform.position += holder.position - b.center;
                float size = Mathf.Max(b.size.x, b.size.y, b.size.z);
                if (size > 0.0001f) holder.localScale = Vector3.one * (2f / size);
            }
            IniciarAnimacion(model);
            holder.localRotation = rotacion;
            // Sin auto-giro en el interactivo: se toca cada actividad y girando costaría acertarle.
            if (spinner != null) spinner.spinning = !Autocuidado.Corresponde(model);
        }

        void Build(GameObject prefab, Color tint)
        {
            var holderGo = new GameObject("Holder");
            holderGo.transform.SetParent(transform, false);
            holder = holderGo.transform;

            ColocarModelo(prefab, tint);

            spinner = holderGo.AddComponent<Spinner>();

            var camGo = new GameObject("PreviewCam");
            camGo.transform.SetParent(transform, false);
            cam = camGo.AddComponent<Camera>();
            cam.cullingMask = 1 << Layer;
            cam.clearFlags = CameraClearFlags.SolidColor;
            cam.backgroundColor = new Color(0, 0, 0, 0);
            cam.fieldOfView = 45f;
            cam.nearClipPlane = 0.05f;
            cam.transform.position = transform.position + new Vector3(0, 0.4f, -3.2f);
            cam.transform.LookAt(transform.position + new Vector3(0, 0.1f, 0));

            Texture = new RenderTexture(800, 900, 16) { name = "ModelRT" };
            cam.targetTexture = Texture;

            var lightGo = new GameObject("PreviewLight");
            lightGo.transform.SetParent(transform, false);
            var l = lightGo.AddComponent<Light>();
            l.type = LightType.Directional;
            l.cullingMask = 1 << Layer;
            l.intensity = 1.1f;
            l.transform.rotation = Quaternion.Euler(40f, -35f, 0f);

            var fill = new GameObject("FillLight").AddComponent<Light>();
            fill.transform.SetParent(transform, false);
            fill.type = LightType.Directional;
            fill.cullingMask = 1 << Layer;
            fill.intensity = 0.5f;
            fill.transform.rotation = Quaternion.Euler(-20f, 140f, 0f);

            // Que la cámara principal NO dibuje el modelo.
            if (Camera.main != null) Camera.main.cullingMask &= ~(1 << Layer);

            // Apagada por defecto: sin esto, cada libro sumaba una cámara renderizando su
            // RenderTexture en TODO momento (incluso en pantallas donde nunca se ve), lo que
            // se notaba como lentitud general de toda la app al pasar de 1 a 3 libros.
            cam.enabled = false;
        }

        public void ToggleSpin() { if (spinner != null) spinner.spinning = !spinner.spinning; }

        public void GirarPaso(float degrees) { if (spinner != null) spinner.spinning = false; AddYaw(degrees); }

        float userScale = 1f;

        public void CambiarTamano(float paso)
        {
            float nuevo = Mathf.Clamp(userScale + paso, .7f, 1.4f);
            if (holder != null) holder.localScale *= nuevo / userScale;
            userScale = nuevo;
        }

        /// <summary>Toque sobre el RawImage que muestra este visor: lo pasa al modelo interactivo, si hay.</summary>
        public bool Tocar(RectTransform imagen, Vector2 pantalla, Camera camUI)
        {
            var auto = modelo != null ? modelo.GetComponent<Autocuidado>() : null;
            if (auto == null || cam == null || imagen == null) return false;
            if (!RectTransformUtility.ScreenPointToLocalPointInRectangle(imagen, pantalla, camUI, out var local)) return false;
            var r = imagen.rect;
            var uv = new Vector2((local.x - r.xMin) / r.width, (local.y - r.yMin) / r.height);
            if (uv.x < 0 || uv.x > 1 || uv.y < 0 || uv.y > 1) return false;
            return auto.Tocar(cam.ViewportPointToRay(uv));
        }

        /// <summary>Enciende/apaga el render de este visor. Solo debe estar activo el del libro
        /// que se está mostrando en ese momento.</summary>
        public void SetActive(bool on) { if (cam != null) cam.enabled = on; }

        /// <summary>Rota el modelo manualmente (arrastre táctil), independiente del auto-giro.</summary>
        public void AddYaw(float degrees) { if (holder != null) holder.Rotate(Vector3.up, degrees, Space.World); }

        static void SetLayer(GameObject go, int layer)
        {
            go.layer = layer;
            foreach (Transform t in go.transform) SetLayer(t.gameObject, layer);
        }

        // Modelos exportados de Sketchfab/Meshy suelen traer incrustada la cámara (y a veces luces)
        // de su escena de origen. Si se dejan activas, compiten con nuestras propias cámaras
        // (Skybox por defecto tapando el feed real de la cámara AR). Se eliminan al instanciar.
        public static void StripEmbeddedCamerasAndLights(GameObject model)
        {
            foreach (var cam in model.GetComponentsInChildren<Camera>(true)) UnityEngine.Object.Destroy(cam);
            foreach (var lt in model.GetComponentsInChildren<Light>(true)) UnityEngine.Object.Destroy(lt);
            foreach (var al in model.GetComponentsInChildren<AudioListener>(true)) UnityEngine.Object.Destroy(al);
        }
    }
}
