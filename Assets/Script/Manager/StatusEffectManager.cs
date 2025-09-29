using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.GraphicsBuffer;

public interface ISubscribeEvents
{
    void Subscribe(StatusEffectManager manager, Unit owner);
    void Unsubscribe(StatusEffectManager manager, Unit owner);
}


public abstract class StatusEffect
{
    public int stack;
    //토큰 : 상태이상이 트리거 몇번 호출까지 지속되는지 를 체크합니다.
    public int token = 1;
    public Sprite IconSprite;

    // UI 참조 보관
    public GameObject UIIcon;
    public TextMeshProUGUI stackText; // TMP 사용 권장

    public virtual void Apply(Unit target, int count)
    {
        stack += count;
        UpdateStackText();
    }

    public virtual void Remove(Unit target)
    {
        Debug.Log("상태이상제거 완료 UI제거하도록!");
        if (UIIcon != null)
        {
            GameObject.Destroy(UIIcon);
            UIIcon = null;
            stackText = null;
        }
    }

    public void UpdateStackText()
    {
        if (stackText != null)
            stackText.text = stack.ToString();
    }

    protected void ConsumeToken(Unit owner)
    {
        token--;
        if (token <= 0)
        {
            owner.effectManager.RemoveEffect(this);
        }
    }
}
/// <summary>
/// 상태이상 효과 : 데미지 배율 증가
/// </summary>
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

/// <summary>
/// 가하는 데미지 배율이 감소합니다.
/// 소멸 트리거 : 데미지를 줄때 
/// </summary>

public class DamageDeBuff : StatusEffect, ISubscribeEvents
{
    private Unit owner;
    private float value = 0.2f;
    public override void Apply(Unit target, int count)
    {
        base.Apply(target, count); // 공통 로직 호출
        IconSprite = Resources.Load<Sprite>("Art/Token/12");
        var Unit = target.GetComponent<Unit>();
        owner = target;
        owner.status.Damage -= value * count;
        Debug.Log(owner.status.Damage);
        stack += count;

    }

    public override void Remove(Unit target)
    {
        base.Remove(target);
        var Unit = target.GetComponent<Unit>();
        Unit.status.Damage += value * stack;
        Debug.Log(owner.status.Damage);
    }

    public void Subscribe(StatusEffectManager manager,Unit ownr)
    {
        Debug.Log(ownr.name);
        manager.OnAttack += HandleAttack;
    }

    public void Unsubscribe(StatusEffectManager manager, Unit ownr)
    {
        Debug.Log(ownr.name);
        manager.OnAttack -= HandleAttack;
    }

    private void HandleAttack(Unit attacker)
    {
        if (attacker != owner)
            return;

        Debug.Log("핸들 작동 구독접근 스택감소");
        stack--;
        if (stack <= 0)
            attacker.effectManager.RemoveEffect(this);
        else
            ConsumeToken(attacker); // 토큰 1 소모 → 0이면 제거
    }
}


/// <summary>
/// 데미지가 value 값만큼 증가합니다.
/// 트리거 : 데미지를 줄때
/// </summary>

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

/// <summary>
/// 데미지가 value 값만큼 증가합니다.
/// 트리거 : 데미지를 받을때
/// </summary>

public class TrueDamageDeBuff : StatusEffect
{

    public void Subscribe(StatusEffectManager manager, Unit owner)
    {
        manager.OnDamageTaken += HandleDamageTaken;
    }

    public void Unsubscribe(StatusEffectManager manager, Unit owner)
    {
        manager.OnDamageTaken -= HandleDamageTaken;
    }


    private void HandleDamageTaken(Unit owner, int dmg)
    {
        int reflect = Mathf.CeilToInt(dmg * 3f * stack);

        Debug.Log($"{owner.name} 데미지 받음: {reflect} 고정피해 추가!");

        ConsumeToken(owner); // 토큰 1 소모 → 0이면 제거
    }

}
/// <summary>
/// 유닛은 해당 상태이상 관리를 반드시 하나 가지고 있어야합니다.
/// 상태이상을 관리합니다.
/// </summary>
public class StatusEffectManager: MonoBehaviour
{
   
    private List<StatusEffect> activeEffects = new List<StatusEffect>();
    public Unit targetUnit;


    // 이벤트 정의
    public event Action<Unit> OnTurnEnd;
    public event Action<Unit, int> OnDamageTaken;
    public event Action<Unit> OnTurnStart;
    public event Action<Unit> OnAttack;
    public event Action<Unit> OnDeath;

