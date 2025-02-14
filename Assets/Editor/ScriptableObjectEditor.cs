using UnityEngine;
using System.IO;
using UnityEditor;

public class ScriptableObjectEditor : EditorWindow
{
    [MenuItem("Tools/Load Excel Data")]
    public static void LoadExcelData()
    {
        string path = EditorUtility.OpenFilePanel("Select CSV File", "", "csv");


        //path를 불러와서 있는지 없는지 체크하고 없으면 리턴 합니다.
        if (string.IsNullOrEmpty(path)) return;


        string[] lines = File.ReadAllLines(path);


        foreach(string line in lines)
        {
            string[] values = line.Split(',');

            if (values.Length < 3) continue; // ID, Type은 반드시 받아와야 함으로 모든 타입은 3개 이상을 가지고있습니다.

            string type = values[0].Trim();
            string savepath = null;

            BaseScriptableObject newBso = null;

            switch(type)
            {
                case "main_rune_data":
                    {
                        newBso = ScriptableObject.CreateInstance<LuneScriptableObejct>();
                        savepath = "rune_data";
                    }
                    break;
                case "small_rune_data":
                    {
                        newBso = ScriptableObject.CreateInstance<LuneScriptableObejct>();
                        savepath = "rune_data";
                    }
                    break;
                case "lantern_data":
                    {
                        newBso = ScriptableObject.CreateInstance<LanternPerkScriptableObejct>();
                        savepath = "lantern_data";
                    }
                    break;
                case "magic_data":
                    {
                        newBso = ScriptableObject.CreateInstance<MagicScriptableObejct>();
                        savepath = "magic_data";
                    }
                    break;
                case "slate_data":
                    {
                        newBso = ScriptableObject.CreateInstance<SlateScriptableObejct>();
                        savepath = "slate_data";
                    }
                    break;
                case "block_data":
                    {
                        newBso = ScriptableObject.CreateInstance<BlockScriptableObject>();
                        savepath = "block_data";
                    }
                    break;
                case "monster_data":
                    {
                        newBso = ScriptableObject.CreateInstance<MonsterScriptableObject>();
                        savepath = "monster_data";
                    }
                    break;
                case "medal_data":
                    {
                        newBso = ScriptableObject.CreateInstance<MedalScriptableObejct>();
                        savepath = "medal_data";
                    }
                    break;
            }
            if (newBso != null)
            {
                newBso.SetValues(values);
                string assetPath = "Assets/Resources/ScriptableObjects/" + savepath + "/" + values[1] +  ".asset";
                AssetDatabase.CreateAsset(newBso, assetPath);
                AssetDatabase.SaveAssets();
            }

        }
        AssetDatabase.Refresh();


    }


}
