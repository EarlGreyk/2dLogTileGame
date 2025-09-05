using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopObject : InteractionObject
{
    // Start is called before the first frame update


    //상점을 한번이라도 접근하고 나왔을때 true로 반환됩니다.
    //해당 값에 따라 상점과 접근할 수 있는지 없는지 체크됩니다.

    public bool Sell;





    private void InteractSet()
    {
        interactionType = Type.Shop;
    }

    public override void Update()
    {
        base.Update();
        if (Input.GetKey(KeyCode.G) && GameManager.instance.GameProsessManager.prosessType == GameProsessManager.ProsessType.Stay)
        {
            Debug.Log("상점전환");


        }
    }

    public override void InteractStart()
    {
        base.InteractEnd();
    }

    public override void InteractEnd()
    {
        base.InteractEnd();
    }


    public override void OnTriggerEnter2D(Collider2D other)
    {
        if (Sell)
            return;

        if (other.CompareTag("Player"))
        {
            Debug.Log("플레이어가 상점 유닛에 접근햇습니다.");
            targetObj = other.gameObject.transform;
            


        }

    }

    public override void OnTriggerExit2D(Collider2D other)
    {


        if (other.CompareTag("Player"))
        {
            Debug.Log("플레이어가 상점 유닛에 나갔습니다.");
            targetObj = null;
        }
    }
}