    /// <summary>
    /// UnitHpbar 라는 script 를 인터페이스 위치 이동하는 스크립트로 이름을 변경한후 아래의 있는 변수를 병합 해볼만합니다.
    /// </summary>
    public GameObject canvas;
    public RectTransform canvasRectTransform;
    public RectTransform rectStatusEffect;
    private GameObject EffectPanel;
    

    private void Awake()
    {
        targetUnit = GetComponent<Unit>();

        //UI 초기화
        canvas = GameManager.instance.HPCanvas;
        canvasRectTransform = canvas.GetComponent<RectTransform>();
        EffectPanel = Instantiate<GameObject>(Resources.Load<GameObject>("인터페이스/StatusEffect/StatusEffectPanel"), canvas.transform);
        rectStatusEffect = EffectPanel.GetComponent<RectTransform>();
    }

    



    void Update()
    {
        if(activeEffects.Count > 0)
        {
            UpdateStatusPanelPosition();
            /*
            foreach (var effect in activeEffects)
            {
                if (effect.UIIcon != null)
                    effect.UpdateStackText();
            }
            */

        }
    }

    /// <summary>
    /// 상태이상 추가
    /// </summary>
    /// <param name="effect"></부여될 상태이상 타입>
    /// <param name="count"></스택값>
  
    public void AddEffect(StatusEffect effect,int count)
    {
        
        

        StatusEffect existingEffect = activeEffects.Find(e => e.GetType() == effect.GetType());


        if (existingEffect != null)
        {
            // 2. 이미 있으면 stack만 올리고 Apply 갱신
            existingEffect.Apply(targetUnit, count);
        }else
        {
            //새로생성
            effect.Apply(targetUnit, count);
            activeEffects.Add(effect);



            // UI 생성
            if (effect.IconSprite != null)
            {
                GameObject icon = Instantiate(
                    Resources.Load<GameObject>("인터페이스/StatusEffect/StatusEffectIcon"),
                    EffectPanel.transform
                );
                icon.GetComponent<Image>().sprite = effect.IconSprite;
                //연결후 텍스트 표시
                effect.UIIcon = icon;
                effect.stackText = icon.GetComponentInChildren<TextMeshProUGUI>();
                effect.UpdateStackText();
            }
            // 이벤트 구독
            if (effect is ISubscribeEvents sub)
                sub.Subscribe(this, targetUnit);
        }

    
            
    
    }

    public void RemoveEffect(StatusEffect effect)
    {
        if (effect is ISubscribeEvents sub)
            sub.Unsubscribe(this, targetUnit);

        activeEffects.Remove(effect);
        effect.Remove(targetUnit);
    }

    // 이벤트 


  

  

    /// <summary>
    /// 트리거 호출함수
    /// </summary>

    public void TriggerTurnEnd() => OnTurnEnd?.Invoke(targetUnit);
    public void TriggerTurnStart() => OnTurnStart?.Invoke(targetUnit);
    public void TriggerDamageTaken(int damage) => OnDamageTaken?.Invoke(targetUnit, damage);
    public void TriggerAttack()
    {
        Debug.Log($"[StatusEffectManager] TriggerAttack called on {targetUnit.name}. Subscribers: {(OnAttack == null ? 0 : OnAttack.GetInvocationList().Length)}");
        OnAttack?.Invoke(targetUnit);
    }
    
    public void TriggerDeath() => OnDeath?.Invoke(targetUnit);

    /// 






    private void UpdateEffectIcon(StatusEffect effect)
    {
        if (effect.UIIcon != null)
        {
            Text stackText = effect.UIIcon.GetComponentInChildren<Text>();
            if (stackText != null)
            {
                stackText.text = effect.stack.ToString();
            }
        }
    }

    public void RemoveEffect(StatusEffect effect,Unit target)
    {
        effect.Remove(target);
        activeEffects.Remove(effect);
    }




    public void UpdateStatusPanelPosition()
    {
        Vector3 worldPos = new Vector3(targetUnit.transform.position.x, targetUnit.transform.position.y - 2f, targetUnit.transform.position.z);
        rectStatusEffect.position = worldPos;
        float scaleMultiplier = 0.5f / Camera.main.orthographicSize;
        rectStatusEffect.localScale = Vector3.one * (scaleMultiplier / 2);
    }




    public void OnDestroy()
    {
        Destroy(EffectPanel);
    }

}
