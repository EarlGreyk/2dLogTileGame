using System;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/Pattern", order = 1)]
public class PatternData : ScriptableObject
{
    [Serializable]
    public struct PatternPoint
    {
        public int x;
        public int y;
    }

    public List<PatternPoint> points = new List<PatternPoint>();
}

