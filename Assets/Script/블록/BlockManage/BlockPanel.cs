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
 
        blockImage.sprite = block.BlockInfo.sprite; 
        mana.text = block.mana.ToString();
        this.block = block;
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
