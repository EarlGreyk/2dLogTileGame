using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Character : MonoBehaviour
{
    
    public  PlayerScriptableObject PlayerData;
    public Image image;



    private void Start()
    {
        image = GetComponent<Image>();

        if (PlayerData == null)
        {
            //플레이어 캐릭터 데이터가 없다면 리턴.
            return;
        }
        image.sprite = PlayerData.PlayerSprite;

    }
}
