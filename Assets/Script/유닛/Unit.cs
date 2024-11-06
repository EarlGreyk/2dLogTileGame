using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;
using static System.Collections.Specialized.BitVector32;
using static UnityEditor.Timeline.Actions.MenuPriority;

public class UnitStatus
{
    public float Health { get; set; }
    public float MaxHealth { get; set; }
    public float Damage { get; set; }
    public float ElementalDamage { get; set; } // 속성 데미지
    public float Defense { get; set; }
    public float ItemChan { get; set; }
    public float BlockChan { get; set; }
    public float MagicChian { get; set; }
    public float MagicCount { get; set; }
    public UnitStatus()
    {
        Health = 0;
        MaxHealth = 0;
        Damage = 0;
        ElementalDamage = 0;
        Defense = 0;
        ItemChan = 0;
        BlockChan = 0; 
        MagicChian = 0;
        MagicCount = 0;
    }
    public UnitStatus(UnitStatusObject status)
    {
        Health = status.Health;
        MaxHealth = Health;
        Damage = status.Damage;
        ElementalDamage = status.ElementalDamage;
        Defense = status.Defense;
        ItemChan = 0;
        BlockChan = 0; 
        MagicChian = 0;
        MagicCount = 0;
    }

    public void effectUp(string effectString, int effectValue)
    {
        switch(effectString)
        {
            case "Health": Health += effectValue;
                break;
            case "Damage": Damage += effectValue;
                break;
            case "ElementalDamage": ElementalDamage += effectValue;
                break;
            case "Defence":Defense += effectValue;
                break;
            case "ItemChan":ItemChan += effectValue;
                break;
            case "BlockChan": BlockChan += effectValue;
                break;
            case "MagicChain": MagicChian += effectValue;
                break;
            case "MagicCount": MagicCount += effectValue;
                break;

        }
        Debug.Log($"룬증가 방어력 : {Defense}");

    }
    public void effectCopy(UnitStatus copyTemp)
    {
        Health = copyTemp.Health;
        Damage = copyTemp.Damage;
        ElementalDamage = copyTemp.ElementalDamage;
        Defense = copyTemp.Defense;
        ItemChan = copyTemp.ItemChan;
        BlockChan = copyTemp.BlockChan;
        MagicChian = copyTemp.MagicChian;
        MagicCount = copyTemp.MagicCount;
    }
    public void effectAdd(UnitStatus addTemp)
    {
        Health += addTemp.Health;
        Damage += addTemp.Damage;
        ElementalDamage += addTemp.ElementalDamage;
        Defense += addTemp.Defense;
        ItemChan += addTemp.ItemChan;
        BlockChan += addTemp.BlockChan;
        MagicChian += addTemp.MagicChian;
        MagicCount += addTemp.MagicCount;
    }
    public void effectRatio(UnitStatusObject ratioTemp)
    {
        Health *= ratioTemp.Health;
        Damage *= ratioTemp.Damage;
        ElementalDamage *= ratioTemp.ElementalDamage;
        Defense *= ratioTemp.Defense;
        ItemChan *= ratioTemp.ItemChan;
        BlockChan *= ratioTemp.BlockChan;
        MagicChian *= ratioTemp.MagicChian;
        MagicCount *= ratioTemp.MagicCount;
    }

}

public class Unit :MonoBehaviour
{

    public UnitStatus status;
    public GameObject HPbar;
    public GameObject Icon;
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
            hpbar.rectIcon.gameObject.SetActive(false);
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
            hpbar.rectIcon.gameObject.SetActive(true);
            hpbar.UpdateIconPosition();
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


    public void HitDamage(int Damage)
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


