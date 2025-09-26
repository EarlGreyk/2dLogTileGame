using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;


public class PlayerUnit : Unit
{

    public bool ColiderCheck = false;

    public BoxCollider2D boxCollider2D;

    private Vector3 previousPosition;

    private InteractionObject targetinteraction;

    private Rigidbody2D rb;


    // Start is called before the first frame update
    public override void Awake()
    {
        base.Awake();

        status.effectAdd(SettingData.LuneStatus);
        rb = GetComponent<Rigidbody2D>();

 
    }

    public override void Update()
    {
        base.Update();
        if (GameProsessManager.instance.prosessType == GameProsessManager.ProsessType.Battle)
            return;

        if (GameProsessManager.instance.prosessType == GameProsessManager.ProsessType.Rest)
            return;


        if (Input.GetKey(KeyCode.G) && targetinteraction != null)
        {
            Debug.Log("대상자 :" + targetinteraction);
            targetinteraction.InteractStart();
            
        }

      
    }
    private void FixedUpdate()
    {
        //아래는 이동
        if (GameProsessManager.instance.prosessType == GameProsessManager.ProsessType.Battle)
            return;

        if (GameProsessManager.instance.prosessType == GameProsessManager.ProsessType.Rest)
            return;


        if (previousPosition != transform.position)
            previousPosition = transform.position;


        //아래는 전투가 아닐때만 작동합니다.
        //유닛 기본이동구현
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
        {

            rb.MovePosition(rb.position+(5f * Time.fixedDeltaTime * Vector2.up));
        }

        // S 키를 눌렀을 때, 카메라를 아래쪽(Y축)으로 이동
        if (Input.GetKey(KeyCode.S))
        {

            rb.MovePosition(rb.position + (5f * Time.fixedDeltaTime * Vector2.down));
        }

        // A 키를 눌렀을 때, 카메라를 왼쪽으로 이동
        if (Input.GetKey(KeyCode.A))
        {

            rb.MovePosition(rb.position + (5f * Time.fixedDeltaTime * Vector2.left));
        }

        // D 키를 눌렀을 때, 카메라를 오른쪽으로 이동
        if (Input.GetKey(KeyCode.D))
        {

            rb.MovePosition(rb.position + (5f * Time.fixedDeltaTime * Vector2.right));
        }
    }


    public override void HitDamage(float Damage)
    {
        base.HitDamage(Damage);
    }
    /// <summary>
    /// 지역 이동 + 상호작용 오브젝트 접촉을 체크합니다.
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("접촉");
        Debug.Log(ColiderCheck);
        if (GameProsessManager.instance.prosessType == GameProsessManager.ProsessType.Battle)
            return;

        if (other.CompareTag("Interaction"))
        {
            InteractionObject interaction = other.gameObject.GetComponent<InteractionObject>();
            targetinteraction = interaction;
            interaction.PlayerColiderEnter();
        }
        


        if (other.CompareTag("MoveTrigger") && ColiderCheck == false)
        {
            Debug.Log("이동준비 슈우우웃!");
            if (!MapGenerator.Instance.spawnedTilemaps.ContainsKey(GameManager.instance.CurrentPos))
            {
                Debug.Log("현재 해당 값은 딕셔너리에 없음");
                Debug.Log($"비교좌표 : {GameManager.instance.CurrentPos} :: 있는여부 {MapGenerator.Instance.spawnedTilemaps[GameManager.instance.CurrentPos]}");

                return;
            }

            GameObject tilemap = MapGenerator.Instance.spawnedTilemaps[GameManager.instance.CurrentPos];
          
            if (tilemap == other.gameObject.transform.parent.gameObject)
            {
                
                Vector2 moveDirection = (Vector2)transform.position - (Vector2)previousPosition;
                StartCoroutine(GameManager.instance.PlayerStop(1f));
       
                //서쪽
                if (moveDirection.x>0)
                {
                    Debug.Log("동쪽");


                    if (MapGenerator.Instance.tileMapInfo[GameManager.instance.CurrentPos].MoveCheck ||
                        MiniMapManager.instance.SlotDic[GameManager.instance.CurrentPos + new Vector2Int(1, 0)].show)
                    {
                        GameManager.instance.CurrentPos += new Vector2Int(1, 0);
                        transform.position += Vector3.right * 4f;
                    }else
                    {
                        transform.position -= Vector3.right * 4f;
                        return;
                    }
                    
                
                }else if (moveDirection.x < 0)
                {
                    Debug.Log("서쪽");
                    if (MapGenerator.Instance.tileMapInfo[GameManager.instance.CurrentPos].MoveCheck || 
                        MiniMapManager.instance.SlotDic[GameManager.instance.CurrentPos + new Vector2Int(-1, 0)].show)
                    {
                        GameManager.instance.CurrentPos += new Vector2Int(-1, 0);
                        transform.position += Vector3.left * 4f;
                    }else
                    {
                        transform.position -= Vector3.left * 4f;
                        return;
                    }
                }else if (moveDirection.y >0)
                {
                    Debug.Log("북쪽");
                    if (MapGenerator.Instance.tileMapInfo[GameManager.instance.CurrentPos].MoveCheck ||
                        MiniMapManager.instance.SlotDic[GameManager.instance.CurrentPos + new Vector2Int(0, 1)].show)
                    {
                        GameManager.instance.CurrentPos += new Vector2Int(0, 1);
                        transform.position += Vector3.up * 4f;
                    }
                    else
                    {
                        transform.position -= Vector3.up * 4f;
                        return;
                    }
                

                }else if (moveDirection.y < 0)
                {
                    Debug.Log("남쪽");
                    if (MapGenerator.Instance.tileMapInfo[GameManager.instance.CurrentPos].MoveCheck ||
                        MiniMapManager.instance.SlotDic[GameManager.instance.CurrentPos + new Vector2Int(0, -1)].show)
                    {
                        GameManager.instance.CurrentPos += new Vector2Int(0, -1);
                        transform.position += Vector3.down * 4f;
                    }
                    else
                    {
                        transform.position -= Vector3.down * 4f;
                        return;
                    }
                        
                   

                }

                CameraSetting.instance.moveCoroutine = StartCoroutine(CameraSetting.instance.SmoothMoveCoroutine(GameManager.instance.CurrentPos, 0.2f));
                MiniMapManager.instance.SlotShow(GameManager.instance.CurrentPos);

            

            }
        }


    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (GameProsessManager.instance.prosessType == GameProsessManager.ProsessType.Battle)
            return;

        if (other.CompareTag("Interaction"))
        {
            InteractionObject interaction = other.gameObject.GetComponent<InteractionObject>();
            interaction.PlayerColiderExit();
            targetinteraction = null;
        }

    }


    public override void UnitDie()
    {
        base.UnitDie();
        GameManager.instance.RemovePlayer();
    }

    public void OnDestroy()
    {
        if(hpbar != null)
            Destroy(hpbar.gameObject);
    }







}
