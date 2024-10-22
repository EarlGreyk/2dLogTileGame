using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicManager : MonoBehaviour
{
    [SerializeField]
    private List<MagicUI> magicUIList;


    [SerializeField]
    private MagicDesc magicDesc;


    public virtual void Set(SlateUI slateUI)
    {
        magicListSet(slateUI.Slate.Magics);
    }

    public void magicListSet(List<Magic> magics)
    {
        for(int i =0; i < magicUIList.Count; i++)
        {
            Debug.Log($"{i} , {magics.Count}");
            if (i >= magics.Count)
                return;
            magicUIList[i].Magic = magics[i];
        }
    }


    public void magicDescSet(MagicUI magicUI)
    {
        magicDesc.DescSet(magicUI);
    }
}
