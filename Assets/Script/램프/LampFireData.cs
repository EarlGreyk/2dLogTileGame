using System;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/LampFire", order = 1)]
public class LampFireData : ScriptableObject
{

    /// <summary>
    /// 광원 이름
    /// </summary>
    [SerializeField]
    private string fireName;

    /// <summary>
    /// /// 광원 등급
    /// </summary>
    [SerializeField]
    private int fireGrade;
    public int FireGrade { get { return fireGrade; } }


    /// <summary>
    /// 광원 설명
    /// </summary>
    [SerializeField]
    private string fileDesc;

    public string FileDesc { get { return fileDesc; } }

    /// <summary>
    /// 광원 이미지
    /// </summary>
    [SerializeField]
    private Sprite fireIcon;

    public Sprite FireIcon { get { return fireIcon; } }



    /// <summary>
    /// 플레이어의 모든 스텟을 받아야합니다.
    /// </summary>
    [SerializeField]
    private enum fireType
    {
        DopChance,
        MagicChian,
        MagicCount

    }
    private fireType firetype;



    [SerializeField]
    private bool enable = false;
    public bool Enable { get { return enable; } set { enable = value; } }


    /// <summary>
    /// 활성화에 필요한 플레이어 레벨 입니다.
    /// </summary>
    [SerializeField]
    private int enableLevel;

    public int EnableLevel { get { return enableLevel; } }
}

