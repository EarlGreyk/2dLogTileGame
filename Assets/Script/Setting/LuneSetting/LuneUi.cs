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
        if (node.effectType == BagicLune.EffectType.Health)
            luneApplyValue.text = "ü�� : " + LuneManager.instance.luneTotalStatus.Health.ToString() + " + " + node.effectValue.ToString();
        else if (node.effectType == BagicLune.EffectType.ElementalDamage)
            luneApplyValue.text = "�Ӽ� ������ ���� : " + LuneManager.instance.luneTotalStatus.ElementalDamage.ToString() + " + " + node.effectValue.ToString();
        else if (node.effectType == BagicLune.EffectType.Damage)
            luneApplyValue.text = "���Ӽ� ������ ���� : " + LuneManager.instance.luneTotalStatus.Damage.ToString() + " + " + node.effectValue.ToString();
        else if (node.effectType == BagicLune.EffectType.Defence)
            luneApplyValue.text = "��ȣ�� ���� ���� : " + LuneManager.instance.luneTotalStatus.Defense.ToString() + " + " + node.effectValue.ToString();
        else if (node.effectType == BagicLune.EffectType.MagicChan)
            luneApplyValue.text = "���� �߰� �ߵ� Ȯ�� : " + LuneManager.instance.luneTotalStatus.MagicChan.ToString() + " + " + node.effectValue.ToString();
        else if (node.effectType == BagicLune.EffectType.MagicCount)
            luneApplyValue.text = "������ �߰� ȹ���� : " + LuneManager.instance.luneTotalStatus.MagicCount.ToString() + " + " + node.effectValue.ToString();
        else if (node.effectType == BagicLune.EffectType.CriChan)
            luneApplyValue.text = "ũ��Ƽ�� Ȯ�� : " + LuneManager.instance.luneTotalStatus.CriChan.ToString() + " + " + node.effectValue.ToString();
        else if (node.effectType == BagicLune.EffectType.CriMul)
            luneApplyValue.text = "ũ��Ƽ�� ���� : " + LuneManager.instance.luneTotalStatus.CriMul.ToString() + " + " + node.effectValue.ToString();
        else
            luneApplyValue.text = "����";

    }

    public void offSet()
    {
        gameObject.SetActive(false);
    }
}