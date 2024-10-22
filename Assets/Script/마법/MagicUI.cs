using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MagicUI : MonoBehaviour
{

    private Magic magic;


    public Magic Magic { get { return magic; } set { magic = value; MagicSet(); } }

    [SerializeField]
    private Image magicImage;

    public Image MagicImage { get { return magicImage; } set { magicImage = value; } }


    private void Awake()
    {
        magicImage = GetComponent<Image>();
    }

    public void MagicSet()
    {
        if (magic != null)
        {
            magicImage.sprite = magic.IconSprite;
        }
        
    }


}
