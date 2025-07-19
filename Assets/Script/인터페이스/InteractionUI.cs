using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class InteractionUI : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI textMesh;

    private Vector2 offset = new Vector2(0, 50f);

    private RectTransform rectTransform;

    private Transform targetObj;

    [SerializeField]
    private Canvas canvas;


    private void Start()
    {
        rectTransform = GetComponent<RectTransform>();
    }


    private void Update()
    {
        if (!gameObject.activeSelf)
            return;




        if(targetObj != null)
        {
           
            // 1. 월드 → 스크린 좌표
            Vector3 screenPos = Camera.main.WorldToScreenPoint(targetObj.position);

            // 2. 오프셋 추가 (화면 상에서 위로 띄우기)
            screenPos += (Vector3)offset;

            // 3. 스크린 → Canvas 로컬 좌표
            RectTransform canvasRect = canvas.GetComponent<RectTransform>();
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                canvasRect, screenPos, canvas.renderMode == RenderMode.ScreenSpaceOverlay ? null : canvas.worldCamera, out Vector2 localPos);

            // 4. UI 위치 갱신

            rectTransform.localPosition = localPos;



        }




    }

    /// <summary>
    /// 1. 정화유닛 상호작용
    /// </summary>
    /// <param name="value"></타입을 정수로 받아 비교합니다.>

    public void Set(bool activeSelf, string text)
    {

        textMesh.text = text;

        if (!activeSelf)
        {
            gameObject.SetActive(false);
            targetObj = null;
        }
        else
        {
            gameObject.SetActive(true);
            targetObj = GameManager.instance.StayPlayerUnit.transform;
        }



        
        
    }
}
