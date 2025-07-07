using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterAIManager : MonoBehaviour
{
    // Start is called before the first frame update
    private List<MonsterUnit> monsters = new List< MonsterUnit>();
    public List<MonsterUnit> Monsters { get { return monsters; } }

    private Queue<MonsterUnit> actionMonsters = new Queue<MonsterUnit>();

    private MonsterUnit currentMonster = null;
    public MonsterUnit CurrentMonster { get { return currentMonster; } 
        set
        { currentMonster = value;} }


    public void MonsterSet(MonsterUnit monster)
    {
        monsters.Add(monster);
        GameManager.instance.UnitInfoManager.MonsterInfoAdd(monster);
    }

    /// <summary>
    /// 몬스터가 사망할때 요청합니다
    /// </summary>
    /// <param name="monster"></param>

    public void MonsterRevmoe(MonsterUnit monster)
    {
        monsters.Remove(monster);
        GameManager.instance.GameProsessManager.killMonsterAdd(monster.Sprite.name,monster.KillGold);
        if (monsters.Count == 0)
        {
            if (GameManager.instance.Stage == 3 & GameManager.instance.Round == 10)
                GameManager.instance.PlayerWin();
            //else
                //GameManager.instance.GameProsessManager.ProsessSet(false);
        }
    }
    /// <summary>
    /// 플레이어가 이동, 마법의 행동을 했을때 위치 및 상황이 변동 할 수 있음으로 행동 알고리즘을 재검색합니다.
    /// </summary>
    public void MonsterActionCheck()
    {
        Debug.Log("몬스터 행동 재설정");
        for (int i = 0; i < monsters.Count; i++)
        {
            monsters[i].ActionCheck();
        }
    }

    private void Update()
    {

        if (currentMonster == null && actionMonsters.Count >= 0 && GameManager.instance.IsMonater)
        {
            AiEnable();
        }
        if (Input.GetKeyDown(KeyCode.P))
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
            actionMonsters.Enqueue(monsters[i]);
        }
    }

    public void AiEnable()
    {
        if (actionMonsters.Count > 0)
        {
            GameManager.instance.onMonsterAction();
            currentMonster = actionMonsters.Peek();
            actionMonsters.Dequeue();
            currentMonster.ActionStart();

        }


        if (currentMonster == null && actionMonsters.Count == 0)
        {
            
            GameManager.instance.onPlayerAction();
            MonsterActionCheck();
        }


    }





}
