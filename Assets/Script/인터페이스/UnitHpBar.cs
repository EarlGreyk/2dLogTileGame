using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static System.Collections.Specialized.BitVector32;
/// <summary>
/// ������ ui�� �����մϴ�. 
/// ĸ��ȭ�� ���� ���Ѿ��մϴ�.
/// </summary>
public class UnitHpBar : MonoBehaviour
{

    public Unit monster;
    public GameObject action;
    public GameObject canvas;
    public Image fillImage;
    public TextMeshProUGUI hptext;
    public TextSet actiontext;



    public RectTransform rectHpbar;
    public RectTransform rectAction;
    public RectTransform canvasRectTransform;

    public float hpbarHight = 1.5f;
    public float actionHight = 1.5f;

    private float widthRatio;
    private float heightRatio;
    private bool isOnScreen = false;
    public bool IsOnScreent { get { return isOnScreen; } set { isOnScreen = value; } }
    
    // Start is called before the first frame update
    
  

    // Update is called once per frame
   

    public void HpbarSet(Unit unit)
    {

        canvas = GameManager.instance.HPCanvas;
        canvasRectTransform = canvas.GetComponent<RectTransform>();
        rectHpbar = gameObject.GetComponent<RectTransform>();
       
        if (action != null)
        {
            GameObject obj = Instantiate(action, canvas.transform);
            rectAction = obj.GetComponent<RectTransform>();
            actiontext = obj.GetComponent<TextSet>();


        }
            

        // ���� ȭ�� ������ ���� �������
        // 0. �������ϱ�
        // ���� �ػ�
        float screenWidth = Screen.width;
        float screenHeight = Screen.height;

        // smallCanvas�� ũ��
        Vector2 canvasSize = canvasRectTransform.rect.size;
        float canvasWidth = canvasSize.x;
        float canvasHeight = canvasSize.y;

        // ���� ���
        widthRatio = canvasWidth / screenWidth;
        heightRatio = canvasHeight / screenHeight;
        widthRatio = ((1 - widthRatio) / 2);
        heightRatio = ((1 - heightRatio) / 2) + 0.04f;
        monster = unit;
        HpTextSet();
    }


    public void UpdateHpbarPosition()
    {
        Vector3 worldPos = new Vector3(monster.transform.position.x, monster.transform.position.y - hpbarHight, monster.transform.position.z);
        rectHpbar.position = worldPos;
        float scaleMultiplier = 0.5f / Camera.main.orthographicSize;
        rectHpbar.localScale = Vector3.one * (scaleMultiplier/2);        
    }

    public void UpdateIconPosition()
    {


        // 1. ������ ���� ��ġ�� ȭ�� ��ǥ�� ��ȯ
        Vector3 worldPosition = monster.transform.position;
        Vector3 viewportPoint = Camera.main.WorldToViewportPoint(worldPosition);

        float clampedX = Mathf.Clamp(viewportPoint.x, 0 + widthRatio, 1 - widthRatio);
        float clampedY = Mathf.Clamp(viewportPoint.y, 0 + heightRatio, 1 - heightRatio);

        // 3. Ŭ���ε� ����Ʈ ��ǥ�� ȭ�� ��ǥ�� ��ȯ
        Vector3 clampedScreenPoint = new Vector3(
            clampedX * Screen.width,
            clampedY * Screen.height,
            viewportPoint.z
        );

        // 4. ȭ�� ��ǥ�� ���� ĵ������ ���� ��ǥ�� ��ȯ
        Vector2 localPoint;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRectTransform, clampedScreenPoint, Camera.main, out localPoint);
        Vector2 canvasSize = canvasRectTransform.rect.size;
        Vector2 clampedPosition = new Vector2(
            Mathf.Clamp(localPoint.x, -canvasSize.x, canvasSize.x),
            Mathf.Clamp(localPoint.y, -canvasSize.y, canvasSize.y)
        );



    }


    public void UpdateActionPosition()
    {
        Vector3 worldPos = new Vector3(monster.transform.position.x, monster.transform.position.y + actionHight, monster.transform.position.z);
        rectAction.position = worldPos;
        float scaleMultiplier = 0.5f / Camera.main.orthographicSize;
        rectAction.localScale = Vector3.one * (scaleMultiplier/2);
    }
    public void HpTextSet()
    {
        if(monster.status != null)
        {
            hptext.text = monster.status.Health.ToString() + " / " + monster.status.MaxHealth.ToString();
            fillImage.fillAmount = monster.status.Health / monster.status.MaxHealth;
        }
        
    }
    public void ActionSet(List<string> text)
    {
        actiontext.targetTextSet(text);
    }
    private void OnDestroy()
    {
        if(rectAction != null)
            Destroy(rectAction.gameObject);
    }

}
