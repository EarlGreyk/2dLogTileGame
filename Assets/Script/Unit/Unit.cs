using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitStatus
{
    public int Health { get; set; }
    public int Damage { get; set; }
    public int ElementalDamage { get; set; } // �Ӽ� ������
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
        Debug.Log($"������ ���� : {Defense}");

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
        // ������ ���� ��ġ�� UI ĵ������ ���� ��ǥ�� ��ȯ
        Vector2 screenPoint = RectTransformUtility.WorldToScreenPoint(Camera.main, transform.position);

        // ĵ������ RectTransform�� ������� ȭ�� ��ǥ�� ���� ��ǥ�� ��ȯ
        Vector2 localPoint;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRectTransform, screenPoint, null, out localPoint);

        // RectTransform�� ũ��� ���Ͽ� ������ ĵ���� ���� �ִ��� Ȯ��
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
       

        // 1. ������ ���� ��ġ�� ȭ�� ��ǥ�� ��ȯ
        Vector3 worldPosition = transform.position;
        Vector3 viewportPoint = Camera.main.WorldToViewportPoint(worldPosition);

        float clampedX = Mathf.Clamp(viewportPoint.x, 0+ widthRatio, 1- widthRatio);
        float clampedY = Mathf.Clamp(viewportPoint.y, 0+ heightRatio, 1 - heightRatio);

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

        // 6. �������� ���� ��ġ ������Ʈ
        rectIcon.localPosition = new Vector3(clampedPosition.x, clampedPosition.y, 0);


    }

    public void ApplyNodeEffect(LuneNode node)
    {
        node.ApplyEffect(this);
    }
}


