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

    //전투후 승리 여부를 따집니다. 전투를 하고 승리했다면 true로 변경됩니다.
    public bool victory;
    // Start is called before the first frame update
    public MonsterListScriptableObejct monsterList;



    [SerializeField]
    private Canvas canvas;

    private Transform targetObj;



    void Start()
    {
        clear = false;
        battle = true;
        victory = false;
    }

    private void Update()
    {
        if (targetObj != null)
        {
            if (Input.GetKey(KeyCode.G) && GameManager.instance.GameProsessManager.prosessType == GameProsessManager.ProsessType.Stay)
            {
                Debug.Log("전투전환");
                GameManager.instance.RoundSet();

            }

          

        }
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("플레이어가 정화 유닛에 접근햇습니다.");
            GameManager.instance.GameProsessManager.ClearPanelSet(this, true);
            targetObj = other.gameObject.transform;

        }

    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("플레이어가 정화 유닛에 나갔습니다.");
            GameManager.instance.GameProsessManager.ClearPanelSet(this, false);
            targetObj = null;
        }
    }







}
