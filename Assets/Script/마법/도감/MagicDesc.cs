using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
/// <summary>
/// 도감에서 마법의 상세 설명을 담당합니다.
/// </summary>
public class MagicDesc : MonoBehaviour
{
    [SerializeField]
    private Image descImage;
    public Image DescImage { get { return descImage; } set { descImage = value; } }

    [SerializeField]
    private TextMeshProUGUI descText;

    public TextMeshProUGUI DescText { get { return descText; } set { descText = value; } }


    [SerializeField]
    private TextMeshProUGUI nameText;


    private void OnEnable()
    {
        descImage.sprite = null;
        descText.text = null;
        nameText.text = null;

    }
    //도감용
    public void DescSet(MagicScriptableObejct data,RectTransform rectTransform)
    {
        if (data == null)
        {
            return;
        }
        gameObject.SetActive(true);
        descImage.sprite = data.MagicSprite;
        descText.text = data.MagicDesc;
        nameText.text = data.MagicName;
        Vector2 posSet = new(200f + rectTransform.position.x, 100f + rectTransform.position.y);
        gameObject.transform.position = posSet;

    }

    //UI설정용
    public void DescSet(MagicUI UI)
    {
        if (UI== null)
        {
            return;
        }
        gameObject.SetActive(true);    
        descImage.sprite = UI.Magic.MagicSprite;
        descText.text = UI.Magic.MagicDesc;
        nameText.text = UI.Magic.MagicName;

    }

    //가공된 마법 보여주기용
    public void DescSet(MagicOrigin magic, int level)
    {
        if (magic == null)
            return;
        if (level == 4)
        {
            gameObject.SetActive(false);
            return;
        }
        descImage.sprite = magic.MagicSprite;
        descText.text = magic.MagicDesc;

    }
    public void DescClear()
    {
        descImage.sprite = null;
        descText.text = "";
        nameText.text = "";
        gameObject.SetActive(false);
    }
}
