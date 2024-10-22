using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// BlockManage�� ���Ǵ� block �Դϴ�.
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
    /// ����� �����մϴ�
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
    /// ���� ����� ���� ����� ���ü��� �����ֱ� ���� ���˴ϴ�.
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
