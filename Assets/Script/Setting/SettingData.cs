using UnityEngine;

//셋팅씬에서 게임씬으로 넘어갈때 해당 데이터를 넘겨줍니다.

public static class SettingData
{
  
    public static int difficult;
    public static int Stage = 1;
    public static int Round = 1;
    public static Slate firstSlate;
    public static Slate secondSlate;
    public static Slate thirdSlate;
    public static Slate fourthSlate;

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

}
