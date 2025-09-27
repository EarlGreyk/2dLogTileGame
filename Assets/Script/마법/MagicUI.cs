using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MagicUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{

    [SerializeField]
    private MagicScriptableObejct catalogMagic;

    public MagicScriptableObejct CatalogMagic { get { return catalogMagic; } set { catalogMagic = value; } }

    private MagicOrigin magic;


    public MagicOrigin Magic { get { return magic; }  }

    [SerializeField]
    private Image magicImage;

    public Image MagicImage { get { return magicImage; } set { magicImage = value; } }

    private MagicDesc targetDesc;


    private void Start()
    {
        if(magic ==null && catalogMagic ==null)
        {
            MagicImage.gameObject.SetActive(false);
        }
    }


    /// <summary>
    /// 게임을 플레이 하고 있을때 data값을 쓰는게 아닌 가공한 MagicOrigin값을 쓸떄 사용됩니다.
    /// </summary>
    /// <param name="magicOrigin"></param>
    public void MagicSet(MagicOrigin magicOrigin)
    {
        if (magicOrigin != null)
        {
            magicImage.gameObject.SetActive(true);
            magic = magicOrigin;
            magicImage.sprite = magicOrigin.MagicSprite;
        }
        
    }
    /// <summary>
    /// 게임 플레이전. 설정 및 도감에서 사용합니다.
    /// 가공하지 않는 원본 데이터를 보여줍니다.
    /// </summary>
    /// <param name="magicData"></param>
    public void MagicSet(MagicScriptableObejct magicData)
    {
        if (magicData != null)
        {
            catalogMagic = magicData;
            magicImage.gameObject.SetActive(true);
            magicImage.sprite = magicData.MagicSprite;
        }
    }
    public void MagicClear()
    {
        magic = null;
        catalogMagic = null;
        magicImage.sprite = null;
        magicImage.gameObject.SetActive(false);
    }

    /// <summary>
    /// 포인터 추가값입니다.
    /// </summary>
    /// <param name="desc"></param>

    public void EventInit(MagicDesc desc)
    {
        targetDesc = desc;

    }

    public void OnPointerEnter(PointerEventData eventData)
    {

        if (targetDesc != null)
        {
            targetDesc.gameObject.SetActive(true);
            targetDesc.DescSet(catalogMagic, GetComponent<RectTransform>());
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (targetDesc != null)
        {
            targetDesc.DescClear();
        }
    }


}
