using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitStatus
{
    public int Health { get; set; }
    public int Damage { get; set; }
    public int ElementalDamage { get; set; } // 속성 데미지
    public int Defense { get; set; }
    public int ItemChan { get; set; }
    public int BlockChan { get; set; }
    public int MagicChian { get; set; }
    public int MagicCount { get; set; }

    public UnitStatus()
    {
        Health = 0;
        Damage = 0;
        ElementalDamage = 0;
        Defense = 0;
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

}

public class Unit :MonoBehaviour
{

    public UnitStatus status;

    public virtual void Start()
    {
        GameManager.instance.BattleZone.setTileUnit(transform.position, this);
    }
    public void ApplyNodeEffect(LuneNode node)
    {
        node.ApplyEffect(this);
    }
}


