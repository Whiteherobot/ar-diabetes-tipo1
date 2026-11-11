using System.IO;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Rendering;
using UnityEngine.XR.Management;
using UnityEditor.XR.Management;
using UnityEditor.XR.Management.Metadata;
using ARDiabetes;

/// <summary>
/// Setup reproducible del proyecto (se ejecuta por CLI vía -executeMethod ProjectSetup.Build):
///  - Marca el arte de Assets/Imagenes como Sprite.
///  - Crea las escenas de Assets/Scenes con cámara, EventSystem y AppBootstrap cableados.
///  - Registra la escena en Build Settings y fija PlayerSettings para Android (portrait, bundle id).
/// </summary>
public static class ProjectSetup
{
    public const string ScenePath = "Assets/Scenes/Main.unity";

    [MenuItem("ARDiabetes/Rebuild Scene")]
    public static void Build()
    {
        EscenasSetup.Crear();
    }

    public static AppBootstrap CrearBase()
    {
        MarcarSpritesEnCarpeta("Assets/Imagenes");

        var scene = EditorSceneManager.GetActiveScene();

        // Cámara
        var camGo = new GameObject("Main Camera", typeof(Camera), typeof(AudioListener));
        camGo.tag = "MainCamera";
        var cam = camGo.GetComponent<Camera>();
        cam.clearFlags = CameraClearFlags.SolidColor;
        cam.backgroundColor = new Color(0.918f, 0.957f, 0.984f); // #EAF4FB
        cam.orthographic = true;

        // EventSystem
        new GameObject("EventSystem", typeof(EventSystem), typeof(StandaloneInputModule));

        // AppBootstrap + sprites
        var go = new GameObject("AppBootstrap");
        var boot = go.AddComponent<AppBootstrap>();
        boot.spLogo        = L("Assets/Imagenes/Diseño/personajes.png");
        boot.spWelcome     = L("Assets/Imagenes/Diseño/Avatar2.png");
        boot.spIconFisio   = L("Assets/Imagenes/Diseño/Botones/Icon_Fisiológico.png");
        boot.spIconNutri   = L("Assets/Imagenes/Diseño/Botones/Icon_nutricional.png");
        boot.spIconClinico = L("Assets/Imagenes/Diseño/Botones/Icon_Clínico.png");
        boot.spIconScan    = L("Assets/Imagenes/Diseño/Botones/btn_scaner.png");
        boot.spIconJuegos  = L("Assets/Imagenes/Diseño/Botones/Icon_juegos 1.png");
        boot.spIconProgreso= L("Assets/Imagenes/UI/ic_chart.png"); // barras (reemplaza la llamita)
        boot.spStar        = L("Assets/Imagenes/Diseño/Botones/Icon_Estrella.png");
        // Robots ilustrados del cliente (Recursos/): solo hay 3 personajes, uno por mundo.
        const string Rec = "Assets/Imagenes/Diseño/Recursos/";
        boot.robotCoral    = L(Rec + "Robot2_Coral.png");
        boot.robotVerde    = L(Rec + "Robot2_Verde.png");
        boot.robotAzul     = L(Rec + "Robot2_Azul.png");
        boot.spPancreasPersonaje = L("Assets/Imagenes/Diseño/Personajes/Pancreas_personaje.png");
        // Los usos decorativos de los colores que ya no existen toman uno de los 3 nuevos.
        boot.robotMorado   = boot.robotAzul;
        boot.robotNaranja  = boot.robotVerde;
        boot.robotRosa     = boot.robotCoral;
        boot.bustos        = new[] { L(Rec + "Busto_Verde.png"), L(Rec + "Busto_Azul.png"), L(Rec + "Busto_Coral.png") };
        boot.recFondoInicio = L(Rec + "Fondo_Inicio.png");
        boot.recBienvenida = new[] { L(Rec + "Fondo_Bienvenida1.png"), L(Rec + "Fondo_Bienvenida2.png"), L(Rec + "Fondo_Bienvenida3.png") };
        boot.recComenzar   = L(Rec + "Btn_Comenzar.png");
        boot.recSiguiente  = L(Rec + "Btn_Siguiente.png");
        boot.recOmitir     = L(Rec + "Btn_Omitir.png");
        boot.recExplorar   = L(Rec + "Tarjeta_Explorar.png");
        boot.recExploraAhora = L(Rec + "Btn_ExploraAhora.png");
        boot.recMundos     = new[] { L(Rec + "Tarjeta_Fisiologico.png"), L(Rec + "Tarjeta_Nutricional.png"), L(Rec + "Tarjeta_Clinico.png") };
        boot.recIntroMundos = L(Rec + "Tarjeta_Mundos.png");
        boot.recEscanear   = L(Rec + "Tarjeta_Escanear.png");
        boot.recJuegos     = L(Rec + "Tarjeta_Juegos.png");
        boot.recFlechas    = new[] { L(Rec + "Flecha_Coral.png"), L(Rec + "Flecha_Verde.png"), L(Rec + "Flecha_Azul.png"), L(Rec + "Flecha_Naranja.png"), L(Rec + "Flecha_Rosa.png") };
        boot.figCamera     = L("Assets/Imagenes/UI/Figma/ic_fig_camera.png");
        boot.figCheck      = L("Assets/Imagenes/UI/Figma/ic_fig_check.png");
        boot.figX          = L("Assets/Imagenes/UI/Figma/ic_fig_x.png");
        boot.figKit        = L("Assets/Imagenes/UI/Figma/ic_fig_kit.png");
        boot.figTemas = System.Array.ConvertAll(new[] { "help", "pancreas", "drop", "gear", "plate", "food", "apple", "clock", "syringe", "warning", "drop", "calendar" }, n => L("Assets/Imagenes/UI/Figma/ic_fig_" + n + ".png"));
        boot.figScan       = L("Assets/Imagenes/UI/Figma/ic_fig_scan.png");
        boot.figGame       = L("Assets/Imagenes/UI/Figma/ic_fig_game.png");
        boot.figChevron    = L("Assets/Imagenes/UI/Figma/ic_fig_chevron.png");
        boot.figStar       = L("Assets/Imagenes/UI/Figma/ic_fig_star.png");
        boot.figBack       = L("Assets/Imagenes/UI/Figma/ic_fig_back.png");
        boot.figBook       = L("Assets/Imagenes/UI/Figma/ic_fig_book.png");
        boot.figPlay       = L("Assets/Imagenes/UI/Figma/ic_fig_play.png");
        boot.figInfo       = L("Assets/Imagenes/UI/Figma/ic_fig_info.png");
        boot.figWarning    = L("Assets/Imagenes/UI/Figma/ic_fig_warning.png");
        boot.figSpeaker    = L("Assets/Imagenes/UI/Figma/ic_fig_speaker.png");
        boot.figPause      = L("Assets/Imagenes/UI/Figma/ic_fig_pause.png");
        boot.figStop       = L("Assets/Imagenes/UI/Figma/ic_fig_stop.png");
        boot.figHome       = L("Assets/Imagenes/UI/Figma/ic_fig_home.png");
        boot.figChart      = L("Assets/Imagenes/UI/Figma/ic_fig_chart.png");
        boot.figSettings   = L("Assets/Imagenes/UI/Figma/ic_fig_settings.png");
        boot.figHelp       = L("Assets/Imagenes/UI/Figma/ic_fig_help.png");
        boot.spPancreas    = L("Assets/Imagenes/Diseño/Icon_Pancreas.png");
        boot.pancreasModel = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Imagenes/QR/Pancreas.fbx");
        // Libro Nutricional: modelos animados del cliente, uno por tema (el 4.º tema usa el plato).
        boot.platoModel    = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Imagenes/QR/Nutri_Plato.fbx");
        boot.nutriModelos  = new[]
        {
            boot.platoModel,
            AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Imagenes/QR/Nutri_Carbohidratos.fbx"),
            AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Imagenes/QR/Nutri_Equipo.fbx"),
            boot.platoModel,
        };
        foreach (var m in boot.nutriModelos) if (m == null) Debug.LogWarning("[ProjectSetup] Falta un modelo Nutri_*.fbx");
        // Paletas de color de esos modelos (Unity no extrae las texturas embebidas del FBX).
        Texture2D Paleta(string n) => AssetDatabase.LoadAssetAtPath<Texture2D>("Assets/Imagenes/QR/Texturas/" + n + ".png");
        var paletaPlato = Paleta("El_Plato_Colores");
        boot.nutriPaletas = new[] { paletaPlato, Paleta("Carbohidratos_Colores"), Paleta("Equipo_Colores"), paletaPlato };
        foreach (var t in boot.nutriPaletas) if (t == null) Debug.LogWarning("[ProjectSetup] Falta una paleta en Imagenes/QR/Texturas");
        boot.glucoModel    = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Imagenes/QR/Gluco.fbx");
        if (boot.pancreasModel == null) Debug.LogWarning("[ProjectSetup] Pancreas.fbx no encontrado");
        if (boot.platoModel == null) Debug.LogWarning("[ProjectSetup] Plato.fbx no encontrado");
        if (boot.glucoModel == null) Debug.LogWarning("[ProjectSetup] Gluco.fbx no encontrado");

        boot.icInfo     = L("Assets/Imagenes/UI/ic_info.png");
        boot.icAudio    = L("Assets/Imagenes/UI/ic_audio.png");
        boot.icRotate   = L("Assets/Imagenes/UI/ic_rotate.png");
        boot.icClose    = L("Assets/Imagenes/UI/ic_close.png");
        boot.icCamera   = L("Assets/Imagenes/UI/ic_camera.png");
        boot.icChart    = L("Assets/Imagenes/UI/ic_chart.png");
        boot.icQuestion = L("Assets/Imagenes/UI/ic_question.png");
        boot.icDrop     = L("Assets/Imagenes/UI/ic_drop.png");
        boot.icGear     = L("Assets/Imagenes/UI/ic_gear.png");
        boot.icHome     = L("Assets/Imagenes/UI/ic_home.png");
        boot.icProfile  = L("Assets/Imagenes/UI/ic_profile.png");
        boot.icBook     = L("Assets/Imagenes/UI/ic_book.png");
        boot.icPancreas = L("Assets/Imagenes/UI/ic_pancreas.png");
        boot.icPlate    = L("Assets/Imagenes/UI/ic_plate.png");
        boot.icBread    = L("Assets/Imagenes/UI/ic_bread.png");
        boot.icApple    = L("Assets/Imagenes/UI/ic_apple.png");
        boot.icClock    = L("Assets/Imagenes/UI/ic_clock.png");
        boot.icSyringe  = L("Assets/Imagenes/UI/ic_syringe.png");
        boot.icAlert    = L("Assets/Imagenes/UI/ic_alert.png");
        boot.icCalendar = L("Assets/Imagenes/UI/ic_calendar.png");

        boot.vocesQuiz = System.Array.ConvertAll(AssetDatabase.FindAssets("t:AudioClip", new[] { "Assets/Audio/Quiz" }),
            g => AssetDatabase.LoadAssetAtPath<AudioClip>(AssetDatabase.GUIDToAssetPath(g)));
        boot.ayudaClips = new AudioClip[6];
        for (int i = 0; i < 6; i++)
            boot.ayudaClips[i] = AssetDatabase.LoadAssetAtPath<AudioClip>($"Assets/Audio/Ayuda/ayuda_{i}.wav");
        boot.narracion = new AudioClip[12];
        for (int i = 0; i < 12; i++)
        {
            boot.narracion[i] = AssetDatabase.LoadAssetAtPath<AudioClip>($"Assets/Audio/Narracion/narracion_{i}.wav");
            if (boot.narracion[i] == null) Debug.LogWarning($"[ProjectSetup] narracion_{i}.wav no encontrado");
        }

        boot.musicaFondo = AssetDatabase.LoadAssetAtPath<AudioClip>("Assets/Audio/Musica/musica_fondo.wav");
        if (boot.musicaFondo == null) Debug.LogWarning("[ProjectSetup] musica_fondo.wav no encontrado");

        // Fuente Nunito (se crea con ARDiabetes/Crear fuente Nunito)
        boot.font = AssetDatabase.LoadAssetAtPath<TMPro.TMP_FontAsset>(FuenteNunito.RutaRegular);
        if (boot.font == null) Debug.LogWarning("[ProjectSetup] Nunito SDF no encontrada; usa ARDiabetes/Crear fuente Nunito");

        boot.markersFisio = LoadMarkers("qr_diabetes", "qr_pancreas", "qr_insulina", "qr_funciona");
        boot.markersNutri = LoadMarkers("qr_nutri_plato", "qr_nutri_carbohidratos", "qr_nutri_recomendados", "qr_nutri_habitos");
        boot.markersClinico = LoadMarkers("qr_clinico_insulina", "qr_clinico_sintomas", "qr_clinico_monitoreo", "qr_clinico_cuidados");

        PlayerSettings.companyName = "Endify";
        PlayerSettings.productName = "AR Diabetes Tipo 1";
        PlayerSettings.defaultInterfaceOrientation = UIOrientation.AutoRotation;
        PlayerSettings.allowedAutorotateToPortrait = true;
        PlayerSettings.allowedAutorotateToPortraitUpsideDown = false;
        PlayerSettings.allowedAutorotateToLandscapeLeft = true;
        PlayerSettings.allowedAutorotateToLandscapeRight = true;
        PlayerSettings.useAnimatedAutorotation = true;
        try
        {
            PlayerSettings.SetApplicationIdentifier(
                UnityEditor.Build.NamedBuildTarget.Android, "com.endify.ardiabetes");
            PlayerSettings.Android.minSdkVersion = AndroidSdkVersions.AndroidApiLevel26;
        }
        catch (System.Exception e) { Debug.LogWarning("Android settings: " + e.Message); }

        AssetDatabase.SaveAssets();
        return boot;
    }

