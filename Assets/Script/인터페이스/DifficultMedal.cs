using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DifficultMedal: MonoBehaviour
{
    [SerializeField]
    public string checkValue;

    [SerializeField]
    private Image Image;

    public MedalScriptableObejct medalData;




    private void Start()
    {
        Image = GetComponent<Image>();
    }

    public void MedalSet(MedalScriptableObejct medal)
    {
        medalData = medal;
        Image.sprite = medal.Sprite;
    }
    public void MedalOff()
    {
        medalData = null;
        Image.sprite = null;
    }



    
}
