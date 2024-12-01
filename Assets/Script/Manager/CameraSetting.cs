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
                // ���콺 ��ġ�� ���� ��ǥ�� ��ȯ
                Vector3 mouseWorldPosBeforeZoom = camera.ScreenToWorldPoint(Input.mousePosition);

                // ī�޶��� orthographicSize ����
                camera.orthographicSize -= scrollData * zoomSpeed;

                // orthographicSize �� ����
                camera.orthographicSize = Mathf.Clamp(camera.orthographicSize, minZoom, maxZoom);

                // ���콺 ��ġ�� �ٽ� ���� ��ǥ�� ��ȯ
                Vector3 mouseWorldPosAfterZoom = camera.ScreenToWorldPoint(Input.mousePosition);

                // ī�޶� ��ġ ���� (�� �Ŀ��� ���콺 ��ġ�� ������ ���� ��ǥ�� ����Ű����)
                Vector3 cameraPositionDelta = mouseWorldPosBeforeZoom - mouseWorldPosAfterZoom;
                camera.transform.position += cameraPositionDelta * 1.5f;
            }
            // W Ű�� ������ ��, ī�޶� ������ �̵�
            if (Input.GetKey(KeyCode.W))
            {
                move += Vector3.up;
            }

            // S Ű�� ������ ��, ī�޶� �Ʒ���(Y��)���� �̵�
            if (Input.GetKey(KeyCode.S))
            {
                move += Vector3.down;
            }

            // A Ű�� ������ ��, ī�޶� �������� �̵�
            if (Input.GetKey(KeyCode.A))
            {
                move -= transform.right;
            }

            // D Ű�� ������ ��, ī�޶� ���������� �̵�
            if (Input.GetKey(KeyCode.D))
            {
                move += transform.right;
            }


            // ī�޶� ��ġ�� �̵���Ű�� ���� �ӵ��� ��Ÿ Ÿ���� ����
            camera.transform.position += move * moveSpeed * Time.deltaTime;


        }

    }

    public void unitFocusSet(Vector3 unitPos)
    {
        camera.transform.position = unitPos+new Vector3(0, 0, -1);
        camera.orthographicSize = 10;
    }


    /// <summary>
    /// ȭ���� ���ߴ� ����� ��ȯ�մϴ�.
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

            // Ÿ�ϸ��� �߽� ��ǥ�� ����մϴ�.
            Vector3Int centerCellPosition = new Vector3Int(
                bounds.xMin + bounds.size.x / 2,
                bounds.yMin + bounds.size.y / 2,
                bounds.zMin + bounds.size.z / 2);

            // �߽� Ÿ���� ���� ��ǥ�� ���մϴ�.
            Vector3 centerWorldPosition = GameManager.instance.BattleZone.Tilemap.CellToWorld(centerCellPosition);

            Debug.Log("Ÿ�ϸ��� ���߾� ���� ��ǥ: " + centerWorldPosition);
            camera.transform.position = centerWorldPosition + new Vector3(0,0,-1);
            camera.orthographicSize = 25;
            blockModeOn();
        }

    }





    /// ��� ��带 Ȱ��ȭ�մϴ�.
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
