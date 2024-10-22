using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

public class MovePanel : MonoBehaviour
{
    
    public Block block; // 패널 참조
    public MoveZone Map; // 타일참조

    public Image image;
    public TextMeshProUGUI mana;
    /// <summary>
    /// 테스트용
    /// </summary>

    public void BlockSet(Block block)
    {
        this.block = block;
        gameObject.SetActive(true);
        image.sprite = block.BlockInfo.sprite;
        mana.text = block.mana.ToString();
    }
    public void onBolckTile()
    {
        

        int x = (int)GameManager.instance.PlayerUnit.transform.position.x;
        int y = (int)GameManager.instance.PlayerUnit.transform.position.y;
        Vector3Int cellPos = new Vector3Int(x, y);
        GameManager.instance.MoveZone.gameObject.SetActive(true);
        GameManager.instance.SkillZone.gameObject.SetActive(false);
        PlayerResource.instance.currentBlock = this;
        Map.SetBlock(this, cellPos);
    }    

}
