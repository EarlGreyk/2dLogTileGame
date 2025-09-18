using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopObject : InteractionObject
{
    // Start is called before the first frame update


    

    





    private void InteractSet()
    {
        interactionType = Type.Shop;
    }


    public override void InteractStart()
    {
        base.InteractStart();
        TalkManager.instance.TalkSet(this); 


    }

    public override void InteractEnd()
    {
        base.InteractEnd();
    }

    public override void PlayerColiderEnter()
    {
        base.PlayerColiderEnter();
    }

    public override void PlayerColiderExit()
    {
        base.PlayerColiderExit();
    }



}
