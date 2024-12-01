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

        luneSetting.luneType = (LuneType)EditorGUILayout.EnumPopup("Lune Type", luneSetting.luneType);

        // LuneType에 따른 ScriptableObject 선택 UI
        switch (luneSetting.luneType)
        {
            case LuneType.Basic:
                luneSetting.selectedBasicLune = DrawScriptableObjectPopup<BagicLune>("Select Basic Lune", luneSetting.selectedBasicLune);
                luneSetting.selectedMajorLune = null; // Ensure the other type is null
                break;
            case LuneType.Major:
                luneSetting.selectedMajorLune = DrawScriptableObjectPopup<MajorLune>("Select Major Lune", luneSetting.selectedMajorLune);
                luneSetting.selectedBasicLune = null; // Ensure the other type is null
                break;
        }

        // 변경 사항 저장
        if (GUI.changed)
        {
            EditorUtility.SetDirty(luneSetting);
        }
    }

    // 특정 타입의 ScriptableObject 목록을 드로우하는 메소드
    private T DrawScriptableObjectPopup<T>(string label, T selectedObject) where T : ScriptableObject
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