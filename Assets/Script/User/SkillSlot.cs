using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillSlot : MonoBehaviour
{
    //
    private Magic magic;
    public Magic Magic { get { return magic; }}

    private GameObject magicEffect;
    public GameObject MagicEffect {  get { return magicEffect; }}




    public void magicSet(Magic magic)
    {
        this.magic = magic;
        magicEffect = Resources.Load<GameObject>("MagicAction/" + magic.Tag.ToString() +"/"+ magic.MagicName );
    }

    public void magicUseCheck()
    {
        int x = (int)GameManager.instance.PlayerUnit.transform.position.x;
        int y = (int)GameManager.instance.PlayerUnit.transform.position.y;
        Vector3Int cellPos = new Vector3Int(x, y);
        GameManager.instance.SkillZone.gameObject.SetActive(true);
        GameManager.instance.SkillZone.SettingSkillZone(magic, magicEffect);
    }
}
