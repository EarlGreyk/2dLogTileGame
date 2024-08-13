using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSetting : MonoBehaviour
{
    private Camera camera;
    public float zoomSpeed;
    public float minZoom;
    public float maxZoom;

    public float moveSpeed;



    private void Start()
    {
        camera = GetComponent<Camera>();
    }



    public void Update()
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