    // Build de validación (Linux) para probar las pantallas en la PC.
    public static void BuildLinux()
    {
        var opts = new BuildPlayerOptions
        {
            scenes = EditorBuildSettingsScenePaths(),
            locationPathName = "/tmp/ardiabetes_linux/ARDiabetes.x86_64",
            target = BuildTarget.StandaloneLinux64,
            options = BuildOptions.None
        };
        var report = BuildPipeline.BuildPlayer(opts);
        Debug.Log("[ProjectSetup] Build Linux: " + report.summary.result +
                  " size=" + report.summary.totalSize);
        if (report.summary.result != UnityEditor.Build.Reporting.BuildResult.Succeeded)
            EditorApplication.Exit(1);
    }

    // Build del APK Android.
    public static void BuildAndroid()
    {
        EditorUserBuildSettings.SwitchActiveBuildTarget(BuildTargetGroup.Android, BuildTarget.Android);
        System.IO.Directory.CreateDirectory("/tmp/ardiabetes_apk");
        var opts = new BuildPlayerOptions
        {
            scenes = EditorBuildSettingsScenePaths(),
            locationPathName = "/tmp/ardiabetes_apk/ARDiabetes.apk",
            target = BuildTarget.Android,
            options = BuildOptions.None
        };
        var report = BuildPipeline.BuildPlayer(opts);
        Debug.Log("[ProjectSetup] Build Android: " + report.summary.result +
                  " size=" + report.summary.totalSize);
        if (report.summary.result != UnityEditor.Build.Reporting.BuildResult.Succeeded)
            EditorApplication.Exit(1);
    }

