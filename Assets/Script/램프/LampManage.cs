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
    private List<LampFireData> lampFires = new List<LampFireData>();
    public List<LampFireData> LampFires { get { return lampFires; } }

    private List<LampLight> equipLampLight = new List<LampLight>();

    public List<LampLight> EquipLampLight { get { return equipLampLight; } }

    private List<LampFireData> removeLampFires = new List<LampFireData>();

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
        LightFireDataSet();
        LampFireData[] lampAllFires = Resources.LoadAll<LampFireData>("���� ����");

        for(int i=0;i<lampAllFires.Length;i++)
        {
            lampFires.Add(lampAllFires[i]); 
        }
        

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

        if (lampFires.Count > 0)
        {
            for (int i =0; i< lampLights.Count; i++)
            {
                int randomIndex = UnityEngine.Random.Range(0, lampFires.Count);

                lampLights[i].LampLightDataSet(lampFires[randomIndex]);
                removeLampFires.Add(lampFires[randomIndex]);
                lampFires.Remove(lampFires[randomIndex]);

            }

        }
        else
        {
            Debug.LogWarning("���� ������ LampFireData�� �����ϴ�.");
        }
    }



    public void AddLamp(LampLight LampLight)
    {

        LampLight lampLight = Instantiate<GameObject>(lightPrePab, lightParent.transform).GetComponent<LampLight>();
      
        lampLight.LampLightDataSet(LampLight.FireData);
        equipLampLight.Add(LampLight);

        //�����Ȱ� ���� �ٸ� 2���� �Ҳ� �����͸� �ٽ� ��ȯ�մϴ�.
        for(int i = removeLampFires.Count-1; i>=0;i--)
        {
            if(LampLight.FireData == removeLampFires[i])
            {
                removeLampFires.RemoveAt(i);
            }
        }
        for(int i =0; i< removeLampFires.Count;i++)
        {
            lampFires.Add(removeLampFires[i]);
        }
        removeLampFires.Clear();

    }
    public void AddLamp(string LampFireDataName)
    {
        GameObject ob = Instantiate<GameObject>(lightPrePab, lightParent.transform);
        LampLight lampLight = ob.GetComponent<LampLight>();


        LampFireData firedata = Resources.Load<LampFireData>("��������/" + LampFireDataName);
        lampLight.LampLightDataSet(firedata);
        lampFires.Remove(firedata);

    }

    //�ڽ��� ���� ������ �´� ��� �Һ��� Ȱ��ȭ �մϴ�.
    //���� �ش� �Լ��� LightCollection���� �����մϴ�. 
    //�ش� ������  playerfrefbas�� �Űܼ� �۵���ų�� �����غ����� �մϴ�.
    private void LightFireDataSet()
    {
        LampFireData[] lampFireData = Resources.LoadAll<LampFireData>("��������");


        for (int i = 0; i < lampFireData.Length; i++)
        {
            if (PlayerLevelManager.instance.Level >= lampFireData[i].EnableLevel)
            {
                lampFireData[i].Enable = true;
            }
        }
    }
}
