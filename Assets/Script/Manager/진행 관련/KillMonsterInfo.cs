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




    public void InfoSet(string Iconpath,int Count,int Gold, Transform parent)
    {
        monsterIcon.sprite = Resources.Load<Sprite>("Sprite/∏ÛΩ∫≈Õ/" + Iconpath);
        killCount.text = Count.ToString();
        killGold.text = Gold.ToString();
        gameObject.transform.parent = parent;  
    }


}
