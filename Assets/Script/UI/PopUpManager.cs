using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopUpManager : MonoBehaviour
{
    public static PopUpManager instance;
    
    private Stack<PopUp> popUpStack = new Stack<PopUp>();

    private GameObject popUpChange;



    private void Awake()
    {
        if(instance==null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }else
        {
            Destroy(instance.gameObject);
            instance = this;
        }
    }
    private void Update()
    {
        if(Input.GetKeyUp(KeyCode.Escape)) 
        {
            LastClosePopUp();
        }
    }
    public void PopupPush(PopUp pop)
    {
        popUpStack.Push(pop);
        pop.Pop.SetActive(true);


    }
    public void LastClosePopUp()
    {
        if (popUpStack.Count <= 0)
            return;

        PopUp popUp = popUpStack.Pop();
        popUp.Pop.SetActive(false);

    }
    public void PopupChange(GameObject popGo)
    {
        if(popUpChange!=null)
        {
            popUpChange.gameObject.SetActive(false);
        }
        popUpChange = popGo;
        popUpChange.SetActive(true);



    }



  

 
    
}
