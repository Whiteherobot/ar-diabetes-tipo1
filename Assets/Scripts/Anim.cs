using System.Collections;
using UnityEngine;

namespace ARDiabetes
{
    /// <summary>
    /// Animaciones del diseño (mismas duraciones y curvas que los keyframes del CSS).
    ///  - Entrada: corre una vez al activarse (opacidad, desplazamiento, escala y giro → reposo).
    ///  - Bucle: oscila sin fin entre el reposo y un desplazamiento/giro/escala.
    ///  - Llenar: hace crecer una barra (anchorMax.x) desde cero hasta su valor.
    ///  - PopEstrella: escala 0.25 → 1.25 → 1 con giro, como star-pop.
    /// Los px están en unidades del Canvas (px de Figma × 2.77). y positivo = hacia arriba.
    /// No combinar con Appear en el mismo objeto (ambos mueven anchoredPosition).
    /// </summary>
    public class Anim : MonoBehaviour
    {
        public enum Tipo { Entrada, Bucle, Llenar, PopEstrella, Desvanecer }

        public Tipo tipo = Tipo.Entrada;
        public float dur = 0.35f;
        public float delay = 0f;
        public float alphaDesde = 1f;
        public Vector2 mover;          // Entrada: desde dónde llega. Bucle: hacia dónde va.
        public float escala = 1f;      // Entrada: escala inicial
        public float giro = 0f;        // Entrada: giro inicial (grados)
        // Bucle: va del estado A (giroA, escalaA, alphaA, sin mover) al B (mover, giroB, escalaB, opaco)
        public float giroA = 0f, giroB = 0f;
        public Vector2 escalaA = Vector2.one, escalaB = Vector2.one;
        public float alphaA = 1f;
        public bool alterna = false;   // true = ida y vuelta (alternate); false = 0%-50%-100%

        RectTransform rt;
        CanvasGroup cg;
        Vector2 basePos;
        Vector3 baseScale;
        float baseAnchorMaxX;
        bool listo;

        public const float S = 2.77f;

        void Init()
        {
            if (listo) return;
            rt = (RectTransform)transform;
            basePos = rt.anchoredPosition;
            baseScale = rt.localScale;
            baseAnchorMaxX = rt.anchorMax.x;
            listo = true;
        }

        void OnEnable()
        {
            Init();
            StopAllCoroutines();
            switch (tipo)
            {
                case Tipo.Entrada: StartCoroutine(Entrada()); break;
                case Tipo.Bucle: StartCoroutine(Bucle()); break;
                case Tipo.Llenar: StartCoroutine(Llenar()); break;
                case Tipo.PopEstrella: StartCoroutine(PopEstrella()); break;
                case Tipo.Desvanecer: StartCoroutine(Desvanecer()); break;
            }
        }

        void OnDisable() { if (listo) Reposo(); }

        // Vuelve a correr la animación (p. ej. al cambiar de pregunta o al actualizar una barra)
        public void Reproducir() { if (isActiveAndEnabled) OnEnable(); }

        // La barra cambia de valor en tiempo de ejecución: se llama antes de animar
        public void FijarLlenado(float anchorMaxX) { Init(); baseAnchorMaxX = anchorMaxX; }

        void Reposo()
        {
            rt.anchoredPosition = basePos;
            rt.localScale = baseScale;
            rt.localRotation = Quaternion.identity;
            if (cg != null) cg.alpha = 1f;
            if (tipo == Tipo.Llenar) rt.anchorMax = new Vector2(baseAnchorMaxX, rt.anchorMax.y);
        }

        CanvasGroup Grupo()
        {
            if (cg == null) cg = GetComponent<CanvasGroup>();
            if (cg == null) cg = gameObject.AddComponent<CanvasGroup>();
            return cg;
        }

        static float EaseOut(float k) => 1f - (1f - k) * (1f - k) * (1f - k);
        static float EaseInOut(float k) => k * k * (3f - 2f * k);

        void Aplicar(Vector2 off, float sc, float sx, float sy, float rot)
        {
            rt.anchoredPosition = basePos + off;
            rt.localScale = new Vector3(baseScale.x * sc * sx, baseScale.y * sc * sy, baseScale.z);
            rt.localRotation = Quaternion.Euler(0, 0, rot);
        }

        IEnumerator Entrada()
        {
            var g = alphaDesde < 1f ? Grupo() : null;
            Aplicar(mover, escala, 1, 1, giro);
            if (g != null) g.alpha = alphaDesde;
            for (float t = -delay; t < dur; t += Time.unscaledDeltaTime)
            {
                float e = EaseOut(Mathf.Clamp01(t / dur));
                Aplicar(Vector2.LerpUnclamped(mover, Vector2.zero, e), Mathf.Lerp(escala, 1f, e), 1, 1, Mathf.Lerp(giro, 0f, e));
                if (g != null) g.alpha = Mathf.Lerp(alphaDesde, 1f, e);
                yield return null;
            }
            Reposo();
        }

        IEnumerator Bucle()
        {
            var g = alphaA < 1f ? Grupo() : null;
            for (float t = delay; ; t += Time.unscaledDeltaTime) // en bucle, delay = desfase
            {
                // alternate: A→B en dur y vuelve; normal: A en 0% y 100%, B en 50%
                float k = EaseInOut(alterna ? Mathf.PingPong(t / dur, 1f) : Mathf.PingPong(t / (dur * 0.5f), 1f));
                var sc = Vector2.Lerp(escalaA, escalaB, k);
                Aplicar(Vector2.Lerp(Vector2.zero, mover, k), 1f, sc.x, sc.y, Mathf.Lerp(giroA, giroB, k));
                if (g != null) g.alpha = Mathf.Lerp(alphaA, 1f, k);
                yield return null;
            }
        }

