using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class UnitStatus
{
    //ü��
    public float Health { get; set; }
    public float MaxHealth { get; set; }
    //���Ӽ� ������ ����
    public float Damage { get; set; }
    //�Ӽ� ������
    public float ElementalDamage { get; set; } // �Ӽ� ������
    //��ȣ�� ��������
    public float Defense { get; set; }
    //����� ��ȭ ������
    public float ItemChan { get; set; }
    /// <summary>
    /// ���(��ο�)�� ��� ȹ����
    /// </summary>
    public float BlockGain { get; set; }
    /// <summary>
    /// ���� ȹ��� �ѹ��� ȹ���� ����
    /// </summary>
    public float MagicChan { get; set; }

    //���� ȹ��� �߰� ȹ����
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
        Debug.Log($"������ ���� : {Defense}");

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
        // ������ ���� ��ġ�� UI ĵ������ ���� ��ǥ�� ��ȯ
        Vector2 screenPoint = RectTransformUtility.WorldToScreenPoint(Camera.main, transform.position);

        // ĵ������ RectTransform�� ������� ȭ�� ��ǥ�� ���� ��ǥ�� ��ȯ
        Vector2 localPoint;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(uicanvasRectTransform, screenPoint, null, out localPoint);

        // RectTransform�� ũ��� ���Ͽ� ������ ĵ���� ���� �ִ��� Ȯ��
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


