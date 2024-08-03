using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceanChanger : MonoBehaviour
{
    public static SceanChanger instance;




    private void Awake()
    {
        if(instance == null ) 
        {
            instance = this;
            DontDestroyOnLoad( gameObject);
        }else
        {
            Destroy(gameObject);
        }
    }



    public void SceanChange(string sceanName)
    {
        SceneManager.LoadScene(sceanName);
    }
}
