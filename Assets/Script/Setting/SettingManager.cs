using System.Collections;
using System.Collections.Generic;
using Unity.PlasticSCM.Editor.WebApi;
using UnityEngine;
using UnityEngine.UI;


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

    [SerializeField]
    private Image PlayerImage;


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

    /// <summary>
    /// 캐릭터를 선택하면 플레이어가 어떠한 마법을 들고갈 수 있고 처음에 어떤 마법을 주는지를 부여합니다.
    /// </summary>
    /// <param name="character"></param>
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
        else
        {
            PlayerImage.sprite = character.PlayerData.PlayerSprite;
            if (currentCharacter == null)
            {
                currentCharacter = character;
                SettingData.character = currentCharacter;
            }
            else if (currentCharacter != character)
            {
                currentCharacter = character;
                SettingData.character = currentCharacter;
            }

            for (int i = 0; i < currentCharacter.PlayerData.UsingMagics.Length; i++)
            {
                if (currentCharacter.PlayerData.UsingMagics.Length <= i)
                    return;

                PlaymagicUIs[i].gameObject.SetActive(true);

                PlaymagicUIs[i].MagicSet(currentCharacter.PlayerData.ListMagics[i]);
            }


            for (int i = 0; i < currentCharacter.PlayerData.ListMagics.Length; i++)
            {
                if (currentCharacter.PlayerData.ListMagics.Length <= i)
                    return;

                ListmagicUIs[i].gameObject.SetActive(true);
                ListmagicUIs[i].MagicSet(currentCharacter.PlayerData.ListMagics[i]);
            }
        }
            

        
            
    }

   
}
