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
            Debug.Log($"{gameObject.name}의 플레이어 데이터가 존재하지 않음");
            return;
        }
        image.sprite = PlayerData.PlayerSprite;

    }
}
