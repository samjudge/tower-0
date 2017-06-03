 using UnityEngine;
 using UnityEditor;
  
 public class ImportOverride : AssetPostprocessor
 {
     void OnPreprocessTexture()
     {
         TextureImporter importer = assetImporter as TextureImporter;
         Object asset = AssetDatabase.LoadAssetAtPath(importer.assetPath, typeof(Texture2D));
         if (!asset)
         {
             importer.textureType = TextureImporterType.Advanced;
             importer.npotScale = TextureImporterNPOTScale.None;
             importer.mipmapEnabled = false;
             importer.textureFormat = TextureImporterFormat.AutomaticTruecolor;
         }         
     }
 }