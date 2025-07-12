using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class UnitStatus
{
    //체력
    public float Health { get; set; }
    public float MaxHealth { get; set; }
    //무속성 데미지 배율
    public float Damage { get; set; }
    //속성 데미지
    public float ElementalDamage { get; set; } // 속성 데미지
    //보호막 생성배율
    public float Defense { get; set; }
    //보상시 재화 증가율
    public float ItemChan { get; set; }
    /// <summary>
    /// 기원(드로우)시 블록 획득율
    /// </summary>
    public float BlockGain { get; set; }
    /// <summary>
    /// 마나 획득시 한번더 획득할 배율
    /// </summary>
    public float MagicChan { get; set; }

    //마나 획득시 추가 획득율
    public float MagicCount { get; set; }

    public float CriChan { get; set; }

    public float CriMul {  get; set; }
    public UnitStatus()
    {
        Health = 0;
        MaxHealth = 0;
        Damage = 1;
        ElementalDamage = 1;
        Defense = 1;
        ItemChan = 1;
        BlockGain = 1; 
        MagicChan = 0;
        MagicCount = 0;
        CriChan = 0;
        CriMul = 1.5f;
    }
    public UnitStatus(UnitStatusObject status)
    {
        Health = status.Health;
        MaxHealth = Health;
        Damage = status.Damage;
        ElementalDamage = status.ElementalDamage;
        Defense = status.Defense;
        ItemChan = 0;
        BlockGain = 0; 
        MagicChan = 0;
        MagicCount = 0;
    }

 
    public void effectCopy(UnitStatus copyTemp)
    {
        Health = copyTemp.Health;
        MaxHealth = copyTemp.Health;
        Damage = copyTemp.Damage;
        ElementalDamage = copyTemp.ElementalDamage;
        Defense = copyTemp.Defense;
        ItemChan = copyTemp.ItemChan;
        BlockGain = copyTemp.BlockGain;
        MagicChan = copyTemp.MagicChan;
        MagicCount = copyTemp.MagicCount;
    }
    public void effectAdd(UnitStatus addTemp)
    {
        Health += addTemp.Health;
        MaxHealth += addTemp.Health;
        Damage += addTemp.Damage;
        ElementalDamage += addTemp.ElementalDamage;
        Defense += addTemp.Defense;
        ItemChan += addTemp.ItemChan;
        BlockGain += addTemp.BlockGain;
        MagicChan += addTemp.MagicChan;
        MagicCount += addTemp.MagicCount;
    }
    /// <summary>
    /// 일반 몬스터 유닛의 스테이터스 배율을 조정하기 위해 사용합니다.
    /// </summary>
    /// <param name="ratioTemp"></받아올 스테이터스>
    /// <param name="grade"></false = 노말 몬스터 , true = 보스 몬스터  [기본값은 false입니다.]>
    public void effectRatio(MonsterScriptableObject ratioTemp, bool grade = false)
    {
        float value = 1f;

        if(!grade)
        {
            ///도전자의 메달 (5009) , 호통의 메달(5011)
            if (SettingData.difficultMonster.ContainsKey(5009))
                value += SettingData.difficultMonster[5009];
            if (SettingData.difficultMonster.ContainsKey(5011))
                value += SettingData.difficultMonster[5011];

            Health *= ratioTemp.HpValue * value;
            MaxHealth = Health;

            value = 1f;
            // 도발의 메달 (5010) , 어릿광대의 메달 (5012)
            if (SettingData.difficultMonster.ContainsKey(5010))
                value += SettingData.difficultMonster[5010];
            if (SettingData.difficultMonster.ContainsKey(5012))
                value += SettingData.difficultMonster[5012];

            Damage *= ratioTemp.NonElementalDamageValue * value;
            ElementalDamage *= ratioTemp.ElementalDamageValue * value;

            value = 1f;
            // 박살의 메달(5013)
            if (SettingData.difficultMonster.ContainsKey(5013))
            {
                value += SettingData.difficultMonster[5013];
            }
            Defense *= ratioTemp.ReducionValue * value;
        }else
        {
            ///대장의 메달 (5014) , 지도자의 메달(5016)
            if (SettingData.difficultMonster.ContainsKey(5014))
                value += SettingData.difficultMonster[5014];
            if (SettingData.difficultMonster.ContainsKey(5016))
                value += SettingData.difficultMonster[5016];

            Health *= ratioTemp.HpValue * value;
            MaxHealth = Health;

            value = 1f;
            // 보안관의 메달 (5015) , 군주의 메달(5017)
            if (SettingData.difficultMonster.ContainsKey(5015))
                value += SettingData.difficultMonster[5015];
            if (SettingData.difficultMonster.ContainsKey(5017))
                value += SettingData.difficultMonster[5017];

            Damage *= ratioTemp.NonElementalDamageValue * value;
            ElementalDamage *= ratioTemp.ElementalDamageValue * value;

            value = 1f;
            // 건실의 메달(5013)
            if (SettingData.difficultMonster.ContainsKey(5018))
            {
                value += SettingData.difficultMonster[5018];
            }
            Defense *= ratioTemp.ReducionValue * value;
        }

    }
  

    public void effectUp(string effectString, float effectValue)
    {
        switch (effectString)
        {
            case "Health":
                Health += effectValue;
                break;
            case "Damage":
                Damage += effectValue;
                break;
            case "ElementalDamage":
                ElementalDamage += effectValue;
                break;
            case "Defence":
                Defense += effectValue;
                break;
            case "ItemChan":
                ItemChan += effectValue;
                break;
            case "BlockChan":
                BlockGain += effectValue;
                break;
            case "MagicChain":
                MagicChan += effectValue;
                break;
            case "MagicCount":
                MagicCount += effectValue;
                break;

        }
        Debug.Log($"룬증가 방어력 : {Defense}");

    }







}

