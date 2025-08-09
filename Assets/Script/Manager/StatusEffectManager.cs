using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class StatusEffect
{
    public abstract float Duration { get; }

    public virtual void Apply(GameObject target)
    {
        // 공통적인 적용 로직 (예: 이펙트 시작 처리)
    }

    public virtual void Remove(GameObject target)
    {
        // 공통적인 종료 로직 (예: 이펙트 종료 처리)
    }
}

public class SpeedBuff : StatusEffect
{
    public override float Duration => 5f;

    public override void Apply(GameObject target)
    {
        base.Apply(target); // 공통 로직 호출
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
    
    private List<(StatusEffect effect, float timer)> activeEffects = new List<(StatusEffect, float)>();

    void Update()
    {
        for (int i = activeEffects.Count - 1; i >= 0; i--)
        {
            activeEffects[i] = (activeEffects[i].effect, activeEffects[i].timer - Time.deltaTime);

            if (activeEffects[i].timer <= 0)
            {
                activeEffects[i].effect.Remove(gameObject);
                activeEffects.RemoveAt(i);
            }
        }
    }

    public void AddEffect(StatusEffect effect)
    {
        effect.Apply(gameObject);
        activeEffects.Add((effect, effect.Duration));
    
    }
    
}
