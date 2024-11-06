using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/UnitStatus", order = 1)]
public class UnitStatusObject : ScriptableObject
{
    // Start is called before the first frame update
    [SerializeField]
    private float health;
    public float Health { get { return  health; }  }
    [SerializeField]
    private float damage;
    public float Damage { get { return damage; } }
    [SerializeField]
    private float elementalDamage;
    public float ElementalDamage {  get { return elementalDamage; } }
    [SerializeField]
    private float defense;
    public float Defense { get { return defense; } }
    public float ItemChan { get; }
    public float BlockChan { get; }
    public float MagicChian { get; }
    public float MagicCount { get; }

}
