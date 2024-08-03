using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;


public class LuneManager : MonoBehaviour
{
    public static LuneManager instance;
    [SerializeField]
    private List<LuneSetting> LuneSettings = new List<LuneSetting>();



    [SerializeField]
    private LuneUi luneUi;
    public LuneUi LuneUi { get { return luneUi; } }



    public UnitStatus luneTotalStatus { get; set; }

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }else
        {
            Destroy(gameObject);
        }
            

    }
    private void Start()
    {
        luneTotalStatus = new UnitStatus();
    }
    public void luneSet()
    {
        SettingData.playerStatus.effectCopy(luneTotalStatus);
    }
      



}   
