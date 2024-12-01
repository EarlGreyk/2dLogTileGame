using System;
using System.Collections.Generic;
using UnityEngine;


public abstract class LuneNode : ScriptableObject
{
    public string LuneName;
    public string LuneDesc;
    public int LunePrice;
    public Sprite LuneSprite;


    public abstract void ApplyEffect(Unit character);
}



