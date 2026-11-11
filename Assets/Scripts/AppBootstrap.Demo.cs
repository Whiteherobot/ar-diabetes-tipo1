using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace ARDiabetes
{
    // ============================================================
    // ARDEMO=<carpeta>: recorrido automático de toda la app, grabado cuadro a cuadro desde el
    // player (JPG a 30 fps en <carpeta>, para armar el video con ffmpeg). Solo para la demo:
    // sin la variable no hace nada. La app está en 6 escenas: el paso actual es estático y la
    // demo sigue en la escena nueva; la grabadora vive en DontDestroyOnLoad.
    // En las pantallas AR se usa el visor 3D (en escritorio no hay cámara AR).
    // ============================================================
    public partial class AppBootstrap
    {
        static string DemoDir => Environment.GetEnvironmentVariable("ARDEMO");
        static bool EnDemo => !string.IsNullOrEmpty(DemoDir);
        static int demoPaso;
        bool demoEnEstaEscena;

        void IniciarDemoSiCorresponde()
        {
            if (!EnDemo || demoEnEstaEscena) return;
            demoEnEstaEscena = true;
            DemoGrabadora.Asegurar(DemoDir);
            StartCoroutine(Demo());
        }

        IEnumerator Demo()
        {
            yield return Esperar(0.6f);
            switch (demoPaso)
            {
                case 0: // Inicio, bienvenida y perfil
                    yield return Esperar(2f);
                    yield return Tocar("Comenzar");
                    for (int i = 0; i < 3; i++) { yield return Esperar(2f); yield return Tocar("Siguiente"); }
                    yield return Esperar(1.2f);
                    var campo = panels[shown].GetComponentInChildren<TMP_InputField>();
                    if (campo != null)
                    {
                        yield return TocarObjeto((RectTransform)campo.transform, null);
                        foreach (char c in "Sofía") { campo.text += c; yield return Esperar(0.18f); }
                    }
                    yield return Esperar(0.8f);
                    yield return TocarObjeto(BotonConSprite(bustos[2]), null);
                    yield return Esperar(1.5f);
                    demoPaso = 1;
                    yield return Tocar("CONTINUAR");
                    yield break;

                case 1: // Inicio (menú) y mundos
                    yield return Esperar(2.5f);
                    yield return Desplazar(1f, 5f);
                    yield return Esperar(1f);
                    yield return Desplazar(0f, 2f);
                    yield return Tocar("ExploraAhora");
                    yield return Esperar(2.5f);
                    demoPaso = 2;
                    yield return Tocar("Mundo1");
                    yield break;

                case 2: // Libro Nutricional: los 3 modelos animados
                    string[] temas = { "Plato saludable", "Carbohidratos", "Alimentos recomendados" };
                    for (int t = 0; t < temas.Length; t++)
                    {
                        if (t > 0) { ShowOnly(bookTemas[1]); yield return Esperar(1.2f); }
                        else yield return Esperar(2f);
                        yield return Tocar(temas[t]);
                        yield return Esperar(1.5f);
                        yield return Desplazar(1f, 1.5f);
                        yield return Tocar("EXPLORAR EN RA");
                        yield return Esperar(t == 0 ? 9f : 7f);
                        if (t == 0)
                        {
                            yield return TocarObjeto(null, () => { modelViewer?.CambiarTamano(.1f); });
                            yield return Esperar(1f);
                            yield return TocarObjeto(null, () => { modelViewer?.GirarPaso(25f); });
                            yield return Esperar(2f);
                        }
                    }
                    demoPaso = 3;
                    ShowOnly(pJuegos);
                    yield break;

                case 3: // Juegos: una misión completa
                    yield return Esperar(2.5f);
                    yield return Tocar("JUGAR");
                    yield return Esperar(2f);
                    var quiz = panels[shown].GetComponent<QuizRefs>();
                    for (int q = 0; quiz != null && q < books[quizBook].Quiz.Length; q++)
                    {
                        var item = books[quizBook].Quiz[quizQ];
                        for (int k = 0; k < 4; k++)
                            if (quiz.Orden[k] == item.Correct) { yield return TocarObjeto((RectTransform)quiz.Opciones[k].Btn.transform, null); break; }
                        yield return Esperar(2f);
                        yield return Tocar(q == books[quizBook].Quiz.Length - 1 ? "VER RESULTADO" : "CONTINUAR");
                        yield return Esperar(1.5f);
                    }
                    yield return Esperar(2f);
                    demoPaso = 4;
                    ShowOnly(pProgreso);
                    yield break;

                case 4: // Progreso, insignias y colección
                    yield return Esperar(2.5f);
                    yield return Desplazar(1f, 3f);
                    yield return Esperar(1f);
                    ShowOnly(pInsignias); yield return Esperar(2.5f);
                    ShowOnly(pColeccion); yield return Esperar(2.5f);
                    demoPaso = 5;
                    ShowOnly(pConfig);
                    yield break;

                case 5: // Mi espacio y ayuda
                    yield return Esperar(2f);
                    yield return Tocar("Grande");
                    yield return Esperar(2f);
                    yield return Tocar("Normal");
                    yield return Esperar(1f);
                    yield return Desplazar(1f, 3f);
                    yield return Esperar(1f);
                    ShowOnly(pAyuda);
                    yield return Esperar(2f);
                    yield return Desplazar(.5f, 2.5f);
                    yield return Esperar(1.5f);
                    demoPaso = 6;
                    ShowOnly(pMenu);
                    yield break;

                default: // vuelta al inicio y fin
                    yield return Esperar(3f);
                    DemoGrabadora.Terminar();
                    Application.Quit();
                    yield break;
            }
        }

        static IEnumerator Esperar(float s) { yield return new WaitForSeconds(s); }

        // Busca en la pantalla visible un botón por nombre de objeto o por texto, y lo pulsa
        // mostrando el toque.
        IEnumerator Tocar(string objetivo)
        {
            var rt = BuscarBoton(objetivo);
            if (rt == null) { Debug.LogWarning("[Demo] no encontré «" + objetivo + "»"); yield break; }
            yield return TocarObjeto(rt, null);
        }

        IEnumerator TocarObjeto(RectTransform rt, Action accion)
        {
            Vector2 pos = new Vector2(Screen.width * .5f, Screen.height * .45f);
            if (rt != null)
            {
                var c = new Vector3[4]; rt.GetWorldCorners(c);
                pos = (c[0] + c[2]) * .5f;
            }
            DemoGrabadora.MostrarToque(pos);
            yield return Esperar(0.35f);
            if (accion != null) accion();
            else if (rt != null)
            {
                var btn = rt.GetComponentInParent<Button>();
                if (btn != null) btn.onClick.Invoke();
                else rt.GetComponentInParent<TMP_InputField>()?.Select();
            }
            yield return Esperar(0.3f);
        }

        RectTransform BuscarBoton(string objetivo)
        {
            var raiz = panels[shown];
            foreach (var b in raiz.GetComponentsInChildren<Button>())
                if (b.name == objetivo && b.gameObject.activeInHierarchy) return (RectTransform)b.transform;
            foreach (var t in raiz.GetComponentsInChildren<TMP_Text>())
            {
                if (!t.gameObject.activeInHierarchy || t.text == null) continue;
                if (t.text.Trim().Equals(objetivo, StringComparison.OrdinalIgnoreCase) ||
                    t.text.StartsWith(objetivo, StringComparison.OrdinalIgnoreCase))
                {
                    var b = t.GetComponentInParent<Button>();
                    if (b != null) return (RectTransform)b.transform;
                }
            }
            return null;
        }

        RectTransform BotonConSprite(Sprite s)
        {
            foreach (var im in panels[shown].GetComponentsInChildren<Image>())
                if (im.sprite == s && im.gameObject.activeInHierarchy)
                {
                    var b = im.GetComponentInParent<Button>();
                    if (b != null) return (RectTransform)b.transform;
                }
            return null;
        }

        // Desliza el primer scroll vertical de la pantalla hasta «destino» (0 = arriba, 1 = abajo).
        IEnumerator Desplazar(float destino, float duracion)
        {
            var sr = panels[shown].GetComponentInChildren<ScrollRect>();
            if (sr == null) yield break;
            float inicio = 1f - sr.verticalNormalizedPosition, t = 0f;
            while (t < duracion)
            {
                t += Time.deltaTime;
                float k = Mathf.SmoothStep(0f, 1f, Mathf.Clamp01(t / duracion));
                sr.verticalNormalizedPosition = 1f - Mathf.Lerp(inicio, destino, k);
                yield return null;
            }
        }
    }

    /// <summary>Graba cada cuadro del player (y dibuja el toque) mientras dura la demo.</summary>
    public class DemoGrabadora : MonoBehaviour
    {
        static DemoGrabadora instancia;
        string dir;
        int cuadro;
        Image toque;
        float toqueT = -1f;

        public static void Asegurar(string carpeta)
        {
            if (instancia != null) return;
            var go = new GameObject("DemoGrabadora");
            DontDestroyOnLoad(go);
            instancia = go.AddComponent<DemoGrabadora>();
            instancia.dir = carpeta;
            System.IO.Directory.CreateDirectory(carpeta);
            Time.captureFramerate = 30; // tiempo de juego fijo: el video sale fluido aunque grabar sea lento

            var canvas = go.AddComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            canvas.sortingOrder = 32000;
            var img = new GameObject("Toque", typeof(RectTransform), typeof(CanvasRenderer), typeof(Image));
            img.transform.SetParent(go.transform, false);
            instancia.toque = img.GetComponent<Image>();
            instancia.toque.sprite = UIKit.Circle();
            instancia.toque.raycastTarget = false;
            instancia.toque.color = new Color(1, 1, 1, 0);
            instancia.StartCoroutine(instancia.Grabar());
        }

        public static void MostrarToque(Vector2 pantalla)
        {
            if (instancia == null) return;
            instancia.toque.rectTransform.position = pantalla;
            instancia.toqueT = 0f;
        }

        public static void Terminar()
        {
            if (instancia != null) instancia.StopAllCoroutines();
        }

        void Update()
        {
            if (toqueT < 0f) return;
            toqueT += Time.deltaTime;
            float k = Mathf.Clamp01(toqueT / 0.6f);
            float lado = Screen.height * .045f * (1f + .5f * k);
            toque.rectTransform.sizeDelta = new Vector2(lado, lado);
            toque.color = new Color(.2f, .54f, .93f, .55f * (1f - k));
            if (k >= 1f) toqueT = -1f;
        }

        IEnumerator Grabar()
        {
            while (true)
            {
                yield return new WaitForEndOfFrame();
                var tex = ScreenCapture.CaptureScreenshotAsTexture();
                System.IO.File.WriteAllBytes(System.IO.Path.Combine(dir, "f_" + cuadro.ToString("00000") + ".jpg"), tex.EncodeToJPG(92));
                Destroy(tex);
                cuadro++;
            }
        }
    }
}