        // flash: opacidad 1 → 0 (destello de la cámara)
        IEnumerator Desvanecer()
        {
            var g = Grupo();
            for (float t = 0; t < dur; t += Time.unscaledDeltaTime) { g.alpha = 1f - EaseOut(t / dur); yield return null; }
            g.alpha = 0f;
        }

        IEnumerator Llenar()
        {
            float x0 = rt.anchorMin.x;
            for (float t = -delay; t < dur; t += Time.unscaledDeltaTime)
            {
                float e = EaseOut(Mathf.Clamp01(t / dur));
                rt.anchorMax = new Vector2(Mathf.Lerp(x0, baseAnchorMaxX, e), rt.anchorMax.y);
                yield return null;
            }
            Reposo();
        }

        IEnumerator PopEstrella()
        {
            var g = Grupo();
            for (float t = -delay; t < dur; t += Time.unscaledDeltaTime)
            {
                float k = Mathf.Clamp01(t / dur);
                float sc, rot;
                if (k < 0.7f) { float e = EaseOut(k / 0.7f); sc = Mathf.Lerp(.25f, 1.25f, e); rot = Mathf.Lerp(-25f, 8f, e); g.alpha = e; }
                else { float e = EaseOut((k - 0.7f) / 0.3f); sc = Mathf.Lerp(1.25f, 1f, e); rot = Mathf.Lerp(8f, 0f, e); g.alpha = 1f; }
                Aplicar(Vector2.zero, sc, 1, 1, -rot); // en CSS el giro positivo es horario
                yield return null;
            }
            Reposo();
        }

        // ---------- Presets (keyframes del CSS) ----------
        static Anim Add(Component c) { var a = c.gameObject.AddComponent<Anim>(); return a; }

        // robot-float 3.4s infinite: sube 7px y vuelve
        public static Anim Flotar(Component c) { var a = Add(c); a.tipo = Tipo.Bucle; a.dur = 3.4f; a.mover = new Vector2(0, 7 * S); return a; }
        // robot-celebrate .8s alternate: rotate(-3deg) → rotate(3deg) y sube 5px (en CSS el giro positivo es horario)
        public static Anim Celebrar(Component c) { var a = Add(c); a.tipo = Tipo.Bucle; a.dur = .8f; a.alterna = true; a.giroA = 3f; a.giroB = -3f; a.mover = new Vector2(0, 5 * S); return a; }
        // star-drift 2.8s infinite: rotate(-7deg) ↔ rotate(7deg) y sube 5px
        public static Anim Deriva(Component c) { var a = Add(c); a.tipo = Tipo.Bucle; a.dur = 2.8f; a.giroA = 7f; a.giroB = -7f; a.mover = new Vector2(0, 5 * S); return a; }
        // help-wave .8s alternate: scaleY .55 → 1.15, opacidad .6 → 1 (las barritas se desfasan con delay)
        public static Anim Onda(Component c) { var a = Add(c); a.tipo = Tipo.Bucle; a.dur = .8f; a.alterna = true; a.escalaA = new Vector2(1f, .55f); a.escalaB = new Vector2(1f, 1.15f); a.alphaA = .6f; return a; }
        // audio-pulse 1s alternate: scaleX .35 → 1, opacidad .5 → 1
        public static Anim Pulso(Component c) { var a = Add(c); a.tipo = Tipo.Bucle; a.dur = 1f; a.alterna = true; a.escalaA = new Vector2(.35f, 1f); a.alphaA = .5f; return a; }

        // Entradas genéricas
        public static Anim Entrar(Component c, float dur, float dy = 0f, float dx = 0f, float escala = 1f, float giro = 0f, float alpha = 0f, float delay = 0f)
        {
            var a = Add(c); a.tipo = Tipo.Entrada; a.dur = dur; a.delay = delay;
            a.mover = new Vector2(dx * S, -dy * S); a.escala = escala; a.giro = -giro; a.alphaDesde = alpha; return a;
        }
        public static Anim PantallaEntra(Component c) => Entrar(c, .22f, dy: 4f, alpha: .35f);               // screen-in
        public static Anim HeroLlega(Component c, float d = 0f) => Entrar(c, .55f, dy: 18f, escala: .94f, delay: d); // hero-arrive
        public static Anim TarjetaSube(Component c, float d = 0f) => Entrar(c, .35f, dy: 8f, delay: d);         // card-rise
        public static Anim RobotEntra(Component c) => Entrar(c, .45f, dy: 15f, giro: 4f);                       // help/settings-robot-in
        public static Anim RespuestaEntra(Component c) => Entrar(c, .24f, dy: -5f);                             // help-answer-in
        public static Anim PreguntaEntra(Component c) => Entrar(c, .28f, dx: 9f);                               // question-in
        public static Anim Feedback(Component c) => Entrar(c, .3f, escala: .95f);                               // feedback-pop
        public static Anim Confirmacion(Component c) => Entrar(c, .24f, dy: 8f, escala: .94f);                  // confirmation-in
        public static Anim Estrella(Component c) { var a = Add(c); a.tipo = Tipo.PopEstrella; a.dur = .45f; return a; } // star-pop
        public static Anim Destello(Component c) { var a = Add(c); a.tipo = Tipo.Desvanecer; a.dur = .25f; return a; } // flash
        // progress-grow: la barra crece desde la izquierda
        public static Anim Llenar(Component c, float dur = .8f) { var a = Add(c); a.tipo = Tipo.Llenar; a.dur = dur; return a; }
    }
}
