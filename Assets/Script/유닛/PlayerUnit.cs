using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerUnit : Unit
{
    public bool ColiderCheck = false;

 
    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();

        status.effectAdd(SettingData.LuneStatus);
 
    }

    public override void Update()
    {
        base.Update();
        //유닛 기본이동구현
        if (   Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow)    )
        {
            transform.position += Vector3.up*0.02f;
        }

        // S 키를 눌렀을 때, 카메라를 아래쪽(Y축)으로 이동
        if (Input.GetKey(KeyCode.S))
        {
            transform.position += Vector3.down * 0.02f;
        }

        // A 키를 눌렀을 때, 카메라를 왼쪽으로 이동
        if (Input.GetKey(KeyCode.A))
        {
            transform.position += Vector3.left * 0.04f;
        }

        // D 키를 눌렀을 때, 카메라를 오른쪽으로 이동
        if (Input.GetKey(KeyCode.D))
        {
            transform.position += Vector3.right * 0.02f;
        }
    }


    public override void HitDamage(float Damage)
    {
        base.HitDamage(Damage);
    }
    /// <summary>
    /// 지역 이동을 위해 충돌 처리를 하기 위해 만들어진 Ontrigger입니다.
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("접촉");
        
        if (other.CompareTag("MoveTrigger") && ColiderCheck == false)
        {
            if (!GameManager.instance.MapGenerator.spawnedTilemaps.ContainsKey(GameManager.instance.CurrentPos))
            {
                Debug.Log("현재 해당 값은 딕셔너리에 없음");
                Debug.Log($"비교좌표 : {GameManager.instance.CurrentPos} :: 있는여부 {GameManager.instance.MapGenerator.spawnedTilemaps[GameManager.instance.CurrentPos]}");

                return;
            }

            GameObject tilemap = GameManager.instance.MapGenerator.spawnedTilemaps[GameManager.instance.CurrentPos];
            Vector2 comparePos = new Vector2(GameManager.instance.CurrentPos.x*15, GameManager.instance.CurrentPos.y*15) + new Vector2(7.5f, 7.5f);
            float valueX = transform.position.x - 7.5f;
            float valueY = transform.position.y - 7.5f;
            if (tilemap == other.gameObject.transform.parent.gameObject)
            {
                ColiderCheck = true;
                Debug.Log($"동서 비교 {valueX} : {comparePos.x + 4}");
                //서쪽
                if (valueX > comparePos.x - 4)
                {
                    Debug.Log("동쪽");
                    GameManager.instance.CurrentPos += new Vector2Int(1, 0);
                    CameraSetting.instance.moveCoroutine = StartCoroutine(CameraSetting.instance.SmoothMoveCoroutine(GameManager.instance.CurrentPos, 0.5f));
                    return;
                }
                    
                if (valueX < comparePos.x + 4)
                {
                    Debug.Log("서쪽");
                    GameManager.instance.CurrentPos += new Vector2Int(-1, 0);
                    CameraSetting.instance.moveCoroutine = StartCoroutine(CameraSetting.instance.SmoothMoveCoroutine(GameManager.instance.CurrentPos, 0.5f));
                    return;

                }

                if (valueY > comparePos.y + 4)
                {
                    Debug.Log("북쪽");
                    GameManager.instance.CurrentPos += new Vector2Int(0, 1);
                    CameraSetting.instance.moveCoroutine = StartCoroutine(CameraSetting.instance.SmoothMoveCoroutine(GameManager.instance.CurrentPos, 0.5f));
                    return;

                }

                if (valueY < comparePos.y + 4)
                {
                    Debug.Log("남쪽");
                    GameManager.instance.CurrentPos += new Vector2Int(0, -1);
                    CameraSetting.instance.moveCoroutine = StartCoroutine(CameraSetting.instance.SmoothMoveCoroutine(GameManager.instance.CurrentPos, 0.5f));
                    return;

                }


                
            }
        }


    }
}
