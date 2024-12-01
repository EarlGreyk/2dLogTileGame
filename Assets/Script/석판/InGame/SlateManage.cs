using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// ���� �������� ������ �������ݴϴ�. 
/// �÷��̾� ���ҽ� ���� slate�� �����;��մϴ�.
/// </summary>
public class SlateManage : MonoBehaviour
{

    [SerializeField]
    private SlateUI firstSlate;
    [SerializeField]
    private SlateUI secondSlate;
    [SerializeField]
    private SlateUI thirdSlate;
    [SerializeField]
    private SlateUI fourthSlate;


    private Slate selctSlate;



    private void Start()
    {
        firstSlate.SlateSet(PlayerResource.instance.FirstSlate);
        secondSlate.SlateSet(PlayerResource.instance.SecondSlate);
        thirdSlate.SlateSet(PlayerResource.instance.ThirdSlate);
        fourthSlate.SlateSet(PlayerResource.instance.FirstSlate);

    }
    /// <summary>
    /// ��ư���� ������Ʈ�� �����մϴ�.
    /// </summary>
    public void SelectSlate(SlateUI slateui)
    {
        Slate slate = slateui.Slate;
        if (slate == null)
        {
            Debug.Log("���� : ������ Slate�� ���� �����Դϴ�.");
            return;
        }
            
        if (slate == firstSlate)
            selctSlate = slate;
        else if (slate == secondSlate)
            selctSlate = slate;
        else if (slate == thirdSlate)
            selctSlate = slate;
        else
            selctSlate = slate;

    }
    /// <summary>
    /// ��ư���� �����̸��� �������� �÷� ��ų�� Ȱ��ȭ�մϴ�.
    /// </summary>

    public void SlateLevelUp()
    {
        if (selctSlate == null)
        {
            Debug.Log("���� : ������ Slate�� ���� �����Դϴ�.");
            return;
        }


        if (selctSlate == PlayerResource.instance.FirstSlate)
        {
            firstSlate.SlateLevelUp();
            PlayerResource.instance.FirstSlateLevel++;
        }
        else if (selctSlate == PlayerResource.instance.SecondSlate)
        {
            secondSlate.SlateLevelUp();
            PlayerResource.instance.SecondSlateLevel++;
        }
        else if (selctSlate == PlayerResource.instance.ThirdSlate)
        {
            thirdSlate.SlateLevelUp();
            PlayerResource.instance.ThirdSlateLevel++;
        }
        else if (selctSlate != PlayerResource.instance.FourthSlate)
        {
            fourthSlate.SlateLevelUp();
            PlayerResource.instance.FourSlateLevel++;
        }
        else
            Debug.Log("���� : ������ Slate�� �����ϰ��ִ� 1,2,3,4 Slate�� �ƴմϴ�.");

    }
}
