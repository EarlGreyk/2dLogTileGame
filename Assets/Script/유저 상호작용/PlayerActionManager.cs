using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;

/// <summary>
/// 플레이어가 지시받은 행동을 관리합니다.
/// </summary>

public class PlayerActionManager : MonoBehaviour
{
    private MagicOrigin magic;
    private GameObject magicEffect;
    private Vector3Int hitPoint;
    private List<Vector3Int> targetPos;    
    
   
     
    
    public void SettingSkillAction(MagicOrigin magic ,GameObject magicEffect,Vector3Int hitPoint ,List<Vector3Int> targetPos)
    {
        this.magic = magic;
        this.magicEffect = magicEffect;
        this.hitPoint = hitPoint;
        this.targetPos = targetPos;


        if(magic.MagicType == MagicOrigin.Type.Attack)
        {
            AttackMagicStart();
        }
        if (magic.MagicType == MagicOrigin.Type.Defence)
        {
            DefenceMagicStart();
        }
        if (magic.MagicType == MagicOrigin.Type.Buff)
        {
            BuffMagicStart();
        }
        if (magic.MagicType == MagicOrigin.Type.DeBuff)
        {
            DeBuffmagicStart();
        }


    }

    private void UnitAddBuffToKen(Unit target)
    {
        Debug.Log("버프부여 작동");
        StatusEffectManager EM = target.effectManager;

        if (magic.TokenIndex == 0)
        {
            
            return;
        }
        if (magic.TokenIndex == 1)
        {
            DamageBuff token = new DamageBuff();
            EM.AddEffect(token, magic.TokenCount);
            return;
        }

    }
    private void UnitAddDeBuffToKen(Unit target)
    {
        Debug.Log("디버프부여 작동");
        StatusEffectManager EM = target.effectManager;
        if (magic.TokenIndex == 0)
        {
            return;
        }
        if (magic.TokenIndex == 1)
        {
            DamageDeBuff token = new DamageDeBuff();
            EM.AddEffect(token, magic.TokenCount);
            return;
        }

    }

    //공격 마법을 작동합니다.
    private void AttackMagicStart()
    {
        //스케일에 맞춰 실제값으로 변동 시켜줘야합니다.
        Unit target = null;
        
        if(magicEffect != null)
        {
            GameObject effect = Instantiate<GameObject>(magicEffect);
            //// 이펙트생성자에서 변경해주면됨.
            Vector3 scale = GameManager.instance.Grid.transform.localScale;
            effect.transform.position = new Vector3(hitPoint.x * scale.x, hitPoint.y * scale.y, 0);
        }else
        {
            ErrorManager.instance.ErrorSet("Error : 마법 이펙트가 없습니다.");
        }
       


       
        
        for (int i = 0; i < targetPos.Count; i++)
        {
            target = GameManager.instance.BattleZone.SerchTileUnit(targetPos[i]);
            if (target != null && target != GameManager.instance.PlayerUnit)
            {
                Debug.Log($"{target},{magic.TokenType}");
                if(magic.TokenType == 1)
                {
                    UnitAddBuffToKen(target);
                }else if(magic.TokenType == 2)
                {
                    UnitAddDeBuffToKen(target);
                }
                
                target.HitDamage(magic.MagicDamage);
                target.effectManager.TriggerAttack();


            }
            
        }

        

    }

    /// <summary>
    /// 방어 마법을 작동합니다.
    /// 플레이어를 기준으로 하며 작동시 보호막이 생깁니다.
    /// 보호막은 중첩되지 않으며 보호막이 가장 많은 기술로 다시 갱신됩니다.
    /// </summary>

    private void DefenceMagicStart()
    {
        //스케일에 맞춰 실제값으로 변동 시켜줘야합니다.
        Unit target = null;

        GameObject effect = Instantiate<GameObject>(magicEffect);

        if (effect == null)
        {
            ErrorManager.instance.ErrorSet("Error : 마법 이펙트가 없습니다.");
        }


        //// 이펙트생성자에서 변경해주면됨.
        //// 이펙트 전용 관리자를 만들어서 거기에서 작동하게 할것.
        Vector3 scale = GameManager.instance.Grid.transform.localScale;
        effect.transform.position = new Vector3(hitPoint.x * scale.x, hitPoint.y * scale.y, 0);

        for (int i = 0; i < targetPos.Count; i++)
        {
            target = GameManager.instance.BattleZone.SerchTileUnit(targetPos[i]);
            if (target != null && target == GameManager.instance.PlayerUnit)
            {
                
            }

        }
    }

    private void BuffMagicStart()
    {

    }

    private void DeBuffmagicStart()
    {

    }
}
