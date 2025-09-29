using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 블록 UI에 쓰는 스크립트입니다.
/// 현재 자원값 class의 이름은 xxUi로 되어있어 해당 클래스의 이름도 수정해야합니다.
/// </summary>
public class BlockPanel : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    private BlockScriptableObject catalogBlock;

    public BlockScriptableObject CatalogBlock { get { return catalogBlock; } set { catalogBlock = value; } }

    private Block block = null;
    public Block Block { get { return block; } }

    [SerializeField]
    private Image blockImage;

    public Image BlockImage { get { return blockImage; } }
    [SerializeField]
    private TextMeshProUGUI mana;

    /// <summary>
    /// 도감에 사용합니다.
    /// </summary>
    /// <param name="block"></param>
    public void Set(BlockScriptableObject block)
    {
        catalogBlock = block;
        blockImage.sprite = block.Icon;
        mana.text = block.BlockChargingMana[0].ToString();
    }


    /// <summary>
    /// 블록을 설정합니다
    /// </summary>
    /// <param name="block"></param>
    public void Set(Block block, bool activeSelt = true)
    {
        this.block = block;
        blockImage.sprite = block.BlockInfo.Icon; 
        mana.text = block.BlockInfo.BlockChargingMana[block.level - 1].ToString();
        this.block = block;
        if(activeSelt)
            gameObject.SetActive(true);
    }
    /// <summary>
    /// 현재 블록의 다음 등급의 가시성을 보여주기 위해 사용됩니다.
    /// </summary>
    public void nextSet()
    {
        mana.text = block.BlockInfo.BlockChargingMana[block.level].ToString();
    }
    public void LevelUp()
    {
        block.LevelUp();
        mana.text = block.BlockInfo.BlockChargingMana[block.level - 1].ToString();
    }
    public void Clear()
    {
        this.block = null;
        blockImage.sprite = null;
        mana.text = null;
        gameObject.SetActive(false);

    }

   
}
