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





    public void DescSet(MagicUI UI)
    {
        if (UI.Magic == null)
        {
            Debug.Log("설정할 마법이 없음.");
            return;
        }
            
        descImage.sprite = UI.Magic.IconSprite;
        descText.text = UI.Magic.MagicDesc;
        nameText.text = UI.Magic.name;

    }

    public void DescSet(Magic magic, int level)
    {
        if (magic == null)
            return;
        descImage.sprite = magic.IconSprite;
        descText.text = magic.MagicDesc;

    }
}
