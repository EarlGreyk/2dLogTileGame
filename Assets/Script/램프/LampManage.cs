using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LampManage : MonoBehaviour
{

    private List<LampLight> lampLights;

    [SerializeField]
    private TextMeshProUGUI lampLevel;

    [SerializeField]
    private GameObject lightParent;

    [SerializeField]
    private GameObject lightPrePab;


    public void AddLamp(LampLight LampLight)
    {
        GameObject ob = Instantiate<GameObject>(lightPrePab, lightParent.transform);
        LampLight lampLight = ob.GetComponent<LampLight>();

        lampLight.LampLightSet(LampLight.Fire);



    }
}
