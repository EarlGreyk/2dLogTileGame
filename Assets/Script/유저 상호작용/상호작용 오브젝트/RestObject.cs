using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RestObject : InteractionObject
{
    private void Awake()
    {
        interactionType = Type.Rest;
    }



    private void Start()
    {
        //위험도 증가값.
        value = 20;
    }

    public override void InteractSet()
    {
        base.InteractSet();
    }

    public override void InteractStart()
    {
        base.InteractStart();
        if (GameProsessManager.instance.LampLight == GameProsessManager.instance.MaxLampLight)
        {
            ErrorManager.instance.ErrorSet("불씨가 최대임으로 휴식하지 못합니다.");
            return;
        }
            
        GameProsessManager.instance.LampLight = GameProsessManager.instance.MaxLampLight;
        GameProsessManager.instance.Dangering(value);
    }
    public override void InteractEnd()
    {
        base.InteractEnd();
    }

}
