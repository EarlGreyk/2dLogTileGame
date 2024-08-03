using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(PatternData))]
public class PatternDataEditor : Editor
{
    private const int MaxValue = 5; // 5x5 �׸���

    public override void OnInspectorGUI()
    {
        PatternData patternData = (PatternData)target;

        // 2���� �迭 �ʱ�ȭ
        int[,] grid = new int[MaxValue, MaxValue];
        foreach (var point in patternData.points)
        {
            if (point.x < MaxValue && point.y < MaxValue)
            {
                grid[point.y, point.x] = 1; // y, x�� ����
            }
        }

        // 5x5 �׸��� ǥ��
        for (int y = 0; y < MaxValue; y++)
        {
            EditorGUILayout.BeginHorizontal();
            for (int x = 0; x < MaxValue; x++)
            {
                bool isFilled = grid[y, x] == 1;
                bool newIsFilled = EditorGUILayout.Toggle(isFilled, GUILayout.Width(20));
                if (newIsFilled != isFilled)
                {
                    if (newIsFilled)
                    {
                        patternData.points.Add(new PatternData.PatternPoint { x = x, y = y });
                    }
                    else
                    {
                        patternData.points.RemoveAll(p => p.x == x && p.y == y);
                    }
                    EditorUtility.SetDirty(patternData);
                }
            }
            EditorGUILayout.EndHorizontal();
        }
    }
}
