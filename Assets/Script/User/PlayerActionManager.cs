using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class PlayerActionManager : MonoBehaviour
{
    private Magic magic;
    private GameObject magicEffect;
    private Vector3Int hitPoint;
    private List<Vector3Int> targetPos;    
    
   
     
    
    public void SettingSkillAction(Magic magic ,GameObject magicEffect,Vector3Int hitPoint ,List<Vector3Int> targetPos)
    {
        this.magic = magic;
        this.magicEffect = magicEffect;
        this.hitPoint = hitPoint;
        this.targetPos = targetPos;
        ActionStart();
        
    }

    private void ActionStart()
    {
        GameObject effect = Instantiate<GameObject>(magicEffect);
        effect.transform.position = hitPoint;

    }
}
