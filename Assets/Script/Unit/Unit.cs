using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitStatus
{
    public int Health { get; set; }
    public int Damage { get; set; }
    public int ElementalDamage { get; set; } // 속성 데미지
    public int Defense { get; set; }
    public int ItemChan { get; set; }
    public int BlockChan { get; set; }
    public int MagicChian { get; set; }
    public int MagicCount { get; set; }

    public UnitStatus()
    {
        Health = 0;
        Damage = 0;
        ElementalDamage = 0;
        Defense = 0;
        ItemChan = 0;
        BlockChan = 0; 
        MagicChian = 0;
        MagicCount = 0;
    }

    public void effectUp(string effectString, int effectValue)
    {
        switch(effectString)
        {
            case "Health": Health += effectValue;
                break;
            case "Damage": Damage += effectValue;
                break;
            case "ElementalDamage": ElementalDamage += effectValue;
                break;
            case "Defence":Defense += effectValue;
                break;
            case "ItemChan":ItemChan += effectValue;
                break;
            case "BlockChan": BlockChan += effectValue;
                break;
            case "MagicChain": MagicChian += effectValue;
                break;
            case "MagicCount": MagicCount += effectValue;
                break;

        }
        Debug.Log($"룬증가 방어력 : {Defense}");

    }
    public void effectCopy(UnitStatus copyTemp)
    {
        Health = copyTemp.Health;
        Damage = copyTemp.Damage;
        ElementalDamage = copyTemp.ElementalDamage;
        Defense = copyTemp.Defense;
        ItemChan = copyTemp.ItemChan;
        BlockChan = copyTemp.BlockChan;
        MagicChian = copyTemp.MagicChian;
        MagicCount = copyTemp.MagicCount;
    }

}

public class Unit :MonoBehaviour
{

    public UnitStatus status;
    public GameObject hpbar;
    public GameObject Icon;
    public GameObject canvas;

    public RectTransform rectHpbar;
    public RectTransform rectIcon;
    private RectTransform canvasRectTransform;

    public float hpbarHight = 1.5f;

    private float widthRatio;
    private float heightRatio;

    public virtual void Start()
    {
        canvas = GameManager.instance.HPCanvas;
        GameManager.instance.BattleZone.setTileUnit(transform.position, this);
        rectHpbar = Instantiate(hpbar,canvas.transform).GetComponent<RectTransform>();
        rectIcon = Instantiate(Icon, canvas.transform).GetComponent<RectTransform>();
        canvasRectTransform = canvas.GetComponent<RectTransform>();
        // 향후 화면 비율을 위한 계산진행
        // 0. 비율구하기
        // 현재 해상도
        float screenWidth = Screen.width;
        float screenHeight = Screen.height;

        // smallCanvas의 크기
        Vector2 canvasSize = canvasRectTransform.rect.size;
        float canvasWidth = canvasSize.x;
        float canvasHeight = canvasSize.y;

        // 비율 계산
        widthRatio = canvasWidth / screenWidth;
        heightRatio = canvasHeight / screenHeight;
        widthRatio = ((1 - widthRatio) / 2);
        heightRatio = ((1 - heightRatio) / 2)+0.04f;

    }
    public virtual void Update()
    {
        bool isOnScreen = CheckVisibility();

        if (isOnScreen)
        {
            UpdateHpbarPosition();
            rectIcon.gameObject.SetActive(false);
            rectHpbar.gameObject.SetActive(true);
        }
        else
        {
            UpdateIconPosition();
            rectHpbar.gameObject.SetActive(false);
            rectIcon.gameObject.SetActive(true);
        }




    }
    private bool CheckVisibility()
    {
        // 유닛의 월드 위치를 UI 캔버스의 로컬 좌표로 변환
        Vector2 screenPoint = RectTransformUtility.WorldToScreenPoint(Camera.main, transform.position);

        // 캔버스의 RectTransform을 기반으로 화면 좌표를 로컬 좌표로 변환
        Vector2 localPoint;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRectTransform, screenPoint, null, out localPoint);

        // RectTransform의 크기와 비교하여 유닛이 캔버스 내에 있는지 확인
        return canvasRectTransform.rect.Contains(localPoint);
    }

    private void UpdateHpbarPosition()
    {
        Vector3 worldPos = new Vector3(transform.position.x, transform.position.y + hpbarHight, transform.position.z);
        rectHpbar.position = worldPos;
        float scaleMultiplier = 0.5f / Camera.main.orthographicSize;
        rectHpbar.localScale = Vector3.one * scaleMultiplier;
    }

    private void UpdateIconPosition()
    {
       

        // 1. 유닛의 월드 위치를 화면 좌표로 변환
        Vector3 worldPosition = transform.position;
        Vector3 viewportPoint = Camera.main.WorldToViewportPoint(worldPosition);

        float clampedX = Mathf.Clamp(viewportPoint.x, 0+ widthRatio, 1- widthRatio);
        float clampedY = Mathf.Clamp(viewportPoint.y, 0+ heightRatio, 1 - heightRatio);

        // 3. 클램핑된 뷰포트 좌표를 화면 좌표로 변환
        Vector3 clampedScreenPoint = new Vector3(
            clampedX * Screen.width,
            clampedY * Screen.height,
            viewportPoint.z
        );

        // 4. 화면 좌표를 작은 캔버스의 로컬 좌표로 변환
        Vector2 localPoint;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRectTransform, clampedScreenPoint, Camera.main, out localPoint);
        Vector2 canvasSize = canvasRectTransform.rect.size;
        Vector2 clampedPosition = new Vector2(
            Mathf.Clamp(localPoint.x, -canvasSize.x, canvasSize.x),
            Mathf.Clamp(localPoint.y, -canvasSize.y, canvasSize.y)
        );

        // 6. 아이콘의 로컬 위치 업데이트
        rectIcon.localPosition = new Vector3(clampedPosition.x, clampedPosition.y, 0);


    }

    public void ApplyNodeEffect(LuneNode node)
    {
        node.ApplyEffect(this);
    }
}


