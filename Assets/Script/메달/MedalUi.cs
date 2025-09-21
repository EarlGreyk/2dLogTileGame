using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/// <summary>
/// 메달
/// 플레이어에게 악영양을 주거나 좋은 효과를 줍니다.
/// 난이도 설정에서 게임 시작전에 설정 합니다.
/// MedalManager에서 관리합니다.
/// </summary>
public class MedalUi: MonoBehaviour
{
    [SerializeField]
    public string checkValue;

    [SerializeField]
    private Image Image;

    public MedalScriptableObejct medalData;

    private MedalDesc targetDesc;

    private void Start()
    {
        if (medalData == null)
            Image.gameObject.SetActive(false);
    }


    public void MedalSet(MedalScriptableObejct medal)
    {
        medalData = medal;
        Image.sprite = medal.Sprite;
        Image.gameObject.SetActive(true);
    }
    public void MedalOff()
    {
        medalData = null;
        Image.sprite = null;
        Image.gameObject.SetActive(false);
    }

    public void EventInit(MedalDesc desc)
    {
        targetDesc = desc;
    }

 




}
