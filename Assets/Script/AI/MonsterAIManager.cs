using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterAIManager : MonoBehaviour
{
    // Start is called before the first frame update
    private List<Unit> monsters = new List<Unit>();



    public void MonsterSet(Unit monster)
    {
        monsters.Add(monster);
    }


    public void MonsterRevmoe()
    {
        monsters.Clear();
    }


    public void AiEnable()
    {

    }





}
