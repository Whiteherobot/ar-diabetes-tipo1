using UnityEditor;
using UnityEngine;

/// <summary>
/// Modelos animados del cliente (Imagenes/QR/Nutri_*.fbx): traen un clip cada uno. Se importan
/// como animación Legacy en bucle, así basta el componente Animation que Unity agrega al prefab
/// (sin Animator Controller), tanto en el AR como en el visor de respaldo.
/// Sus texturas embebidas Unity no las extrae: las paletas están en Imagenes/QR/Texturas/ y se
/// aplican al instanciar (ver AppBootstrap.nutriPaletas y ModelViewer.AplicarMateriales).
/// </summary>
public class ImportarModelosAnimados : AssetPostprocessor
{
    static bool EsAnimado(string path) =>
        path.StartsWith("Assets/Imagenes/QR/Nutri_") && path.EndsWith(".fbx");

    void OnPreprocessModel()
    {
        if (!EsAnimado(assetPath)) return;
        var mi = (ModelImporter)assetImporter;
        mi.importAnimation = true;
        mi.animationType = ModelImporterAnimationType.Legacy;
        mi.importCameras = false;
        mi.importLights = false;
    }

    // Paletas: bloques de color planos; sin compresión ni mipmaps para que no se mezclen.
    void OnPreprocessTexture()
    {
        if (!assetPath.StartsWith("Assets/Imagenes/QR/Texturas/")) return;
        var ti = (TextureImporter)assetImporter;
        ti.textureType = TextureImporterType.Default;
        ti.mipmapEnabled = false;
        ti.textureCompression = TextureImporterCompression.Uncompressed;
        ti.wrapMode = TextureWrapMode.Clamp;
    }

    void OnPreprocessAnimation()
    {
        if (!EsAnimado(assetPath)) return;
        var mi = (ModelImporter)assetImporter;
        var clips = mi.defaultClipAnimations;
        foreach (var c in clips)
        {
            c.loopTime = true;
            c.wrapMode = WrapMode.Loop;
        }
        mi.clipAnimations = clips;
    }
}
