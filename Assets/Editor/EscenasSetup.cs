using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using ARDiabetes;

public static class EscenasSetup
{
    public static readonly string[] Nombres =
    {
        "01_Inicio", "02_Menu", "03_Libros", "04_Juegos", "05_Progreso", "06_Configuracion"
    };

    static readonly int[][] Vistas =
    {
        new[] { 0, 1, 2 },
        new[] { 3, 23 },
        new[] { 4, 5, 6, 7, 8, 9, 10, 11, 12, 13 },
        new[] { 17, 18, 19, 20 },
        new[] { 14, 21, 22 },
        new[] { 15, 16 }
    };

    public static string PathForPanel(int index) => "Assets/Scenes/" + Nombres[SceneForPanel(index)] + ".unity";

    public static int SceneForPanel(int index)
    {
        if (index <= 2) return 0;
        if (index == 3 || index == 23) return 1;
        if (index >= 4 && index <= 13) return 2;
        if (index >= 17 && index <= 20) return 3;
        if (index == 14 || index == 21 || index == 22) return 4;
        return 5;
    }

    [MenuItem("ARDiabetes/Crear escenas")]
    public static void Crear()
    {
        AssetDatabase.DeleteAsset("Assets/Imagenes/Generadas");
        guardados.Clear();
        DirectoryEnsure();
        var buildScenes = new List<EditorBuildSettingsScene>();
        for (int i = 0; i < Nombres.Length; i++)
        {
            var scene = EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);
            CrearCamara();
            new GameObject("EventSystem", typeof(EventSystem), typeof(StandaloneInputModule));
            var boot = ProjectSetup.CrearBase();
            boot.vistas = Vistas[i];
            AppBootstrap.PreviewVertical = true;
            boot.EditorPreview(Vistas[i][0]);
            AppBootstrap.PreviewVertical = false;
            GuardarGenerados();
            EditorSceneManager.MarkSceneDirty(scene);
            string path = "Assets/Scenes/" + Nombres[i] + ".unity";
            EditorSceneManager.SaveScene(scene, path);
            buildScenes.Add(new EditorBuildSettingsScene(path, true));
        }

        AssetDatabase.DeleteAsset(ProjectSetup.ScenePath);
        EditorBuildSettings.scenes = buildScenes.ToArray();
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
        Debug.Log("[EscenasSetup] Escenas generadas OK");
    }

    static void DirectoryEnsure()
    {
        System.IO.Directory.CreateDirectory("Assets/Scenes");
        System.IO.Directory.CreateDirectory("Assets/Imagenes/Generadas");
    }

    static void CrearCamara()
    {
        var camGo = new GameObject("Main Camera", typeof(Camera), typeof(AudioListener));
        camGo.tag = "MainCamera";
        var cam = camGo.GetComponent<Camera>();
        cam.clearFlags = CameraClearFlags.SolidColor;
        cam.backgroundColor = new Color(0.918f, 0.957f, 0.984f);
        cam.orthographic = true;
    }

    // Los sprites que UIKit crea en memoria (redondeados, círculo, degradados) no se pueden
    // guardar dentro de la escena: se pasan a PNG en Assets/Imagenes/Generadas y se reasignan
    static readonly Dictionary<Texture2D, Sprite> guardados = new Dictionary<Texture2D, Sprite>();

    static void GuardarGenerados()
    {
        var canvas = UnityEngine.Object.FindAnyObjectByType<Canvas>();
        if (canvas == null) return;
        foreach (var image in canvas.GetComponentsInChildren<Image>(true))
            if (image.sprite != null && !AssetDatabase.Contains(image.sprite))
                image.sprite = GuardarSprite(image.sprite);
        foreach (var raw in canvas.GetComponentsInChildren<RawImage>(true))
            if (raw.texture is Texture2D tex && !AssetDatabase.Contains(tex))
                raw.texture = GuardarSprite(Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), new Vector2(.5f, .5f))).texture;
    }

    static Sprite GuardarSprite(Sprite source)
    {
        var tex = source.texture;
        if (guardados.TryGetValue(tex, out var saved)) return saved;
        // nombre por contenido: misma figura → mismo archivo en cada regeneración
        var png = tex.EncodeToPNG();
        string path = "Assets/Imagenes/Generadas/ui_" + tex.width + "x" + tex.height + "_" + Hash128.Compute(png).ToString().Substring(0, 8) + ".png";
        if (!System.IO.File.Exists(path)) System.IO.File.WriteAllBytes(path, png);
        AssetDatabase.ImportAsset(path);
        var imp = (TextureImporter)AssetImporter.GetAtPath(path);
        imp.textureType = TextureImporterType.Sprite;
        imp.spriteImportMode = SpriteImportMode.Single;
        imp.spritePixelsPerUnit = source.pixelsPerUnit;
        imp.spriteBorder = source.border;
        imp.mipmapEnabled = false;
        imp.alphaIsTransparency = true;
        imp.wrapMode = TextureWrapMode.Clamp;
        imp.textureCompression = TextureImporterCompression.Uncompressed;
        var set = new TextureImporterSettings();
        imp.ReadTextureSettings(set);
        set.spriteMeshType = SpriteMeshType.FullRect;
        imp.SetTextureSettings(set);
        imp.SaveAndReimport();
        saved = AssetDatabase.LoadAssetAtPath<Sprite>(path);
        guardados[tex] = saved;
        return saved;
    }
}