    static string[] EditorBuildSettingsScenePaths()
    {
        var enabled = new System.Collections.Generic.List<string>();
        foreach (var scene in EditorBuildSettings.scenes)
            if (scene.enabled) enabled.Add(scene.path);
        return enabled.ToArray();
    }

    // ---- Preview de las vistas en modo edición ----
    static void Prev(int i)
    {
        EditorSceneManager.OpenScene(EscenasSetup.PathForPanel(i));
        var b = UnityEngine.Object.FindFirstObjectByType<AppBootstrap>();
        if (b == null) { Debug.LogError("[Preview] No hay AppBootstrap en la escena."); return; }
        AppBootstrap.PreviewVertical = true;
        b.EditorPreview(i);
        Debug.Log("[Preview] Mostrando vista " + i + " — mira la pestaña Game.");
    }
    [MenuItem("ARDiabetes/Preview/1 Inicio")] static void Pv0() => Prev(0);
    [MenuItem("ARDiabetes/Preview/2 Bienvenida")] static void Pv1() => Prev(1);
    [MenuItem("ARDiabetes/Preview/3 Perfil")] static void Pv2() => Prev(2);
    [MenuItem("ARDiabetes/Preview/4 Menú")] static void Pv3() => Prev(3);
    [MenuItem("ARDiabetes/Preview/5A Fisiológico/Temas")] static void Pv4() => Prev(4);
    [MenuItem("ARDiabetes/Preview/5A Fisiológico/Detalle")] static void Pv5() => Prev(5);
    [MenuItem("ARDiabetes/Preview/5A Fisiológico/Experiencia AR")] static void Pv6() => Prev(6);
    [MenuItem("ARDiabetes/Preview/5B Nutricional/Temas")] static void Pv7() => Prev(7);
    [MenuItem("ARDiabetes/Preview/5B Nutricional/Detalle")] static void Pv8() => Prev(8);
    [MenuItem("ARDiabetes/Preview/5B Nutricional/Experiencia AR")] static void Pv9() => Prev(9);
    [MenuItem("ARDiabetes/Preview/5C Clínico/Temas")] static void Pv10() => Prev(10);
    [MenuItem("ARDiabetes/Preview/5C Clínico/Detalle")] static void Pv11() => Prev(11);
    [MenuItem("ARDiabetes/Preview/5C Clínico/Experiencia AR")] static void Pv12() => Prev(12);
    [MenuItem("ARDiabetes/Preview/5D Escaneo genérico")] static void Pv13() => Prev(13);
    [MenuItem("ARDiabetes/Preview/7 Progreso")] static void Pv14() => Prev(14);
    [MenuItem("ARDiabetes/Preview/8 Configuración")] static void Pv15() => Prev(15);
    [MenuItem("ARDiabetes/Preview/9 Ayuda")] static void Pv16() => Prev(16);
    [MenuItem("ARDiabetes/Preview/Juegos")] static void Pv17() => Prev(17);
    [MenuItem("ARDiabetes/Preview/Quiz Fisiológico")] static void Pv18() => Prev(18);
    [MenuItem("ARDiabetes/Preview/Quiz Nutricional")] static void Pv19() => Prev(19);
    [MenuItem("ARDiabetes/Preview/Quiz Clínico")] static void Pv20() => Prev(20);
    [MenuItem("ARDiabetes/Preview/Insignias")] static void Pv21() => Prev(21);
    [MenuItem("ARDiabetes/Preview/Colección")] static void Pv22() => Prev(22);
    [MenuItem("ARDiabetes/Preview/Mundos")] static void Pv23() => Prev(23);
    [MenuItem("ARDiabetes/Preview/Limpiar preview")]
    static void PvClear()
    {
        var b = UnityEngine.Object.FindFirstObjectByType<AppBootstrap>();
        if (b != null) b.EditorClearPreview();
    }

