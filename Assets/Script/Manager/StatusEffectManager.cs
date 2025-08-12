using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class StatusEffect
{
    public int stack;

    public virtual void Apply(GameObject target,int count)
    {
        // 공통적인 적용 로직 (예: 이펙트 시작 처리)
        stack += count;
    }

    public virtual void Remove(GameObject target)
    {
        // 공통적인 종료 로직 (예: 이펙트 종료 처리)
    }
}
 

public class DamageBuff : StatusEffect
{

    public override void Apply(GameObject target,int count)
    {
        base.Apply(target,count); // 공통 로직 호출
        var stats = target.GetComponent<UnitStatus>();
        stats.Damage += 0.1f * count;
    }

    public override void Remove(GameObject target)
    {
        var stats = target.GetComponent<UnitStatus>();
        stats.Damage -= 0.1f * stack;
    }
}

public class DamageDeBuFF : StatusEffect
{
    public override void Apply(GameObject target,int count)
    {
        base.Apply(target, count); // 공통 로직 호출
        var stats = target.GetComponent<UnitStatus>();

    }

    public override void Remove(GameObject target)
    {
        var stats = target.GetComponent<UnitStatus>();
    }
}

public class TruDamageDamageBuff : StatusEffect
{

    public override void Apply(GameObject target, int count)
    {
        base.Apply(target, count); // 공통 로직 호출
        
    }

    public override void Remove(GameObject target)
    {
        var stats = target.GetComponent<UnitStatus>();
        
    }
}

public class TrueDamageDeBuFF : StatusEffect
{
    public override void Apply(GameObject target, int count)
    {
        base.Apply(target, count); // 공통 로직 호출
        var stats = target.GetComponent<UnitStatus>();

    }

    public override void Remove(GameObject target)
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

    void Update()
    {
        
    }

    public void AddEffect(StatusEffect effect,int count)
    {
        effect.Apply(gameObject,count);
        activeEffects.Add((effect));
    
    }

    public void RemoveEffect(StatusEffect effect)
    {
        effect.Remove(gameObject);
        activeEffects.Remove(effect);
    }

   
    
}
