using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class SetPixelsPerUnit : MonoBehaviour
{
    //[MenuItem("Tools/Change Pixels Per Unit")]
    static void ChangeAllSpritesPPU()
    {
        string[] guids = AssetDatabase.FindAssets("t:Texture2D", new[] { "Assets/RafaelMatos" });
        int targetPPU = 32; // 원하는 PPU로 변경하세요

        foreach (string guid in guids)
        {
            string path = AssetDatabase.GUIDToAssetPath(guid);
            TextureImporter importer = AssetImporter.GetAtPath(path) as TextureImporter;

            if (importer != null && importer.textureType == TextureImporterType.Sprite)
            {
                importer.spritePixelsPerUnit = targetPPU;
                importer.SaveAndReimport();
                Debug.Log($"Updated PPU to {targetPPU} for: {path}");
            }
        }

        AssetDatabase.Refresh();
    }
}
