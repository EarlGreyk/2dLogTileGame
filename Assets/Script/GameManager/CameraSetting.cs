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
