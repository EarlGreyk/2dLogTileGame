using System.Collections.Generic;
using UnityEngine;

//셋팅씬에서 게임씬으로 넘어갈때 해당 데이터를 넘겨줍니다.

public static class SettingData
{
  
    public static int difficult;
    public static int Stage = 1;
    public static int Round = 1;
    public static SlateScriptableObejct firstSlate;
    public static SlateScriptableObejct secondSlate;
    public static SlateScriptableObejct thirdSlate;
    public static SlateScriptableObejct fourthSlate;



    public static Dictionary<int, float> difficultPlayer = new Dictionary<int, float>();

    public static Dictionary<int, float> difficultMonster = new Dictionary<int, float>();

    public static UnitStatus LuneStatus = new UnitStatus();

    public static bool Load = false;

    public static void BgmVolumeSave(float voulme)
    {
        PlayerPrefs.SetFloat("Bgm", voulme);
    }
    public static void EffectVolumeSave(float voulme)
    {
        PlayerPrefs.SetFloat("Effect", voulme);
    }
    public static float BgmVolumeLoad()
    {
        return PlayerPrefs.GetFloat("Bgm", 0.5f);
    }
    public static float EffectVolumeLoad()
    {
        return PlayerPrefs.GetFloat("Effect", 0.5f);
    }


    public static void DifficultAdd(int id, float value, bool Player = true)
    {
        if(Player)
        {
            difficultPlayer.Add(id,value);
        }else
        {
            difficultMonster.Add(id, value);
        }
        
    }
    public static void DifficultRemove(int id , bool Player = true)
    {
       if(Player)
       {
            difficultPlayer.Remove(id);
            
       }else
       {
            difficultMonster.Remove(id);
       }
    }
}
