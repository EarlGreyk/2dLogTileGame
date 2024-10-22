using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LampLight : MonoBehaviour
{
    private LampFire fire;
    public LampFire Fire;
    [SerializeField]
    private Image LightIcon;
    [SerializeField]
    private TextMeshProUGUI LightDesc;
    // Start is called before the first frame update

    public void LampLightSet(LampFire fire)
    {
        this.fire = fire;
        

    }
  
}
