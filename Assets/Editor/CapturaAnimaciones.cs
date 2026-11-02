using System.Collections.Generic;
using System.IO;
using System.Reflection;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using ARDiabetes;

/// <summary>
/// Corre la app en Play y guarda fotogramas de cada pantalla durante sus animaciones.
/// Uso: -executeMethod CapturaAnimaciones.Iniciar -vistas 0,3,14 (sin -quit; se cierra sola).
/// Las imágenes quedan en Capturas/anim/vista_NN_fFF.png (10 fotogramas por segundo, 2.5 s por vista).
/// </summary>
[InitializeOnLoad]
public static class CapturaAnimaciones
{
    const string Clave = "CapturaAnimaciones.vistas";
    const int Ancho = 540, Alto = 960, Fotos = 25;
    const float Paso = 0.1f;

    static Queue<int> pendientes;
    static int vista = -1, foto;
    static double siguiente;
    static Camera cam;
    static RenderTexture rt;

    static CapturaAnimaciones()
    {
        // Tras recargar el dominio al entrar a Play, se retoma desde aquí
        if (!SessionState.GetBool(Clave + ".activo", false)) return;
        AppBootstrap.PreviewVertical = true;
        EditorApplication.update += Tick;
    }

    public static void Iniciar()
    {
        string vistas = "0,3";
        var args = System.Environment.GetCommandLineArgs();
        for (int i = 0; i < args.Length - 1; i++) if (args[i] == "-vistas") vistas = args[i + 1];
        int first = int.Parse(vistas.Split(',')[0]);
        EditorSceneManager.OpenScene(EscenasSetup.PathForPanel(first));
        SessionState.SetString(Clave, vistas);
        SessionState.SetBool(Clave + ".activo", true);
        AppBootstrap.PreviewVertical = true;
        Directory.CreateDirectory("Capturas/anim");
        EditorApplication.update += Tick;
        EditorApplication.EnterPlaymode();
    }

    static void Tick()
    {
        if (!EditorApplication.isPlaying) return;
        var boot = Object.FindAnyObjectByType<AppBootstrap>();
        if (boot == null || Time.timeSinceLevelLoad < 1.5f) return; // deja que Start arme la UI

        if (pendientes == null)
        {
            pendientes = new Queue<int>();
            foreach (var s in SessionState.GetString(Clave, "0").Split(',')) pendientes.Enqueue(int.Parse(s));
        }

        if (vista < 0 || foto >= Fotos)
        {
            if (pendientes.Count == 0) { Terminar(); return; }
            vista = pendientes.Dequeue(); foto = 0;
            SceneManager.LoadScene(EscenasSetup.PathForPanel(vista));
            return;
        }

        if (Time.timeSinceLevelLoad < 1.5f) return;
        if (foto == 0)
        {
            Mostrar(boot, vista);
            siguiente = EditorApplication.timeSinceStartup;
        }

        if (EditorApplication.timeSinceStartup < siguiente) return;
        Guardar("Capturas/anim/vista_" + vista.ToString("00") + "_f" + foto.ToString("00") + ".png");
        if (foto == 4) Interactuar(boot, vista);
        foto++;
        siguiente += Paso;
    }

    // Simula la interacción que dispara las animaciones de respuesta:
    // quiz → respuesta correcta (feedback-pop + star-pop); config → abrir confirmación de reinicio
    static void Interactuar(AppBootstrap boot, int i)
    {
        var f = BindingFlags.NonPublic | BindingFlags.Instance;
        var panel = ((RectTransform[])typeof(AppBootstrap).GetField("panels", f).GetValue(boot))[i];
        var quiz = panel.GetComponent("QuizRefs");
        if (quiz != null) typeof(AppBootstrap).GetMethod("ShowFeedback", f).Invoke(boot, new object[] { quiz, true });
        var modal = panel.Find("ResetModal");
        if (modal != null) modal.gameObject.SetActive(true);
    }

    // Llama a ShowOnly(panels[i], true) como lo haría la navegación real
    static void Mostrar(AppBootstrap boot, int i)
    {
        var f = BindingFlags.NonPublic | BindingFlags.Instance;
        var panels = (RectTransform[])typeof(AppBootstrap).GetField("panels", f).GetValue(boot);
        var show = typeof(AppBootstrap).GetMethod("ShowOnly", f);
        show.Invoke(boot, new object[] { panels[Mathf.Clamp(i, 0, panels.Length - 1)], true });
    }

    static void Guardar(string ruta)
    {
        var canvas = Object.FindAnyObjectByType<Canvas>();
        if (cam == null)
        {
            rt = new RenderTexture(Ancho, Alto, 24);
            cam = new GameObject("CamCapturaAnim").AddComponent<Camera>();
            cam.clearFlags = CameraClearFlags.SolidColor;
            cam.backgroundColor = Color.white;
            cam.orthographic = true;
            cam.targetTexture = rt;
        }
        if (canvas.renderMode != RenderMode.ScreenSpaceCamera || canvas.worldCamera != cam)
        {
            canvas.renderMode = RenderMode.ScreenSpaceCamera;
            canvas.worldCamera = cam;
            canvas.planeDistance = 1;
            var scaler = canvas.GetComponent<CanvasScaler>();
            scaler.uiScaleMode = CanvasScaler.ScaleMode.ConstantPixelSize;
            scaler.scaleFactor = 0.5f; // 1080x1920 de referencia → 540x960
        }
        Canvas.ForceUpdateCanvases();
        cam.Render();
        RenderTexture.active = rt;
        var tex = new Texture2D(Ancho, Alto, TextureFormat.RGB24, false);
        tex.ReadPixels(new Rect(0, 0, Ancho, Alto), 0, 0);
        tex.Apply();
        RenderTexture.active = null;
        File.WriteAllBytes(ruta, tex.EncodeToPNG());
        Object.DestroyImmediate(tex);
    }

    static void Terminar()
    {
        EditorApplication.update -= Tick;
        SessionState.SetBool(Clave + ".activo", false);
        Debug.Log("[CapturaAnim] listo");
        EditorApplication.Exit(0);
    }
}
