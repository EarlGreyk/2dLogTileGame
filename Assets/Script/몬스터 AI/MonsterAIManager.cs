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
    /// <param name="monster"></사망하는 몬스터 대상>
    /// /// <param name="infoAdd"></보상을 위해 몬스터 킬 정보를 넣을지 체크>

    public void MonsterRevmoe(MonsterUnit monster, bool infoAdd = true)
    {
        GameObject obj = monster.gameObject;
        //유닛을 지우기 전에 유닛과 관련된 GUI를 제거합니다
        Destroy(monster.hpbar.actiontext.gameObject);
        Destroy(monster.hpbar.gameObject);
        //유닛을 제거하기전에 리스트에서 제거합니다
        
        monsters.Remove(monster);
        GameManager.instance.UnitInfoManager.MonsterInfoRemove(monster);
        if(infoAdd)
        {
            GameProsessManager.instance.killMonsterAdd(monster.Sprite.name, monster.KillGold);
        }
        
        //유닛제거
        Destroy(obj);
        
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

        if (currentMonster == null && actionMonsters.Count >= 0 && GameManager.instance.IsMonater && GameManager.instance.PlayerUnit !=null)
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

    /// <summary>
    /// 몬스터의 AI를 활성화합니다.
    /// </summary>

    public void AiEnable()
    {
        if (actionMonsters.Count > 0)
        {
            currentMonster = actionMonsters.Peek();
            actionMonsters.Dequeue();
            currentMonster.ActionStart();

        }


        if (currentMonster == null && actionMonsters.Count == 0)
        {
            
            GameManager.instance.PlayerTurnStart();
            MonsterActionCheck();
        }


    }


    /// <summary>
    /// 현재 등록되어 있는 모든 몬스터를 파괴하고 소거합니다.
    /// </summary>

    public void MonsterReset()
    {
        while(monsters.Count>0)
            MonsterRevmoe(monsters[monsters.Count - 1] , false);


      

    }





}
