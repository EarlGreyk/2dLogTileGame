using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LampLight : MonoBehaviour
{
    
    private LampFireData fireData;

    public LampFireData FireData { get { return fireData; } set { fireData = value; } }
    [SerializeField]
    private Image fireIcon;
    [SerializeField]
    private TextMeshProUGUI fireDesc;
    // Start is called before the first frame update


   

    

    public void LampLightDataSet(LampFireData fireData)
    {
        this.fireData = fireData;
        fireIcon.sprite = fireData.FireIcon;    
        fireDesc.text = fireData.FileDesc;
        

    }
  
}
