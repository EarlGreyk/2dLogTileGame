using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MonsterActionInterFace : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI name;

    public TextMeshProUGUI Name { get { return name; } set { name = value; } }

    [SerializeField]
    private TextMeshProUGUI desc;

    public TextMeshProUGUI Desc { get { return desc; } set { desc = value; } }

    [SerializeField]
    private TextMeshProUGUI value;

    public TextMeshProUGUI Value { get { return this.value; } set { this.value = value; } }

    [SerializeField]
    private TextMeshProUGUI cost;





    public void InteFaceSet(MonSterMagicScriptableObejct action)
    {
        name.text = action.Name;
        desc.text = action.Desc;
        value.text = action.MagicValue.ToString();
        cost.text = action.RequiredCost.ToString();


    }

}
