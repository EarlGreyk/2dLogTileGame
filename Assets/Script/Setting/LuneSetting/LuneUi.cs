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

   




 
    

    public void onSet(LuneScriptableObejct node)
    {
        if (node == null)
            return;
        gameObject.SetActive(true);
        luneName.text = node.LuneName;
        luneDesc.text = node.LuneDesc;
        lunePrice.text = node.LunePrice.ToString();

        ApplyTextSet(node);



    }
  


    private void ApplyTextSet(LuneScriptableObejct node)
    {
     
        Debug.Log(LuneManager.instance.luneTotalStatus);
        if (node.effectType == LuneScriptableObejct.EffectType.Health)
            luneApplyValue.text = "체력 : " + LuneManager.instance.luneTotalStatus.Health.ToString() + " + " + node.effectValue.ToString();
        else if (node.effectType == LuneScriptableObejct.EffectType.ElementalDamage)
            luneApplyValue.text = "속성 데미지 배율 : " + LuneManager.instance.luneTotalStatus.ElementalDamage.ToString() + " + " + node.effectValue.ToString();
        else if (node.effectType == LuneScriptableObejct.EffectType.Damage)
            luneApplyValue.text = "무속성 데미지 배율 : " + LuneManager.instance.luneTotalStatus.Damage.ToString() + " + " + node.effectValue.ToString();
        else if (node.effectType == LuneScriptableObejct.EffectType.Defence)
            luneApplyValue.text = "보호막 생성 비율 : " + LuneManager.instance.luneTotalStatus.Defense.ToString() + " + " + node.effectValue.ToString();
        else if (node.effectType == LuneScriptableObejct.EffectType.MagicChan)
            luneApplyValue.text = "영감 추가 발동 확률 : " + LuneManager.instance.luneTotalStatus.MagicChan.ToString() + " + " + node.effectValue.ToString();
        else if (node.effectType == LuneScriptableObejct.EffectType.MagicCount)
            luneApplyValue.text = "영감시 추가 획득율 : " + LuneManager.instance.luneTotalStatus.MagicCount.ToString() + " + " + node.effectValue.ToString();
        else
            luneApplyValue.text = "오류";

    }

    public void offSet()
    {
        gameObject.SetActive(false);
    }
}