using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// SettingScean에서 플레이어가 설정할것을 관리합니다.
/// 이후 최종적으로 SettingData에 다가 정보값을 static으로 옮겨 사용합니다.
/// </summary>

public class SettingManager : MonoBehaviour
{
    private int difficult;

    public void setDifficult(int i) 
    {
        difficult = i;
        PopUpManager.instance.LastClosePopUp();
        SettingData.difficult = i;
    }

    public void MagicDataSet(MagicScriptableObejct magicData)
    {
        SettingData.magicList.Add(magicData);
    }

   
}
