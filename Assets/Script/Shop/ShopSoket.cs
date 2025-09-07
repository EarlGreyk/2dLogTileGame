using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;


/// <summary>
/// 상점이 판매하는 품목을 보여줍니다.
/// 석판/ 마법 / 블록 3가지를 판매합니다.
/// 이중 판매 품목은 랜덤입니다.
/// </summary>
public class ShopSoket: MonoBehaviour
{
    // Start is called before the first frame update
    public BaseScriptableObject shopItem;
    


    //판매가격
    public TextMeshProUGUI sellValue;

    //판매 설명
    public TextMeshProUGUI sellDesc;
    //판매 아이콘
    public Image sellIcon;


    //판매했는지의 여부
    private Button sellButton; 

    private void Start()
    {
        sellButton = GetComponent<Button>();
        Set();
        if(shopItem == null)
        {
            sellButton.interactable = false;
        }else
        {
            sellButton.interactable = true;
        }
    }



    public void Set()
    {
        
        int R = Random.Range(0, 100);


        if(R<=50)
        {
            MagicScriptableObejct[] data = Resources.LoadAll<MagicScriptableObejct>("ScriptableObjects/magic_data");
            //판매 :마법
            int i = Random.Range(0,data.Length);

            shopItem = data[i];
            data[i].ApplyToUI(this);

        }
        else if(R<=85)
        {
            //판매 : 블록
            BlockScriptableObject[] data = Resources.LoadAll<BlockScriptableObject>("ScriptableObjects/block_data");
            //판매 :마법
            int i = Random.Range(0, data.Length);

            shopItem = data[i];
            data[i].ApplyToUI(this);
        }
        else
        {
            //판매 : 석판
            SlateScriptableObejct[] data = Resources.LoadAll<SlateScriptableObejct>("ScriptableObjects/slate_data");
            int i = Random.Range(0, data.Length);

            shopItem = data[i];
            data[i].ApplyToUI(this);
        }


        

    }
   
    /// <summary>
    /// 플레이어의 아이템 구매 를 담당합니다. 
    /// Button을 통해 인식하여 사용됩니다.
    /// </summary>
    public void Sell()
    {
        if (shopItem is IShopItem sellable)
        {
            sellable.Sell();

            // 판매된 상품의 이미지. Null값을 베이스로 하는 이미지를 구해서 대체하는것이 좋아보임.
            sellIcon.sprite = null;
            sellDesc.text = "";
            sellValue.text = "";
            shopItem = null;
        }
        else
        {
            Debug.Log("아이템 판매 불가.");
        }

    }


}
