using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/RoundInfo", order = 1)]
public class RoundInfo : ScriptableObject
{

    [SerializeField]
    private List<GameObject> monsterList = new List<GameObject>();

    public List<GameObject> MonsterList { get { return monsterList; } }

    [SerializeField]
    private List<Vector3Int> regenPosList = new List<Vector3Int>();
    
  


}
