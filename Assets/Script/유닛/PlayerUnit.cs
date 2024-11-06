using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerUnit : Unit
{
    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();

        status.effectAdd(SettingData.LuneStatus);
    }




}
