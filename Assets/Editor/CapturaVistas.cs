using System.IO;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.UI;
using ARDiabetes;

/// <summary>
/// Captura PNG de las pantallas sin darle Play (para revisar el diseño desde CLI).
/// Uso: -executeMethod CapturaVistas.Capturar  (args opcionales: -vistas 3,4,5)
/// Las imágenes quedan en Capturas/ (fuera de Assets).
/// </summary>
public static class CapturaVistas
{
    const int Ancho = 1080, Alto = 1920;

    [MenuItem("ARDiabetes/Capturar vistas")]
    public static void Capturar()
    {
        AppBootstrap.PreviewVertical = true;

        int[] vistas = LeerVistas();
        Directory.CreateDirectory("Capturas");
        foreach (int v in vistas)
        {
            EditorSceneManager.OpenScene(EscenasSetup.PathForPanel(v));
            var boot = Object.FindAnyObjectByType<AppBootstrap>();
            if (boot == null) throw new System.InvalidOperationException("No hay AppBootstrap para la vista " + v);
            boot.EditorPreview(v);
            var canvas = Object.FindAnyObjectByType<Canvas>();
            MostrarOcultos(canvas.transform);
            var ruta = "Capturas/vista_" + v.ToString("00") + ".png";
            Renderizar(canvas, ruta);
            Debug.Log("[Captura] " + ruta);

            // Si la vista tiene scroll, también se captura la mitad y el final
            var scroll = canvas.GetComponentInChildren<ScrollRect>();
            if (scroll != null)
            {
                foreach (float pos in new[] { 0.5f, 0f })
                {
                    Canvas.ForceUpdateCanvases();
                    scroll.verticalNormalizedPosition = pos;
                    var r = "Capturas/vista_" + v.ToString("00") + (pos > 0 ? "_medio" : "_final") + ".png";
                    Renderizar(canvas, r);
                    Debug.Log("[Captura] " + r);
                }
            }
        }
        AppBootstrap.PreviewVertical = false;
    }

    // Pasa el Canvas a ScreenSpaceCamera y lo dibuja en una RenderTexture del tamaño de referencia
    static void Renderizar(Canvas canvas, string ruta)
    {
        var rt = new RenderTexture(Ancho, Alto, 24);
        var camGo = new GameObject("CamCaptura");
        var cam = camGo.AddComponent<Camera>();
        cam.clearFlags = CameraClearFlags.SolidColor;
        cam.backgroundColor = Color.white;
        cam.orthographic = true;
        cam.targetTexture = rt;

        canvas.renderMode = RenderMode.ScreenSpaceCamera;
        canvas.worldCamera = cam;
        canvas.planeDistance = 1;
        var scaler = canvas.GetComponent<CanvasScaler>();
        scaler.uiScaleMode = CanvasScaler.ScaleMode.ConstantPixelSize;
        scaler.scaleFactor = 1;
        Canvas.ForceUpdateCanvases();
        cam.Render();
        Canvas.ForceUpdateCanvases();
        cam.Render();

        RenderTexture.active = rt;
        var tex = new Texture2D(Ancho, Alto, TextureFormat.RGB24, false);
        tex.ReadPixels(new Rect(0, 0, Ancho, Alto), 0, 0);
        tex.Apply();
        RenderTexture.active = null;
        File.WriteAllBytes(ruta, tex.EncodeToPNG());

        Object.DestroyImmediate(tex);
        Object.DestroyImmediate(camGo);
        rt.Release();
    }

    // -mostrar Nombre1,Nombre2: activa objetos ocultos (modales, retroalimentación) para revisarlos
    static void MostrarOcultos(Transform raiz)
    {
        var args = System.Environment.GetCommandLineArgs();
        for (int i = 0; i < args.Length - 1; i++)
        {
            if (args[i] != "-mostrar") continue;
            var nombres = new System.Collections.Generic.HashSet<string>(args[i + 1].Split(','));
            foreach (var t in raiz.GetComponentsInChildren<Transform>(true))
                if (nombres.Contains(t.name) && t.parent.gameObject.activeInHierarchy) t.gameObject.SetActive(true);
        }
    }

    static int[] LeerVistas()
    {
        var args = System.Environment.GetCommandLineArgs();
        for (int i = 0; i < args.Length - 1; i++)
            if (args[i] == "-vistas")
                return System.Array.ConvertAll(args[i + 1].Split(','), int.Parse);
        return new[] { 3 };
    }
}
