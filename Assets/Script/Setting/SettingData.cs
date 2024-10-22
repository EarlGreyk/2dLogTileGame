using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//셋팅씬에서 게임씬으로 넘어갈때 해당 데이터를 넘겨줍니다.

public static class SettingData
{
    // Start is called before the first frame update
    public static int difficult;
    public static int Stage = 1;
    public static int Round = 1;
    public static Slate firstSlate;
    public static Slate secondSlate;
    public static Slate thirdSlate;
    public static Slate fourthSlate;



    public static UnitStatus playerStatus;

    public static bool Load = false;

    

}
