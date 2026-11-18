using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ARDiabetes
{
    /// <summary>
    /// RA 03 «Organizo mi autocuidado» (Clin_Autocuidado.fbx): RoboGluc presenta un calendario y cinco
    /// actividades. El clip del cliente las lleva solas al calendario, así que aquí se reproduce solo la
    /// presentación (hasta que las cinco esperan al costado) y desde ahí manda el niño: al tocar una
    /// actividad vuela a su casilla y aparece su marca; con las cinco sale «rutina completa» y luego la
    /// felicitación final. El clip queda detenido mientras tanto, para que no pise lo que hizo el usuario.
    /// Las poses de destino se toman del propio clip (no hay números a mano).
    /// </summary>
    public class Autocuidado : MonoBehaviour
    {
        static readonly string[] Actividades = { "Alimentacion", "Medicion", "Tratamiento", "Actividad", "Descanso" };

        // Momentos del clip (20 s, 30 fps): a los 7,8 s las cinco actividades ya aparecieron y esperan;
        // a los 13,3 s se ve «rutina completa» y al final la felicitación con los destellos.
        const float TEspera = 7.8f, TMensaje = 13.3f, TFinal = 19.9f;

        struct PoseLocal
        {
            public Vector3 Pos, Escala; public Quaternion Rot;
            public static PoseLocal De(Transform t) => new PoseLocal { Pos = t.localPosition, Rot = t.localRotation, Escala = t.localScale };
            public void Aplicar(Transform t) { t.localPosition = Pos; t.localRotation = Rot; t.localScale = Escala; }
        }

        class Pieza
        {
            public Transform Accion, Marca;
            public PoseLocal Espera, Destino;
            public Vector3 EscalaMarca;
            public BoxCollider Zona;
            public bool Hecha;
        }

        enum Fase { Intro, Esperando, Final }

        Animation anim;
        AnimationClip clip;
        readonly List<Pieza> piezas = new List<Pieza>();
        Transform mensajeCompleto, mensajeFinal;
        PoseLocal poseCompleto, poseFinal;
        readonly List<Transform> destellos = new List<Transform>();
        readonly List<PoseLocal> posesDestello = new List<PoseLocal>();
        Fase fase;
        float finDesde;

        public static bool Corresponde(GameObject model) => model != null && model.name.StartsWith("Clin_Autocuidado");

        void Start()
        {
            anim = GetComponentInChildren<Animation>();
            clip = anim != null ? anim.clip : null;
            if (clip == null) { enabled = false; return; }
            var raiz = anim.gameObject;

            foreach (var nombre in Actividades)
            {
                var a = Buscar(raiz.transform, "Action_" + nombre);
                var c = Buscar(raiz.transform, "Check_" + nombre);
                if (a == null || c == null) continue;
                piezas.Add(new Pieza { Accion = a, Marca = c });
            }
            mensajeCompleto = Buscar(raiz.transform, "Message_Complete");
            mensajeFinal = Buscar(raiz.transform, "Message_End");
            foreach (var t in raiz.GetComponentsInChildren<Transform>(true))
                if (t.name.StartsWith("Destello")) destellos.Add(t);

            // Poses de destino, sacadas del clip.
            clip.SampleAnimation(raiz, TFinal);
            foreach (var p in piezas) { p.Destino = PoseLocal.De(p.Accion); p.EscalaMarca = p.Marca.localScale; }
            if (mensajeFinal != null) poseFinal = PoseLocal.De(mensajeFinal);
            foreach (var d in destellos) posesDestello.Add(PoseLocal.De(d));
            clip.SampleAnimation(raiz, TMensaje);
            if (mensajeCompleto != null) poseCompleto = PoseLocal.De(mensajeCompleto);
            clip.SampleAnimation(raiz, TEspera);
            foreach (var p in piezas) p.Espera = PoseLocal.De(p.Accion);

            Reiniciar();
        }

        static Transform Buscar(Transform raiz, string nombre)
        {
            foreach (var t in raiz.GetComponentsInChildren<Transform>(true)) if (t.name == nombre) return t;
            return null;
        }

        void Reiniciar()
        {
            StopAllCoroutines();
            foreach (var p in piezas) p.Hecha = false;
            fase = Fase.Intro;
            anim.cullingType = AnimationCullingType.AlwaysAnimate;
            anim.wrapMode = WrapMode.Once;
            anim.Stop();
            anim.Play(clip.name);
        }

        void Update()
        {
            if (fase == Fase.Intro)
            {
                var estado = anim[clip.name];
                if (estado != null && estado.time < TEspera && anim.isPlaying) return;
                anim.Stop();
                clip.SampleAnimation(anim.gameObject, TEspera);
                foreach (var p in piezas) PrepararZona(p);
                fase = Fase.Esperando;
            }
            if (fase == Fase.Esperando)
            {
                // Las pendientes «laten» un poco para que se note que se pueden tocar.
                float latido = 1f + .07f * Mathf.Sin(Time.time * 4.5f);
                foreach (var p in piezas) if (!p.Hecha) p.Accion.localScale = p.Espera.Escala * latido;
            }
        }

        // Zona táctil generosa alrededor de cada actividad (la caja de sus piezas, un poco más grande).
        void PrepararZona(Pieza p)
        {
            var e = p.Espera; e.Aplicar(p.Accion);
            bool hay = false; var caja = new Bounds();
            foreach (var r in p.Accion.GetComponentsInChildren<Renderer>())
            {
                var b = r.bounds;
                for (int i = 0; i < 8; i++)
                {
                    var esquina = new Vector3((i & 1) == 0 ? b.min.x : b.max.x, (i & 2) == 0 ? b.min.y : b.max.y, (i & 4) == 0 ? b.min.z : b.max.z);
                    var local = p.Accion.InverseTransformPoint(esquina);
                    if (!hay) { caja = new Bounds(local, Vector3.zero); hay = true; } else caja.Encapsulate(local);
                }
            }
            if (!hay) return;
            if (p.Zona == null) p.Zona = p.Accion.gameObject.AddComponent<BoxCollider>();
            p.Zona.center = caja.center;
            p.Zona.size = caja.size * 1.4f;
            p.Zona.enabled = true;
        }

        /// <summary>Toque del usuario (rayo desde la cámara que muestra el modelo). Devuelve true si
        /// lo usó. Durante la presentación, tocar adelanta hasta el momento de elegir.</summary>
        public bool Tocar(Ray rayo)
        {
            if (!isActiveAndEnabled || clip == null) return false;
            if (fase == Fase.Intro)
            {
                var estado = anim[clip.name];
                if (estado != null && estado.time > 4f) { estado.time = TEspera; return true; }
                return false;
            }
            if (fase == Fase.Final)
            {
                if (Time.time - finDesde < 2f) return false;
                Reiniciar();
                return true;
            }
            Pieza tocada = null; float mejor = float.MaxValue;
            foreach (var p in piezas)
            {
                if (p.Hecha || p.Zona == null) continue;
                if (p.Zona.Raycast(rayo, out var hit, 1000f) && hit.distance < mejor) { mejor = hit.distance; tocada = p; }
            }
            if (tocada == null) return false;
            tocada.Hecha = true;
            tocada.Zona.enabled = false;
            StartCoroutine(Colocar(tocada));
            return true;
        }

        IEnumerator Colocar(Pieza p)
        {
            var desde = PoseLocal.De(p.Accion);
            const float dur = .6f;
            for (float t = 0; t < dur; t += Time.deltaTime)
            {
                float k = Mathf.SmoothStep(0, 1, t / dur);
                float salto = Mathf.Sin(k * Mathf.PI) * .25f; // crece un poco a mitad de camino
                p.Accion.localPosition = Vector3.Lerp(desde.Pos, p.Destino.Pos, k);
                p.Accion.localRotation = Quaternion.Slerp(desde.Rot, p.Destino.Rot, k);
                p.Accion.localScale = Vector3.Lerp(desde.Escala, p.Destino.Escala, k) * (1f + salto);
                yield return null;
            }
            p.Destino.Aplicar(p.Accion);
            yield return Aparecer(p.Marca, p.EscalaMarca, .35f);

            bool todas = true;
            foreach (var q in piezas) if (!q.Hecha) { todas = false; break; }
            if (todas && fase == Fase.Esperando) { fase = Fase.Final; finDesde = Time.time; StartCoroutine(Celebrar()); }
        }

        IEnumerator Celebrar()
        {
            yield return new WaitForSeconds(.4f);
            if (mensajeCompleto != null)
            {
                poseCompleto.Aplicar(mensajeCompleto);
                yield return Aparecer(mensajeCompleto, poseCompleto.Escala, .45f);
                yield return new WaitForSeconds(2.4f);
                yield return Escalar(mensajeCompleto, poseCompleto.Escala, Vector3.zero, .3f);
            }
            if (mensajeFinal != null)
            {
                poseFinal.Aplicar(mensajeFinal);
                StartCoroutine(Aparecer(mensajeFinal, poseFinal.Escala, .5f));
            }
            for (int i = 0; i < destellos.Count; i++)
            {
                posesDestello[i].Aplicar(destellos[i]);
                StartCoroutine(Aparecer(destellos[i], posesDestello[i].Escala, .4f));
                yield return new WaitForSeconds(.15f);
            }
            finDesde = Time.time;
        }

        // Aparece desde cero con un pequeño rebote.
        static IEnumerator Aparecer(Transform t, Vector3 escala, float dur)
        {
            for (float s = 0; s < dur; s += Time.deltaTime)
            {
                float k = s / dur;
                float rebote = k < .7f ? Mathf.Lerp(0f, 1.15f, k / .7f) : Mathf.Lerp(1.15f, 1f, (k - .7f) / .3f);
                t.localScale = escala * rebote;
                yield return null;
            }
            t.localScale = escala;
        }

        static IEnumerator Escalar(Transform t, Vector3 a, Vector3 b, float dur)
        {
            for (float s = 0; s < dur; s += Time.deltaTime) { t.localScale = Vector3.Lerp(a, b, s / dur); yield return null; }
            t.localScale = b;
        }
    }
}
