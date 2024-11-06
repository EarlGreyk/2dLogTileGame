using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TextSet : MonoBehaviour
{
    [SerializeField]
    private List<TextMeshProUGUI> targetTextList = new List<TextMeshProUGUI>();

    public List<TextMeshProUGUI> TargetTextList { get { return targetTextList; } }





    public void targetTextSet(string Text)
    {
        targetTextList[0].text = Text;
    }
    public void targetTextSet(List<string> TextList)
    {
        for(int i =0;  i < targetTextList.Count; i++)
        {
            targetTextList[i].text = TextList[i];
        }
    }
}
