using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

/// <summary>
/// 상호 작용이 가능한 유닛입니다.
/// 플레이어는 해당 유닛과 상호작용 하여 전투에 진입하거나 게임을 진행할 수 있습니다.
/// </summary>
public class ClearObject : InteractionObject
{
    //정화의 여부를 따집니다.
    public bool clear;

    //전투의 여부를 따집니다. 전투를 하지 않았다면 false 한번이라도 했다면 true로 전환됩니다
    public bool battle;

    //전투의 승리여부를 따집니다. 기본값을 false로합니다.
    public bool victory;

    // Start is called before the first frame update
    public List<MonsterScriptableObject> monsterList = new List<MonsterScriptableObject>();

   

    //정화에 필요한 렘프수치입니다.
    public float LampValue;

    //정화시 증가하는 정화 게이지수치입니다
    public float ClearValue;





    private void Awake()
    {
        interactionType = Type.Clear;
    }

    
    public void Start()
    {
        if(!SettingData.Load)
        {
            clear = false;
            battle = true;
            LampValue = 20;
            ClearValue = 50;


        }
    }
    /// FeilidInfo 해서 해당 함수를 사용
    /// 선언시 정화 유닛의 몬스터 정보값을 수정합니다. 
    public override void InteractSet()
    {
        base.InteractSet();
        //value가 처음부터 0이상이면 최초생성 된것이 아님으로 일반적으로 생성하면 됩니다
        if (value > 0)
        {
            MonsterScriptableObject[] monsterArray = Resources.LoadAll<MonsterScriptableObject>("ScriptableObjects/monster_data");

            List<MonsterScriptableObject> grade0 = new List<MonsterScriptableObject>();

            foreach (var monster in monsterArray)
            {
                if (monster == null) continue; // 타입 불일치 등으로 null 들어온 경우 스킵

                if (monster.Level == 1)
                    grade0.Add(monster);
            }



            if (grade0.Count > 0)
            {

                //Debug.Log($"grade0[0] is {(grade0[0] == null ? "NULL" : grade0[0].name)}"); // 등급몬스터 가 있는지 없는지 체크하고 이름을반환
            }

            while (value > 0)
            {
                int r = Random.Range(0, grade0.Count);
                monsterList.Add(grade0[r]);
                value -= grade0[r].Reward * 10;
            }
        }else
        {
            //보스 스테이지이며 반드시 보스몬스터가 할당되어야합니다.
            MonsterScriptableObject[] monsterArray = Resources.LoadAll<MonsterScriptableObject>("ScriptableObjects/monster_data");

            List<MonsterScriptableObject> grade0 = new List<MonsterScriptableObject>();
            foreach (var monster in monsterArray)
            {
                if (monster == null) continue; // 타입 불일치 등으로 null 들어온 경우 스킵

                //향후 현재 Stage값을 비교하여 걸러낸다음 더해야합니다.
                if (monster.Level == 4)
                    grade0.Add(monster);
            }
            for (int i = 0; i < grade0.Count; i++)
            {
                monsterList.Add(grade0[i]);
            }
            //보스 스테이지는 처음에 생성될떄 비활성화상태입니다.
            gameObject.SetActive(false);


        }






    }
    /// <summary>
    /// 전투 종료시 상호작용한 정화유닛에게 승리했는지 패배했는지 정보값을 넘겨주기 위한 함수입니다.
    /// 
    /// </summary>
    /// <param name="value"></승패채크>
    public override void InteractEnd()
    {
        base.InteractEnd();
        battle = false;
        if (victory)
        {
            LampValue *= 0.1f;

        }
        GameProsessManager.instance.LampLight -= LampValue;
        

    }

    public override void InteractStart()
    {
        base.InteractStart();
        if (battle)
        {
            Debug.Log("전투전환");
            GameManager.instance.BattleSet(monsterList);
            gameObject.SetActive(false);
        }
        else
        {
            if (!clear)
            {
                Debug.Log("정화시작");
                clear = true;
         
                //상호작용 패널 닫기
                GameProsessManager.instance.InteractionPanelSet(this, false);
                //상호작용한 유닛의 램프감소와 정화수치 증가값 넘기기
                GameProsessManager.instance.ClearSet(LampValue, ClearValue);
            }
        }

    }


    public override void PlayerColiderEnter()
    {
        base.PlayerColiderEnter();
        if(battle)
        {
            GameProsessManager.instance.ClearPanelSet(this, true, monsterList);
        }
        
    }

    public override void PlayerColiderExit()
    {
        base.PlayerColiderExit();
        if(battle)
        {
            GameProsessManager.instance.ClearPanelSet(this, false);
        }
        
    }











}
