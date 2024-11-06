using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LuneUi : MonoBehaviour
{
    
    

    [SerializeField]
    private TextMeshProUGUI luneName;

    [SerializeField]
    private TextMeshProUGUI lunePrice;

    [SerializeField]
    private TextMeshProUGUI luneDesc;

    [SerializeField]
    private TextMeshProUGUI luneApplyValue;

   




 
    

    public void onSet(BagicLune node)
    {
        if (node == null)
            return;
        gameObject.SetActive(true);
        luneName.text = node.LuneName;
        luneDesc.text = node.LuneDesc;
        lunePrice.text = node.LunePrice.ToString();

        ApplyTextSet(node);



    }
    public void onSet(MajorLune node)
    {
        if (node == null)
            return;

        gameObject.SetActive(true);
        luneName.text = node.LuneName + "\n";
        luneDesc.text = node.LuneDesc;

        //ApplyTextSet();

    }   


    private void ApplyTextSet(BagicLune node)
    {

        Debug.Log(LuneManager.instance.luneTotalStatus);
        if(node.effectType == BagicLune.EffectType.Health)
            luneApplyValue.text = "체력 : " + LuneManager.instance.luneTotalStatus.Health.ToString() +" + " +  node.effectValue.ToString(); 
        if(node.effectType == BagicLune.EffectType.Damage)
            luneApplyValue.text = "데미지 : " + LuneManager.instance.luneTotalStatus.Damage.ToString() + " + " + node.effectValue.ToString();
        if (node.effectType == BagicLune.EffectType.Defence)
            luneApplyValue.text = "방어력 : " + LuneManager.instance.luneTotalStatus.Defense.ToString() + " + " + node.effectValue.ToString();


    }

    public void offSet()
    {
        gameObject.SetActive(false);
    }
}