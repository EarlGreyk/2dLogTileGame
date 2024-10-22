using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class KillMonsterInfo : MonoBehaviour
{
    [SerializeField]
    private Image monsterIcon;

    [SerializeField]
    private TextMeshProUGUI killCount;

    [SerializeField]

    private TextMeshProUGUI killGold;




    public void InfoSet(string Iconpath,int Count, Transform parent)
    {
        monsterIcon = Resources.Load<Image>(Iconpath);
        killCount.text = Count.ToString();
        killGold.text = (Count * 100).ToString();
        gameObject.transform.parent = parent;  
    }


}
