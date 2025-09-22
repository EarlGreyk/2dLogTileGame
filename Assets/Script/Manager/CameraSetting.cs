using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;

public class CameraSetting : MonoBehaviour
{
    public static CameraSetting instance;
    [SerializeField]
    private Grid grid;
    private Camera camera;
    public float zoomSpeed;
    public float minZoom;
    public float maxZoom;

    public float moveSpeed;

    private bool mapMode = false;

    private Vector3 resetPos;

    [SerializeField]
    private RectTransform hpcanvasRect;

    public Coroutine moveCoroutine;

    private Vector2 limitSize;

    private void Awake()
    {
        if (instance == null)
            instance = this;
    }

    private void Start()
    {
        camera = GetComponent<Camera>();
        hpcanvasRect.position = new Vector2(Screen.width / 2 , Screen.height / 2 + (Screen.height /20));
        hpcanvasRect.sizeDelta = new Vector2(Screen.width, Screen.height/2 + 100);
        limitSize = new Vector2(15, 17);

    }



    public void Update()
    {
        
        if(GameProsessManager.instance.prosessType == GameProsessManager.ProsessType.Stay)
        {



        }
        else
        {
            ///아래의 행동은 플레이어가 전투에 돌입할때 전환되어야 하는 카메라 무빙워크입니다.
            if (Input.GetKey(KeyCode.Space))
            {
                changeMode();
            }
            if (!mapMode)
            {
                float scrollData = Input.GetAxis("Mouse ScrollWheel");
                Vector3 move = Vector3.zero;

                if (scrollData != 0.0f)
                {
                    // 마우스 위치를 월드 좌표로 변환
                    Vector3 mouseWorldPosBeforeZoom = camera.ScreenToWorldPoint(Input.mousePosition);

                    // 카메라의 orthographicSize 조정
                    camera.orthographicSize -= scrollData * zoomSpeed;

                    // orthographicSize 값 제한
                    camera.orthographicSize = Mathf.Clamp(camera.orthographicSize, minZoom, maxZoom);

                    // 마우스 위치를 다시 월드 좌표로 변환
                    Vector3 mouseWorldPosAfterZoom = camera.ScreenToWorldPoint(Input.mousePosition);

                    // 카메라 위치 보정 (줌 후에도 마우스 위치가 동일한 월드 좌표를 가리키도록)
                    Vector3 cameraPositionDelta = mouseWorldPosBeforeZoom - mouseWorldPosAfterZoom;
                    camera.transform.position += cameraPositionDelta * 1.5f;
                }
                // W 키를 눌렀을 때, 카메라를 앞으로 이동
                if (Input.GetKey(KeyCode.W))
                {
                    move += Vector3.up;
                }

                // S 키를 눌렀을 때, 카메라를 아래쪽(Y축)으로 이동
                if (Input.GetKey(KeyCode.S))
                {
                    move += Vector3.down;
                }

                // A 키를 눌렀을 때, 카메라를 왼쪽으로 이동
                if (Input.GetKey(KeyCode.A))
                {
                    move -= transform.right;
                }

                // D 키를 눌렀을 때, 카메라를 오른쪽으로 이동
                if (Input.GetKey(KeyCode.D))
                {
                    move += transform.right;
                }


                // 카메라 위치를 이동시키기 위해 속도와 델타 타임을 곱함
                camera.transform.position += move * moveSpeed * Time.deltaTime;


            }
        }

       

    }


    private void LateUpdate()
    {
        if (GameProsessManager.instance.prosessType == GameProsessManager.ProsessType.Stay)
        {

            if (moveCoroutine == null)
                camera.transform.position = transPos(GameManager.instance.CurrentPos);

        }
        
    }

    public void unitorthographicSizeSet(Vector3 unitPos,bool forCus = false)
    {
        if (!forCus)
        {
            if (GameProsessManager.instance.prosessType == GameProsessManager.ProsessType.Stay)
                camera.orthographicSize = 8;
            else if (GameProsessManager.instance.prosessType == GameProsessManager.ProsessType.Battle)
            {
                camera.orthographicSize = 12;
                camera.transform.position = unitPos;
            }
        }else
        {
            camera.transform.position = unitPos + new Vector3(-1, 0, -1);
            camera.orthographicSize = 8;
        }
        
            
       
    }


