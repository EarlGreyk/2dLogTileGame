using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/UnitStatus", order = 1)]
public class UnitStatusObject : ScriptableObject
{
    // Start is called before the first frame update
    [SerializeField]
    private int health;
    public int Health { get { return  health; }  }
    [SerializeField]
    private int damage;
    public int Damage { get { return damage; } }
    [SerializeField]
    private int elementalDamage;
    public int ElementalDamage {  get { return elementalDamage; } }
    [SerializeField]
    private int defense;
    public int Defense { get { return defense; } }
    public int ItemChan { get; }
    public int BlockChan { get; }
    public int MagicChian { get; }
    public int MagicCount { get; }

}
