using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlateSetting : MonoBehaviour
{
    [SerializeField]
    private SlateUI firstSlate;
    [SerializeField]
    private SlateUI secondSlate;
    [SerializeField]
    private SlateUI thirdSlate;
    [SerializeField]
    private SlateUI fourthSlate;

    private List<Slate> slateList = new List<Slate>();

    private List<Slate> enableslateList = new List<Slate>();

    private SlateUI curentSlateUI;

    public SlateUI CurrentSlateUI { get { return curentSlateUI; } }

    public void SlateUiSet(SlateUI slateUI)
    {
        curentSlateUI = slateUI;
    }
    public void SlateSet(SlateUI slateUI)
    {
     
        if(curentSlateUI != null)
        {
            curentSlateUI.SlateSet(slateUI.Slate);

            if(curentSlateUI == firstSlate)
            {
                SettingData.firstSlate = slateUI.Slate;
            }
            if (curentSlateUI == secondSlate)
            {
                SettingData.secondSlate = slateUI.Slate;
            }
            if (curentSlateUI == thirdSlate)
            {
                SettingData.thirdSlate = slateUI.Slate;
            }
            if (curentSlateUI == fourthSlate)
            {
                SettingData.fourthSlate = slateUI.Slate;
            }

        }
    }
}
