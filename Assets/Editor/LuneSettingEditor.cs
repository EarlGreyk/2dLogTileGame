    using System.Linq;
using UnityEditor;
using UnityEngine;
using static LuneSetting;


[CustomEditor(typeof(LuneSetting))]
public class LuneSettingEditor : Editor
{
    public override void OnInspectorGUI()
    {

        LuneSetting luneSetting = (LuneSetting)target;

        DrawDefaultInspector();

        LuneScriptableObejct newLuneData =  DrawScriptableObjectPopup<LuneScriptableObejct>("Select Basic Lune", luneSetting.LuneData);

        // 선택된 LuneData가 기존의 LuneData와 다르면 갱신
        if (newLuneData != luneSetting.LuneData)
        {
            luneSetting.LuneData = newLuneData;

            // 변경 사항 저장
            EditorUtility.SetDirty(luneSetting);
        }


        // 변경 사항 저장
        if (GUI.changed)
        {
            EditorUtility.SetDirty(luneSetting);
        }
    }

    // 특정 타입의 ScriptableObject 목록을 드로우하는 메소드
    private T DrawScriptableObjectPopup<T>(string label, T selectedObject) where T : LuneScriptableObejct
    {
        string[] guids = AssetDatabase.FindAssets($"t:{typeof(T).Name}");
        T[] objects = guids.Select(guid => AssetDatabase.LoadAssetAtPath<T>(AssetDatabase.GUIDToAssetPath(guid))).ToArray();

        if (objects.Length == 0)
        {
            EditorGUILayout.LabelField($"No {typeof(T).Name} found in project.");
            return null;
        }

        string[] objectNames = objects.Select(obj => obj.name).ToArray();
        int selectedIndex = System.Array.IndexOf(objects, selectedObject);
        int newSelectedIndex = EditorGUILayout.Popup(label, selectedIndex, objectNames);

        if (newSelectedIndex >= 0 && newSelectedIndex < objects.Length)
        {
            return objects[newSelectedIndex];
        }
        return null;
    }
}