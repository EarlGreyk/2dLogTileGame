using System;
using System.Collections.Generic;
using UnityEngine;


public abstract class LuneNode : ScriptableObject
{
    public string LuneName;
    public string LuneDesc;
    public int CurrentLevel;
    public int MaxLevel;
    public int ConnectingLune;

    public abstract void ApplyEffect(Unit character);
}



