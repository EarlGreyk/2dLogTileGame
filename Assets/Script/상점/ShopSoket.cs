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
        if(shopItem == null)
        {
            sellButton.interactable = false;
            sellIcon.gameObject.SetActive(false);
        }else
        {
            sellButton.interactable = true;
            sellIcon.gameObject.SetActive(true);
        }
    }



    public void Set()
    {
        
        int R = Random.Range(0, 100);


        if(R<=50)
        {
            
            List<MagicScriptableObejct> data = ShopManager.Instance.ShopMagicList;
            if(data.Count>0)
            {
                //판매 :마법
                int i = Random.Range(0, data.Count);

                shopItem = data[i];
                data[i].ApplyToUI(this);
            }else
            {
                Set();
            }
            

        }
        else if(R<=85)
        {
            //판매 : 블록
            List<BlockScriptableObject> data = ShopManager.Instance.ShopBlockList;
            //판매 :마법
            int i = Random.Range(0, data.Count);

            shopItem = data[i];
            data[i].ApplyToUI(this);
        }
        else
        {
            //판매 : 석판
            List<SlateScriptableObejct> data = ShopManager.Instance.ShopSlateList;
            int i = Random.Range(0, data.Count);

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
            if (!TalkManager.instance.SellCheck(sellable.Price))
                return; ;

            sellable.Sell();
            sellIcon.sprite = null;
            sellIcon.gameObject.SetActive(false);
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
