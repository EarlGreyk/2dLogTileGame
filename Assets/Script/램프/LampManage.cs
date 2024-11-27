using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LampManage : MonoBehaviour
{
    public static LampManage Instance;

    [SerializeField]
    private List<LampLight> lampLights;

    [SerializeField]
    private TextMeshProUGUI lampLevelText;

    [SerializeField]
    private GameObject lightParent;

    [SerializeField]
    private GameObject lightPrePab;

    [SerializeField]
    private LampFireData[] lampFires;
    public LampFireData[] LampFires { get { return lampFires; } }

    private List<LampLight> equipLampLight = new List<LampLight>();

    public List<LampLight> EquipLampLight { get { return equipLampLight; } }

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
        {
            Destroy(Instance);
            Instance = this;
        }
    }

    private void Start()
    {
        lampFires = Resources.LoadAll<LampFireData>("램프 정보");

        if (SettingData.Load == true)
        {
            List<string> dataList = SaveLoadManager.instance.LightManagerSave.lampFireName;

            for(int i = 0; i < dataList.Count; i++)
            {
                AddLamp(dataList[i]);
            }

        }
    }

    public void SetLampLightS()
    {

        if (lampFires.Length > 0)
        {
            for (int i =0; i< lampLights.Count; i++)
            {
                int randomIndex = UnityEngine.Random.Range(0, lampFires.Length);

                Debug.Log(lampLights[i]);
                lampLights[i].LampLightDataSet(lampFires[randomIndex]);

            }

        }
        else
        {
            Debug.LogWarning("램프 폴더에 LampFireData가 없습니다.");
        }
    }



    public void AddLamp(LampLight LampLight)
    {
        Debug.Log(LampLight);
        Debug.Log(LampLight.FireData);
        LampLight lampLight = Instantiate<GameObject>(lightPrePab, lightParent.transform).GetComponent<LampLight>();
        Debug.Log(lampLight);
        lampLight.LampLightDataSet(LampLight.FireData);
        equipLampLight.Add(LampLight);

    }
    public void AddLamp(string LampFireDataName)
    {
        GameObject ob = Instantiate<GameObject>(lightPrePab, lightParent.transform);
        LampLight lampLight = ob.GetComponent<LampLight>();


        LampFireData firedata = Resources.Load<LampFireData>("램프정보/" + LampFireDataName);
        lampLight.LampLightDataSet(firedata);

    }
}
