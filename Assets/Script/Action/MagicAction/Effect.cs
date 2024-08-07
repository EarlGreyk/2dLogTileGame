using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Effect : MonoBehaviour
{
    public Sprite magicEffect;

    public float time;
    public float endtime;

    private void Start()
    {
        time += Time.deltaTime;
        if (time > endtime)
        {
            endAction();
        }
    }

    private void endAction()
    {
        Destroy(gameObject);
    }

    public abstract void MagicAnimation();




    public abstract void MagicSetDamage();
    

    
}
