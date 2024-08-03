using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingManager : MonoBehaviour
{
    private int difficult;

    [SerializeField]
    SlateManager slateManager;

    public void setDifficult(int i) 
    {
        difficult = i;
        PopUpManager.instance.LastClosePopUp();
        SettingData.difficult = i;
    }

    public void setSlate()
    {

    }
   
}
