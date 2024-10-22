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
        
        StartCoroutine(SceanDelay(sceanName));
    }



    IEnumerator SceanDelay(string sceanName)
    {
        yield return new WaitForSeconds(1.5f);
        SceneManager.LoadScene(sceanName);
        yield break;
    }
}
