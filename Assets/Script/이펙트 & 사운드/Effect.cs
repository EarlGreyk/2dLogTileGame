using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Effect : MonoBehaviour
{
    private float time;
    [SerializeField]
    private float endtime;
    [SerializeField]
    private AudioClip soundClip;

    private void Start()
    {
        MagicSound();
    }


    private void Update()
    {
        time += Time.deltaTime;
        if (time > endtime)
        {
            Destroy(gameObject);
        }
    }

    public void MagicSound()
    {
        SoundManager.instance.AudioPlay(soundClip);
    }




    

    
}
