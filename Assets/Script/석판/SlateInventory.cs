using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlateInventory : MonoBehaviour
{
    public static SlateInventory instance;
    //실질적으로 플레이어가 들고있는 slate값입니다.
    //획득할 경우 해당 리스트에 넣어주면 됩니다.
    private List<SlateOrigin> slateOrigins = new List<SlateOrigin>();


    //플레이어에게 UI상으로 들고있는 slate를 보여주기 위해 넣어줍니다.
    //장착할 경우 해당 SlateUI에서 제거해줘야합니다.
    [SerializeField]
    private List<SlateUI> slateUIPanel = new List<SlateUI>();



    [SerializeField]
    private SlateDesc slateDesc;

    private void Awake()
    {
        if(instance != null)
        {
            Destroy(instance);
        }else
        {
            instance = this;
        }
    }

    private void Start()
    {
        for (int i = 0; i < slateUIPanel.Count; i++)
        {
            slateOrigins.Add(null);
        }
    }

    /// <summary>
    /// 어떤경우로 Slate를 획득할 경우 넣어줍니다
    /// </summary>
    /// <param name="slate"></param>

    public void SlateAdd(SlateOrigin slate)
    {

        for(int i =0;i<slateUIPanel.Count;i++)
        {
            if(slateUIPanel[i].RuntimeSlate == null )
            {
                slateOrigins[i] = slate;
                slateUIPanel[i].SlateSet(slate);
                break;
            }
        }
      
    }
    /// <summary>
    /// 마법에 slate를 장착할 경우 탐색하여 리스트에서 제거합니다.
    /// </summary>
    /// <param name="slate"></param>
    public void SlateEquip(SlateUI slate)
    {
        // slateOrigin과 UI의 번호는 전부 같은 매칭입니다.
        for(int i =0; i< slateOrigins.Count;i++)
        {
            Debug.Log($"{slateOrigins[i]} 과 {slate.RuntimeSlate}");

            if (slateOrigins[i] == slate.RuntimeSlate)
            {
                Debug.Log(i);
                slateOrigins[i] = null;
                slateUIPanel[i].SlateClear();
                break;
            }
        }
        MagicManager.instance.SlateActivate(slate.RuntimeSlate);
    }


    /// <summary>
    /// 마우스가 Slate에 올라오면 상세 정보를 보여줍니다,
    /// </summary>
    /// <param name="selectSlate"></param>
    public void SlateDescSet(SlateOrigin selectSlate)
    {

    }



}
