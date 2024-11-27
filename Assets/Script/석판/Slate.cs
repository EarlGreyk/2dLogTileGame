using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/Slate", order = 1)]
public class Slate : ScriptableObject
{
    [SerializeField]
    private string slateName;
    public string SlateName { get { return slateName; } }
    [SerializeField]
    private List<Magic> magics;
    public List<Magic> Magics { get { return magics; } }

    [SerializeField]
    private string tag;
    public string Tag { get { return tag; } }

    [SerializeField]
    private bool enable = false;
    public bool Enable { get { return enable; } set { enable = value; } }

    [SerializeField]
    public List<BlockInfo> blocks;

    [SerializeField]
    public Sprite SlateSprite;

    /// <summary>
    /// 활성화에 필요한 플레이어 레벨 입니다.
    /// </summary>
    [SerializeField]
    private int enableLevel;

    public int EnableLevel { get { return enableLevel; } }



}
