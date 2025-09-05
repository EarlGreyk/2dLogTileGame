using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MagicUpgrade : MonoBehaviour
{
    // Start is called before the first frame update

    private MagicUI magicUI;

    private int needGold;

    [SerializeField]
    private TextMeshProUGUI needGoldText;

    [SerializeField]
    private MagicDesc currenntLevel;

    
    


    public void Setting(MagicUI magicUI)
    {
        this.magicUI = magicUI;
        //юс╫ц
        needGold = this.magicUI.Magic.Gold;
        needGoldText.text = needGold.ToString();

        currenntLevel.DescSet(this.magicUI);

    }
    
}