    // Configura el proyecto para ARCore: input handling, OpenGLES3, loader XR, QR legibles.
    public static void SetupAR()
    {
        // 1) Active Input Handling = Both (TrackedPoseDriver del Input System + UI legacy)
        try
        {
            var objs = AssetDatabase.LoadAllAssetsAtPath("ProjectSettings/ProjectSettings.asset");
            if (objs.Length > 0)
            {
                var so = new SerializedObject(objs[0]);
                var prop = so.FindProperty("activeInputHandler");
                if (prop != null && prop.intValue != 2) { prop.intValue = 2; so.ApplyModifiedProperties(); Debug.Log("[SetupAR] activeInputHandler=Both"); }
            }
        }
        catch (System.Exception e) { Debug.LogWarning("[SetupAR] input handler: " + e.Message); }

        // 2) Graphics API: solo OpenGLES3 (ARCore no soporta Vulkan)
        PlayerSettings.SetUseDefaultGraphicsAPIs(BuildTarget.Android, false);
        PlayerSettings.SetGraphicsAPIs(BuildTarget.Android, new[] { GraphicsDeviceType.OpenGLES3 });

        // 3) Habilitar loader ARCore
        try
        {
            Directory.CreateDirectory("Assets/XR");
            XRGeneralSettingsPerBuildTarget perBT;
            if (!EditorBuildSettings.TryGetConfigObject(XRGeneralSettings.k_SettingsKey, out perBT) || perBT == null)
            {
                perBT = ScriptableObject.CreateInstance<XRGeneralSettingsPerBuildTarget>();
                AssetDatabase.CreateAsset(perBT, "Assets/XR/XRGeneralSettingsPerBuildTarget.asset");
                EditorBuildSettings.AddConfigObject(XRGeneralSettings.k_SettingsKey, perBT, true);
            }
            var settings = perBT.SettingsForBuildTarget(BuildTargetGroup.Android);
            if (settings == null)
            {
                settings = ScriptableObject.CreateInstance<XRGeneralSettings>();
                settings.name = "Android";
                perBT.SetSettingsForBuildTarget(BuildTargetGroup.Android, settings);
                AssetDatabase.AddObjectToAsset(settings, perBT);
            }
            var mgr = settings.AssignedSettings;
            if (mgr == null)
            {
                mgr = ScriptableObject.CreateInstance<XRManagerSettings>();
                mgr.name = "XRManagerSettings";
                settings.AssignedSettings = mgr;
                AssetDatabase.AddObjectToAsset(mgr, perBT);
            }
            bool ok = XRPackageMetadataStore.AssignLoader(mgr, "UnityEngine.XR.ARCore.ARCoreLoader", BuildTargetGroup.Android);
            Debug.Log("[SetupAR] ARCore loader assigned=" + ok);
            EditorUtility.SetDirty(perBT);
        }
        catch (System.Exception e) { Debug.LogWarning("[SetupAR] XR loader: " + e.Message); }

        // 4) QR marcadores legibles (para librería de imágenes en runtime)
        foreach (var guid in AssetDatabase.FindAssets("t:Texture2D", new[] { "Assets/Markers" }))
        {
            var path = AssetDatabase.GUIDToAssetPath(guid);
            var ti = AssetImporter.GetAtPath(path) as TextureImporter;
            if (ti == null) continue;
            ti.textureType = TextureImporterType.Default;
            ti.isReadable = true;
            ti.mipmapEnabled = false;
            ti.textureCompression = TextureImporterCompression.Uncompressed;
            ti.SaveAndReimport();
        }

        AssetDatabase.SaveAssets();
        Debug.Log("[SetupAR] listo");
    }

