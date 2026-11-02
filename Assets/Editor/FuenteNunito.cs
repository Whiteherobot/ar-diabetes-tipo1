using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using TMPro;
using ARDiabetes;

/// <summary>
/// Crea los font assets SDF de Nunito (fuente del diseño de Figma) y los asigna al AppBootstrap.
///  - Genera un SDF dinámico por cada peso (Regular, SemiBold, Bold, ExtraBold).
///  - En el SDF Regular llena la tabla de pesos para que FontStyles.Bold use Nunito Bold de verdad.
///  - Asigna el SDF Regular al campo "font" del AppBootstrap de la escena abierta y la guarda.
/// </summary>
public static class FuenteNunito
{
    const string Carpeta = "Assets/Fuentes";
    public const string RutaRegular = Carpeta + "/Nunito-Regular SDF.asset";

    static readonly string[] Pesos = { "Regular", "SemiBold", "Bold", "ExtraBold" };

    [MenuItem("ARDiabetes/Crear fuente Nunito")]
    public static void Crear()
    {
        foreach (var peso in Pesos) CrearSdf(peso);
        AssetDatabase.SaveAssets();

        var regular = AssetDatabase.LoadAssetAtPath<TMP_FontAsset>(RutaRegular);
        if (regular == null) { Debug.LogError("[FuenteNunito] No se pudo crear " + RutaRegular); return; }

        // Tabla de pesos: índice = peso / 100 (600 SemiBold, 700 Bold, 800 ExtraBold)
        var tabla = regular.fontWeightTable;
        tabla[6].regularTypeface = Cargar("SemiBold");
        tabla[7].regularTypeface = Cargar("Bold");
        tabla[8].regularTypeface = Cargar("ExtraBold");
        EditorUtility.SetDirty(regular);
        AssetDatabase.SaveAssets();

        // Asignar al AppBootstrap de la escena abierta
        var boot = Object.FindAnyObjectByType<AppBootstrap>();
        if (boot != null)
        {
            boot.font = regular;
            EditorUtility.SetDirty(boot);
            EditorSceneManager.MarkSceneDirty(boot.gameObject.scene);
            EditorSceneManager.SaveScene(boot.gameObject.scene);
        }
        else Debug.LogWarning("[FuenteNunito] No hay AppBootstrap en la escena abierta; asígnala a mano.");

        Debug.Log("[FuenteNunito] Fuente Nunito lista.");
    }

    // Crea el SDF de un peso usando el mismo menú de TMP (si ya existe, no lo repite)
    static void CrearSdf(string peso)
    {
        if (Cargar(peso) != null) return;
        var ttf = AssetDatabase.LoadAssetAtPath<Font>(Carpeta + "/Nunito-" + peso + ".ttf");
        if (ttf == null) { Debug.LogError("[FuenteNunito] Falta Nunito-" + peso + ".ttf en " + Carpeta); return; }
        Selection.activeObject = ttf;
        EditorApplication.ExecuteMenuItem("Assets/Create/TextMeshPro/Font Asset/SDF");
    }

    static TMP_FontAsset Cargar(string peso)
    {
        return AssetDatabase.LoadAssetAtPath<TMP_FontAsset>(Carpeta + "/Nunito-" + peso + " SDF.asset");
    }
}