public class Unit :MonoBehaviour
{

    public UnitStatus status;
    public GameObject HPbar;
    public Sprite Sprite;
    public SpriteRenderer SpriteRenderer;
    public UnitStatusObject baseStatus;
    public UnitHpBar hpbar;
    private GameObject uicanvas;
    private RectTransform uicanvasRectTransform;


    public virtual void Start()
    {
        status = new UnitStatus(baseStatus);
        GameObject obj = Instantiate(HPbar, GameManager.instance.HPCanvas.transform);
        hpbar = obj.GetComponent<UnitHpBar>();
        hpbar.HpbarSet(this);
        
        uicanvas = GameManager.instance.HPCanvas;
        uicanvasRectTransform = uicanvas.GetComponent<RectTransform>();
        SpriteRenderer = GetComponent<SpriteRenderer>();
        


    }
    public virtual void Update()
    {
        /*   몬스터의 체력바 밑 텍스트 활성화 문구입니다. 
        if (CheckVisibility())
        {

            hpbar.rectHpbar.gameObject.SetActive(true);
            hpbar.UpdateHpbarPosition();

            if (hpbar.rectAction != null)
            {
                hpbar.UpdateActionPosition();
                hpbar.rectAction.gameObject.SetActive(true);
            }

        }
        else
        {
            hpbar.rectHpbar.gameObject.SetActive(false);
            if (hpbar.rectAction != null)
            {
                hpbar.rectAction.gameObject.SetActive(false);
            }
        }
        */

    }
    private bool CheckVisibility()
    {
        // 유닛의 월드 위치를 UI 캔버스의 로컬 좌표로 변환
        Vector2 screenPoint = RectTransformUtility.WorldToScreenPoint(Camera.main, transform.position);

        // 캔버스의 RectTransform을 기반으로 화면 좌표를 로컬 좌표로 변환
        Vector2 localPoint;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(uicanvasRectTransform, screenPoint, null, out localPoint);

        // RectTransform의 크기와 비교하여 유닛이 캔버스 내에 있는지 확인
        return uicanvasRectTransform.rect.Contains(localPoint); ;
    }


    public virtual void HitDamage(float Damage)
    {
        status.Health -= Damage;
        hpbar.HpTextSet();
        if(status.Health <=0)
        {
            UnitDie();

        }
    }
    public virtual void UnitDie()
    {

        Destroy(hpbar.gameObject);
    }

}


