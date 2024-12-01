using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ErrorManager : MonoBehaviour
{
    public static ErrorManager instance;

    [SerializeField]
    private Image errorImage;
    [SerializeField]
    private TextMeshProUGUI errorText;

    private Coroutine coroutineA;






    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {

            Destroy(instance.gameObject);
            instance = this;
        }

    }

    public void ErrorSet(string message)
    {
        errorImage.gameObject.SetActive(true);
        errorText.text = message;
        if (coroutineA != null)
        {
            StopCoroutine(coroutineA);
        }
        coroutineA = StartCoroutine(ErrorOpacity());

    }

    IEnumerator ErrorOpacity()
    {
        float time = 1.5f;
        float delay = 0;
        float a = 0;
        Color errorColor = errorImage.color;
        Color errorTextColor = errorText.color;
        while (time > 0)
        {
            delay = Time.deltaTime;
            time -= delay;
            a = time / 1.5f;
            errorColor.a = a;
            errorImage.color = errorColor;
            errorTextColor.a = a;
            errorText.color = errorTextColor;

            yield return new WaitForSeconds(delay);
        }
        errorImage.gameObject.SetActive(false);
        coroutineA = null;
        yield return null;
        yield break;
    }
}
