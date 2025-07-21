using System.Collections;
using System.Collections.Generic;
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
    public MonsterListScriptableObejct monsterList;



    [SerializeField]
    private Canvas canvas;

    private Transform targetObj;

    //정화에 필요한 수치입니다.
    public int ClearValue;
   




    private void Start()
    {
        clear = false;
        battle = true;
        ClearValue = 50;
    }
    /// FeilidInfo 해서 해당 함수를 사용
    /// 선언시 정화 유닛의 몬스터 정보값을 수정합니다. 
    public void MonsterListSet(int value)
    {
        
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
            ClearValue /= 10;
            
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
                    GameManager.instance.BattleSet();
                }else
                {
                    if (!clear)
                    {
                        Debug.Log("정화시작");
                        clear = true;
                        GameManager.instance.GameProsessManager.ClearPanelSet(this, false);
                        GameManager.instance.ClearSet(ClearValue);
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
            GameManager.instance.GameProsessManager.ClearPanelSet(this, true);
            targetObj = other.gameObject.transform;

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
