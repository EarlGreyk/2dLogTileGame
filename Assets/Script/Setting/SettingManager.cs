using System.Collections;
using System.Collections.Generic;
using Unity.PlasticSCM.Editor.WebApi;
using UnityEngine;


/// <summary>
/// SettingScean에서 플레이어가 설정할것을 관리합니다.
/// 이후 최종적으로 SettingData에 다가 정보값을 static으로 옮겨 사용합니다.
/// </summary>

public class SettingManager : MonoBehaviour
{
    private int difficult;

    private Character currentCharacter;
    
    private int difficulty;
    /// <summary>
    /// 플레이어가 게임 시작시 소유하고 있는 마법 목록
    /// </summary>
    [SerializeField]
    private List<MagicUI> PlaymagicUIs = new List<MagicUI>();


    /// <summary>
    /// 플레이어가 사용할 수 있는 마법 전체목록
    /// </summary>
    [SerializeField]
    private List<MagicUI> ListmagicUIs = new List<MagicUI>();


    private void Start()
    {
        for(int i =0; i< PlaymagicUIs.Count; i++)
        {
            PlaymagicUIs[i].gameObject.SetActive(false);
        }
        for (int i = 0; i < ListmagicUIs.Count; i++)
        {
            ListmagicUIs[i].gameObject.SetActive(false);
        }
    }

    public void setDifficult(int i) 
    {
        difficult = i;
        PopUpManager.instance.LastClosePopUp();
        SettingData.difficult = i;
    }


    public void CharacterSet(Character character)
    {
        if (character.PlayerData == null)
        {
            for (int i = 0; i < PlaymagicUIs.Count; i++)
            {
                PlaymagicUIs[i].gameObject.SetActive(false);
            }
            for (int i = 0; i < ListmagicUIs.Count; i++)
            {
                ListmagicUIs[i].gameObject.SetActive(false);
            }
            return;
        }
            

        if(currentCharacter == null)
        {
           currentCharacter = character;
           SettingData.character = currentCharacter;
        }else if(currentCharacter != character)
        {
           currentCharacter = character;
           SettingData.character = currentCharacter;
        }

        for (int i = 0; i < currentCharacter.PlayerData.UsingMagics.Length; i++)
        {
            if (currentCharacter.PlayerData.UsingMagics.Length <= i)
                return;

            PlaymagicUIs[i].gameObject.SetActive(true);
            PlaymagicUIs[i].Magic = currentCharacter.PlayerData.ListMagics[i];
        }


        for (int i=0;i <currentCharacter.PlayerData.ListMagics.Length; i++)
        {
            if (currentCharacter.PlayerData.ListMagics.Length <= i)
                return;

            ListmagicUIs[i].gameObject.SetActive(true);
            ListmagicUIs[i].Magic = currentCharacter.PlayerData.ListMagics[i];
        }
            
    }

   
}
