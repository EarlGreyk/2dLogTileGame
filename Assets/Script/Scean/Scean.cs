using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scean : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    private string nextSceanname;
    private Animation ani;

    
    private void Start()
    {

    }
    

    public void EnableScean()
    {
        if (nextSceanname != null)
        {
            SceanChanger.instance.SceanChange(nextSceanname);
        }
    }



    
}
