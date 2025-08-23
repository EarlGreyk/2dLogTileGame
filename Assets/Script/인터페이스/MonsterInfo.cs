using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MonsterInfo : MonoBehaviour
{
    [SerializeField]
    private Image monsterIcon;

    private int monsterCount;

    private int monsterGrade;


    [SerializeField]
    private TextMeshProUGUI textCount;

    // Start is called before the first frame update
    public void InfoSet(Sprite sprite, int count)
    {
        monsterIcon.sprite = sprite;
        monsterCount = count;
        textCount.text = "X " + monsterCount.ToString();
    }

    
}
