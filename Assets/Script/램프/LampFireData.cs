using System;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/LampFire", order = 1)]
public class LampFireData : ScriptableObject
{

    /// <summary>
    /// ���� �̸�
    /// </summary>
    [SerializeField]
    private string fireName;

    /// <summary>
    /// /// ���� ���
    /// </summary>
    [SerializeField]
    private int fireGrade;
    public int FireGrade { get { return fireGrade; } }


    /// <summary>
    /// ���� ����
    /// </summary>
    [SerializeField]
    private string fileDesc;

    public string FileDesc { get { return fileDesc; } }

    /// <summary>
    /// ���� �̹���
    /// </summary>
    [SerializeField]
    private Sprite fireIcon;

    public Sprite FireIcon { get { return fireIcon; } }



    /// <summary>
    /// �÷��̾��� ��� ������ �޾ƾ��մϴ�.
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
    /// Ȱ��ȭ�� �ʿ��� �÷��̾� ���� �Դϴ�.
    /// </summary>
    [SerializeField]
    private int enableLevel;

    public int EnableLevel { get { return enableLevel; } }
}

