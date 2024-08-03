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

    private SlateUI CurentSlateUI;

    public void SlateUiSet(SlateUI slateUI)
    {
        CurentSlateUI = slateUI;
    }
    public void SlateSet(SlateUI slateUI)
    {
     
        if(CurentSlateUI != null)
        {
            CurentSlateUI.SlateSet(slateUI.Slate);

            if(CurentSlateUI == firstSlate)
            {
                SettingData.firstSlate = slateUI.Slate;
            }
            if (CurentSlateUI == secondSlate)
            {
                SettingData.secondSlate = slateUI.Slate;
            }
            if (CurentSlateUI == thirdSlate)
            {
                SettingData.thirdSlate = slateUI.Slate;
            }
            if (CurentSlateUI == fourthSlate)
            {
                SettingData.fourthSlate = slateUI.Slate;
            }

        }
    }
}
