using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
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
        //�����Ͽ� ���� ���������� ���� ��������մϴ�.
        Unit target = null;
        GameObject effect = Instantiate<GameObject>(magicEffect);
        
        //// ����Ʈ�����ڿ��� �������ָ��.
        Vector3 scale = GameManager.instance.Grid.transform.localScale;
        effect.transform.position = new Vector3(hitPoint.x * scale.x, hitPoint.y * scale.y, 0);
        
        for (int i = 0; i < targetPos.Count; i++)
        {
            target = GameManager.instance.BattleZone.SerchTileUnit(targetPos[i]);
            if(target != null)
            {
                target.HitDamage(magic.MagicValue);
                Debug.Log(target);
            }
            
        }

    }
}
