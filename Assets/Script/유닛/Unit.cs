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
    public void effectRatio(UnitStatusObject ratioTemp)
    {
        Health *= ratioTemp.Health;
        MaxHealth = Health;
        Damage *= ratioTemp.Damage;
        ElementalDamage *= ratioTemp.ElementalDamage;
        Defense *= ratioTemp.Defense;
        ItemChan *= ratioTemp.ItemChan;
        BlockGain *= ratioTemp.BlockChan;
        MagicChan *= ratioTemp.MagicChian;
        MagicCount *= ratioTemp.MagicCount;
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
        
        GameManager.instance.BattleZone.setTileUnit(transform.position, this);
        uicanvas = GameManager.instance.HPCanvas;
        uicanvasRectTransform = uicanvas.GetComponent<RectTransform>();
        


    }
    public virtual void Update()
    {

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
        Destroy(gameObject);
    }

}


