using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class StatusEffect
{
    public int stack;
    public Sprite IconSprite;

    public virtual void Apply(Unit target,int count)
    {
        // 공통적인 적용 로직 (예: 이펙트 시작 처리)
        stack += count;
        
    }

    public virtual void Remove(Unit target)
    {
        // 공통적인 종료 로직 (예: 이펙트 종료 처리)
    }
}
 

public class DamageBuff : StatusEffect
{

    public override void Apply(Unit target, int count)
    {
        base.Apply(target,count); // 공통 로직 호출
        IconSprite = Resources.Load<Sprite>("인터페이스/StatusEffect/Buff/DamageBuff");
        var stats = target.GetComponent<UnitStatus>();
        stats.Damage += 0.1f * count;
        

    }

    public override void Remove(Unit target)
    {
        var stats = target.GetComponent<UnitStatus>();
        stats.Damage -= 0.1f * stack;
    }
}

public class DamageDeBuff : StatusEffect
{
    public override void Apply(Unit target, int count)
    {
        base.Apply(target, count); // 공통 로직 호출
        IconSprite = Resources.Load<Sprite>("인터페이스/StatusEffect/DeBuff/DamageDeBuff");
        Debug.Log(IconSprite);
        var Unit = target.GetComponent<Unit>();
        Unit.status.Damage -= 0.1f * count;
        stack += count;

    }

    public override void Remove(Unit target)
    {
        var Unit = target.GetComponent<Unit>();
        Unit.status.Damage += 0.1f * stack;
    }
}

public class TruDamageDamageBuff : StatusEffect
{

    public override void Apply(Unit target, int count)
    {
        base.Apply(target, count); // 공통 로직 호출
        
    }

    public override void Remove(Unit target)
    {
        var stats = target.GetComponent<UnitStatus>();
        
    }
}

public class TrueDamageDeBuff : StatusEffect
{
    public override void Apply(Unit target, int count)
    {
        base.Apply(target, count); // 공통 로직 호출
        var stats = target.GetComponent<UnitStatus>();

    }

    public override void Remove(Unit target)
    {
        var stats = target.GetComponent<UnitStatus>();
    }
}
/// <summary>
/// 유닛은 해당 상태이상 관리를 반드시 하나 가지고 있어야합니다.
/// 상태이상을 관리합니다.
/// </summary>
public class StatusEffectManager: MonoBehaviour
{
    
    private List<StatusEffect> activeEffects = new List<StatusEffect>();

    private GameObject EffectPanel;

    private void Start()
    {
        GameObject canvas  = GameManager.instance.HPCanvas;
        EffectPanel = Instantiate<GameObject>(Resources.Load<GameObject>("인터페이스/StatusEffect/StatusEffectPanel"),canvas.transform);
        
    }



    void Update()
    {
        if(activeEffects.Count > 0)
        {
            Debug.Log($"{gameObject.name} 의 현재 활성화된 상태이상 개수 : {activeEffects.Count}");
        }
    }

    public void AddEffect(StatusEffect effect,int count,Unit target)
    {
        
        Debug.Log("상태이상 효과 추가");
        effect.Apply(target,count);
        activeEffects.Add((effect));

        if(effect.IconSprite != null)
        {
            GameObject EffectIcon = Instantiate<GameObject>(Resources.Load<GameObject>("인터페이스/StatusEffect/StatusEffectIcon"),EffectPanel.transform);
            Image IconImage = EffectIcon.GetComponent<Image>();
            IconImage.sprite = effect.IconSprite;



        }
            
    
    }

    public void RemoveEffect(StatusEffect effect,Unit target)
    {
        effect.Remove(target);
        activeEffects.Remove(effect);
    }

   
    
}
