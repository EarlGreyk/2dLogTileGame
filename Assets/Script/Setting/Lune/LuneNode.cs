using System;
using System.Collections.Generic;
using UnityEngine;


public abstract class LuneNode : ScriptableObject
{
    public string LuneName;
    public string LuneDesc;


    public abstract void ApplyEffect(Unit character);
}



