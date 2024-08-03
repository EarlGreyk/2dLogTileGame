using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LuneUi : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI luneDesc;


    

    public void onSet(Vector3 pos, BagicLune node)
    {
        if (node == null)
            return;
        Debug.Log(node);
        Debug.Log(node.LuneDesc);
        Debug.Log(node.LuneName);
        gameObject.SetActive(true);
        this.transform.position = pos;
        luneDesc.text = node.LuneName;
        luneDesc.text += node.LuneDesc ;
        

    }
    public void onSet(Vector3 pos, MajorLune node)
    {
        if (node == null)
            return;
        Debug.Log(node);
        Debug.Log(node.LuneDesc);
        gameObject.SetActive(true);
        this.transform.position = pos;
        luneDesc.text = node.LuneName + "\n";
        luneDesc.text += node.LuneDesc;


    }
    public void offSet()
    {
        gameObject.SetActive(false);
    }
}