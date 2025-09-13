using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Character : MonoBehaviour
{
    
    public  PlayerScriptableObject PlayerData;
    public Image icon;
    


    private Button selectButton;


    private void Start()
    {
        selectButton = GetComponent<Button>();

        if (PlayerData == null)
        {
            //플레이어 캐릭터 데이터가 없다면 리턴.
            selectButton.interactable = false;
            return;
        }
        icon.gameObject.SetActive(true);
        icon.sprite = PlayerData.PlayerSprite;

        

    }
}