    public static void PrintARVersions()
    {
        var req = UnityEditor.PackageManager.Client.Search("com.unity.xr.arfoundation");
        while (!req.IsCompleted) System.Threading.Thread.Sleep(100);
        if (req.Status == UnityEditor.PackageManager.StatusCode.Success && req.Result.Length > 0)
        {
            var p = req.Result[0];
            Debug.Log("[ARF] latestCompatible=" + p.versions.latestCompatible);
            Debug.Log("[ARF] compatible=" + string.Join(",", p.versions.compatible));
        }
        else Debug.Log("[ARF] search failed: " + (req.Error != null ? req.Error.message : "?"));
    }

    static Texture2D[] LoadMarkers(params string[] names)
    {
        var arr = new Texture2D[names.Length];
        for (int i = 0; i < names.Length; i++)
        {
            arr[i] = AssetDatabase.LoadAssetAtPath<Texture2D>($"Assets/Markers/{names[i]}.png");
            if (arr[i] == null) Debug.LogWarning($"[ProjectSetup] marcador no encontrado: {names[i]}.png");
        }
        return arr;
    }

    static Sprite L(string path)
    {
        var s = AssetDatabase.LoadAssetAtPath<Sprite>(path);
        if (s == null) Debug.LogWarning("[ProjectSetup] Sprite no encontrado: " + path);
        return s;
    }

    static void MarcarSpritesEnCarpeta(string folder)
    {
        foreach (var guid in AssetDatabase.FindAssets("t:Texture2D", new[] { folder }))
        {
            var path = AssetDatabase.GUIDToAssetPath(guid);
            if (path.Contains("/QR/Texturas/")) continue; // paletas de modelos 3D, no sprites
            var ti = AssetImporter.GetAtPath(path) as TextureImporter;
            if (ti == null) continue;
            // Las ilustraciones del cliente (Recursos/) miden más de 2048 de alto: sin esto se verían borrosas.
            int maximo = path.Contains("/Recursos/") ? 4096 : ti.maxTextureSize;
            if (ti.textureType != TextureImporterType.Sprite || ti.spriteImportMode != SpriteImportMode.Single || ti.maxTextureSize != maximo)
            {
                ti.textureType = TextureImporterType.Sprite;
                ti.spriteImportMode = SpriteImportMode.Single;
                ti.maxTextureSize = maximo;
                ti.SaveAndReimport();
            }
        }
    }
}
