using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.UI;

public class PopUp : MonoBehaviour
{
    [SerializeField]
    private GameObject pop;
    public GameObject Pop { get { return pop; } }
    private Animation ani;


    private void Start()
    {
        
    }


    public void EnablePop()
    {
        if(Pop != null)
        {
            StartCoroutine(ImageOpen(2f));
        }
    }
    public void ChangePop()
    {
        if(Pop != null)
        {
            PopUpManager.instance.PopupChange(Pop);
        }
    }



    IEnumerator ImageOpen(float time)
    {
        //애니메이션 동작 넣기

        //해당 팝업이 활성화 되어있는지 체크하고 작동.
        if (!Pop.activeSelf)
        {
            PopUpManager.instance.PopupPush(this);
        }
        else
            PopUpManager.instance.LastClosePopUp();
        yield return null;
    }
}