    /// <summary>
    /// 화면이 비추는 방식을 변환합니다.
    /// </summary>
    public void changeMode()
    {
        mapMode = !mapMode;
        if(!mapMode)
        {
            Vector3 unitPos = GameManager.instance.PlayerUnit.transform.position;
            camera.transform.position = resetPos;
            camera.orthographicSize = 10;
            blockModeOff();
            return;
        }else
        {
            resetPos = Camera.main.transform.position;

            BoundsInt bounds = GameManager.instance.BattleZone.Tilemap.cellBounds;

            // 타일맵의 중심 좌표를 계산합니다.
            Vector3Int centerCellPosition = new Vector3Int(
                bounds.xMin + bounds.size.x / 2,
                bounds.yMin + bounds.size.y / 2,
                bounds.zMin + bounds.size.z / 2);

            // 중심 타일의 월드 좌표를 구합니다.
            Vector3 centerWorldPosition = GameManager.instance.BattleZone.Tilemap.CellToWorld(centerCellPosition);

            Debug.Log("타일맵의 정중앙 월드 좌표: " + centerWorldPosition);
            camera.transform.position = centerWorldPosition + new Vector3(0,0,-1);
            camera.orthographicSize = 25;
            blockModeOn();
        }

    }





    /// 블록 모드를 활성화합니다.
    public void blockModeOn()
    {
        if(mapMode)
            GameManager.instance.BlockModeZone.ModeSetting(true);
    }
    public void blockModeOff()
    {
        GameManager.instance.BlockModeZone.ModeSetting(false);
    }

    /// <summary>
    /// 지역이동간에 카메라가 급작스럽게 이동하는걸 방지하기 위하여 필요한 코루틴입니다.
    /// 발동될 당시의 현재 카메라 위치를 저장하고 이동해야할 카메라 위치까지 이동합니다.
    /// </summary>
    /// <param name="targetPosition"></param>
    /// <param name="duration"></param>
    /// <returns></returns>
    public IEnumerator SmoothMoveCoroutine(Vector2Int Position, float duration)
    {
        float elapsedTime = 0f;
        Vector3 startPosition = transform.position;

        

        while (elapsedTime < duration)
        {
            Vector3 targetPosition = transPos(Position);
            // 이동하는 동안 Lerp를 사용해서 부드럽게 이동
            transform.position = Vector3.Lerp(startPosition, targetPosition, (elapsedTime / duration));
            elapsedTime += Time.deltaTime;
            yield return null;  // 다음 프레임까지 기다림
        }


        moveCoroutine = null;
        GameManager.instance.StayPlayerUnit.ColiderCheck = false;
    }

    /// <summary>
    /// 카메라의 위치를 보정하기 위한 함수입니다.
    /// 지역간 이동, 텔레포트시 사용됩니다.
    /// </summary>
    /// <param name="pos"></해당 벡터를 기점으로 최소와 최대치가 정해집니다.>

    private Vector3 transPos(Vector2 center)
    {
        center = new Vector2(center.x * 30f+15, center.y * 30f+15);


        float limitMinX, limitMaxX;
        float limitMinY, limitMaxY;

        //영역설정.
        limitMinX = center.x - limitSize.x;
        limitMaxX = center.x + limitSize.x;
        limitMinY = center.y - limitSize.y;
        limitMaxY = center.y + limitSize.y;

        Vector2 targetPos = GameManager.instance.StayPlayerUnit.transform.position;

        float halfHeight = camera.orthographicSize;
        float halfWidth = camera.orthographicSize * camera.aspect;

        float clampedX = Mathf.Clamp(targetPos.x, limitMinX + halfWidth, limitMaxX - halfWidth);
        float clampedY = Mathf.Clamp(targetPos.y, limitMinY + halfHeight, limitMaxY - halfHeight);

       

        return new Vector3(clampedX, clampedY,-1);



    }



}
