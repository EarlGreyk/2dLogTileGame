using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MagicAcion : MonoBehaviour
{
    public float time;
    public float endtime;
    public Sprite magicEffect;
    
   
    public abstract void MagicAnimation();




    public abstract void MagicSetDamage();
    

    
}
