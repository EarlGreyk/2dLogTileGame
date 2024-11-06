using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterAIManager : MonoBehaviour
{
    // Start is called before the first frame update
    private List<MonsterUnit> monsters = new List< MonsterUnit>();
    public List<MonsterUnit> Monsters { get { return monsters; } }
    private Queue<MonsterUnit> actionMonsters = new Queue<MonsterUnit>();

    private MonsterUnit currentMonster;
    public MonsterUnit CurrentMonster { get { return currentMonster; } set { currentMonster = value; } }


    public void MonsterSet(MonsterUnit monster)
    {
        monsters.Add(monster);
    }


    public void MonsterRevmoe(MonsterUnit monster)
    {
        monsters.Remove(monster);
        GameManager.instance.GameProsessManager.killMonsterAdd(monster.Icon.name);
        if (monsters.Count == 0)
        {
            GameManager.instance.GameProsessManager.ProsessSet();


        }
    }

    private void Update()
    {
        if (currentMonster == null && actionMonsters.Count > 0)
        {
            AiEnable();
        }

        if(Input.GetKeyDown(KeyCode.P))
        {
            Debug.Log("몬스터 처치 명령");
            MonsterRevmoe(monsters[monsters.Count - 1]);

        }
            
    }


    /// <summary>
    /// 몬스터의 행동 카운트를 감소시킵니다.
    /// 만약 카운트를 감소시켰을때 0이 된 몬스터가 있다면 해당 몬스터를 활성화 몬스터 리스트 그룹에 넣습니다.
    /// </summary>
    public void MonsterCount()
    {
        for (int i = 0; i < monsters.Count; i++)
        {
            monsters[i].ActionCount--;
            if (monsters[i].ActionCount == 0)
            {
                actionMonsters.Enqueue(monsters[i]);   
            }
        }
    }

    public void AiEnable()
    {
        if (actionMonsters.Count > 0)
        {
            GameManager.instance.onMonsterAction();
            currentMonster = actionMonsters.Peek();
            actionMonsters.Dequeue();
            currentMonster.monsterAction();

        }
            
    }





}
