using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MagicDesc : MonoBehaviour
{
    [SerializeField]
    private Image descImage;
    public Image DescImage { get { return descImage; } set { descImage = value; } }

    [SerializeField]
    private TextMeshProUGUI descText;

    public TextMeshProUGUI DescText { get { return descText; } set { descText = value; } }





    public void DescSet(MagicUI UI)
    {
        if (UI.Magic == null)
            return;
        descImage.sprite = UI.Magic.IconSprite;
        descText.text = UI.Magic.MagicDesc;

    }
}
