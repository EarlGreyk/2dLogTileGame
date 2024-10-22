using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// BlockManage에 사용되는 block 입니다.
/// </summary>
public class BlockPanel : MonoBehaviour
{
    // Start is called before the first frame update

    private Block block = null;
    public Block Block { get { return block; } }

    [SerializeField]
    private Image blockImage;

    public Image BlockImage { get { return blockImage; } }
    [SerializeField]
    private TextMeshProUGUI mana;
   

    /// <summary>
    /// 블록을 설정합니다
    /// </summary>
    /// <param name="block"></param>
    public void Set(Block block)
    {
        this.block = block;
        blockImage.sprite = this.block.BlockInfo.sprite;
        mana.text = this.block.mana.ToString();
        gameObject.SetActive(true);
    }
    /// <summary>
    /// 현재 블록의 다음 등급의 가시성을 보여주기 위해 사용됩니다.
    /// </summary>
    public void nextSet()
    {
        mana.text = (block.mana + 1).ToString();
    }
    public void LevelUp()
    {
        block.LevelUp();
        mana.text = block.mana.ToString();
    }
    public void Clear()
    {
        this.block = null;
        blockImage.sprite = null;
        mana.text = null;
        gameObject.SetActive(false);

    }

   
}
