using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlateInventory : MonoBehaviour
{
    //실질적으로 플레이어가 들고있는 slate값입니다.
    //획득할 경우 해당 리스트에 넣어주면 됩니다.
    private List<SlateOrigin> slateOrigins = new List<SlateOrigin>();


    //플레이어에게 UI상으로 들고있는 slate를 보여주기 위해 넣어줍니다.
    //장착할 경우 해당 SlateUI에서 제거해줘야합니다.
    private List<SlateUI> slatePanel = new List<SlateUI>();



    [SerializeField]
    private SlateDesc slateDesc;




    public void SlatePush(SlateOrigin slate)
    {

        slateOrigins.Add(slate);
        
      
    }



    public void SlateDescSet(SlateOrigin selectSlate)
    {

    }



}
