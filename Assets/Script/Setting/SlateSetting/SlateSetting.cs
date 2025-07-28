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

    private List<SlateScriptableObejct> slateList = new List<SlateScriptableObejct>();

    private List<SlateScriptableObejct> enableslateList = new List<SlateScriptableObejct>();

    private SlateUI curentSlateUI;

    public SlateUI CurrentSlateUI { get { return curentSlateUI; } }

    public void SlateUiSet(SlateUI slateUI)
    {
        curentSlateUI = slateUI;
    }
    
}
