using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SkillSlot : MonoBehaviour
{
    //
    private Magic magic;
    public Magic Magic { get { return magic; }}

    [SerializeField]
    private Image magicImage;
    [SerializeField]
    private TextMeshProUGUI magicCost;

    private GameObject magicEffect;
    public GameObject MagicEffect {  get { return magicEffect; }}




    public void magicSet(Magic magic)
    {
        this.magic = magic;
        magicImage.sprite = magic.IconSprite;
        magicCost.text = magic.MagicCost.ToString();

        magicEffect = Resources.Load<GameObject>("MagicAction/" + magic.Tag.ToString() +"/"+ magic.MagicName );
        gameObject.SetActive( true );
    }

    public void magicUseCheck()
    {
        int x = (int)GameManager.instance.PlayerUnit.transform.position.x;
        int y = (int)GameManager.instance.PlayerUnit.transform.position.y;
        Vector3Int cellPos = new Vector3Int(x, y);
        GameManager.instance.SkillZone.gameObject.SetActive(true);
        GameManager.instance.MoveZone.gameObject.SetActive(false);
        GameManager.instance.SkillZone.SettingSkillZone(magic, magicEffect);
    }
}
