using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

/// <summary>
/// 상호 작용이 가능한 유닛입니다.
/// 플레이어는 해당 유닛과 상호작용 하여 전투에 진입하거나 게임을 진행할 수 있습니다.
/// </summary>
public class ClearUnit : MonoBehaviour
{
    //정화의 여부를 따집니다.
    public bool clear;

    //전투의 여부를 따집니다. 전투를 하지 않았다면 false 한번이라도 했다면 true로 전환됩니다
    public bool battle;

    
  
    // Start is called before the first frame update
    public List<MonsterScriptableObject> monsterList = new List<MonsterScriptableObject>();



    [SerializeField]
    private Canvas canvas;

    private Transform targetObj;

    //정화에 필요한 렘프수치입니다.
    public float LampValue;

    //정화시 증가하는 정화 게이지수치입니다
    public float ClearValue;
   




    private void Start()
    {
        clear = false;
        battle = true;
        LampValue = 20;
        ClearValue = 50;
    }
    /// FeilidInfo 해서 해당 함수를 사용
    /// 선언시 정화 유닛의 몬스터 정보값을 수정합니다. 
    public void MonsterListSet(int value)
    {
        MonsterScriptableObject[] monsterArray = Resources.LoadAll<MonsterScriptableObject>("ScriptableObjects/monster_data");

        List<MonsterScriptableObject> grade0 = new List<MonsterScriptableObject>();

        foreach (var monster in monsterArray)
        {
            if (monster == null) continue; // 타입 불일치 등으로 null 들어온 경우 스킵

            if (monster.Level == 0)
                grade0.Add(monster);
        }

     
        Debug.Log($"grade0.Count = {grade0.Count}");
        Debug.Log(monsterList.Count);
        if (grade0.Count > 0)
        {
            Debug.Log($"grade0[0] is {(grade0[0] == null ? "NULL" : grade0[0].name)}");
        }

        if (value<10)
        {
            for(int k = 0; k<value /2; k++)
            {
                int r = Random.Range(0, grade0.Count);

                monsterList.Add(grade0[r]);
            }
            
        }
            
        
            
    }
    /// <summary>
    /// 전투 종료시 상호작용한 정화유닛에게 승리했는지 패배했는지 정보값을 넘겨주기 위한 함수입니다.
    /// </summary>
    /// <param name="value"></승패채크>
    public void BattleCheck(bool value)
    {
        battle = false;
        if (value)
        {
            LampValue *= 0.1f;
            
        }
        else
        {
          
        }
    }
  



    private void Update()
    {
        if (targetObj != null)
        {
            if (Input.GetKey(KeyCode.G) && GameManager.instance.GameProsessManager.prosessType == GameProsessManager.ProsessType.Stay)
            {
                if(battle)
                {
                    Debug.Log("전투전환");
                    GameManager.instance.BattleSet(monsterList);
                }else
                {
                    if (!clear)
                    {
                        Debug.Log("정화시작");
                        clear = true;
                        GameManager.instance.GameProsessManager.ClearPanelSet(this, false);
                        GameManager.instance.ClearSet(LampValue,ClearValue);
                    }
                    
                }
                    
                

            }

          

        }
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (clear)
            return;
            
        if (other.CompareTag("Player"))
        {
            Debug.Log("플레이어가 정화 유닛에 접근햇습니다.");
            if(battle)
            {
                GameManager.instance.GameProsessManager.ClearPanelSet(this, true, monsterList);
                targetObj = other.gameObject.transform;
            }
            

        }

    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (clear)
            return;


        if (other.CompareTag("Player"))
        {
            Debug.Log("플레이어가 정화 유닛에 나갔습니다.");
            GameManager.instance.GameProsessManager.ClearPanelSet(this, false);
            targetObj = null;
        }
    }







}
