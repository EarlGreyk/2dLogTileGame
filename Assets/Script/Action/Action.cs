using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Action: MonoBehaviour
{
    public float time;
    public float endtime;

    private void Start()
    {
        time += Time.deltaTime;
        if (time > endtime)
        {
            endAction();
        }
    }

    private void endAction()
    {
        Destroy(gameObject);
    }

}
