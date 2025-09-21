using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static TMPro.SpriteAssetUtilities.TexturePacker_JsonArray;

public class MedalDesc : MonoBehaviour
{

    [SerializeField]
    private Image Icon;
    [SerializeField]
    private TextMeshProUGUI Desc;

    [SerializeField]
    private TextMeshProUGUI Name;

    [SerializeField]
    private TextMeshProUGUI targetTag;

    [SerializeField]
    private Button EnableButton;



    private void Start()
    {
        DescClear();
    }


    /// <summary>
    /// 메달은 OnPointer가 필요없습니다.
    /// </summary>
    /// <param name="medal"></대상 메달 정보>
    /// <param name="tag"></대상 메달 테크>

    public void DescSet(MedalScriptableObejct medal ,int tag)
    {
        Icon.sprite = medal.Sprite;
        Desc.text = medal.Description;
        Name.text = medal.Name;

        if(tag == 0)
        {
            targetTag.text = "플레이어";
        }else
        {
            targetTag.text = "몬스터";
        }
        Icon.gameObject.SetActive(true);
        EnableButton.interactable = true;
    }

    public void DescClear()
    {
        Icon.sprite = null;
        Desc.text = "";
        Name.text = "";
        targetTag.text = "";
        Icon.gameObject.SetActive(false);
        EnableButton.interactable = false;
    }

}
