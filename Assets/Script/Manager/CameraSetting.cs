using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;

public class CameraSetting : MonoBehaviour
{
    [SerializeField]
    private Grid grid;
    public static CameraSetting instance;
    private Camera camera;
    public float zoomSpeed;
    public float minZoom;
    public float maxZoom;

    public float moveSpeed;

    private bool mapMode = false;

    private Vector3 resetPos;

    [SerializeField]
    private RectTransform hpcanvasRect;

    private void Awake()
    {
        if (instance == null)
            instance = this;
    }

    private void Start()
    {
        camera = GetComponent<Camera>();
        hpcanvasRect.position = new Vector2(Screen.width / 2 , Screen.height / 2 + (Screen.height /20));
        hpcanvasRect.sizeDelta = new Vector2(Screen.width, Screen.height/2 + 140);

    }



    public void Update()
    {
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

    public void unitFocusSet(Vector3 unitPos)
    {
        camera.transform.position = unitPos+new Vector3(0, 0, -1);
        camera.orthographicSize = 10;
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
}
