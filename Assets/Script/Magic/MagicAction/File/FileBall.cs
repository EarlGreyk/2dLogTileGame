using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class FileBall : MagicAcion
{
    private void Start()
    {

        endtime = 2f;

    }
    private void Update()
    {
        time += Time.deltaTime;
        if (time > endtime)
        {
            endAction();
        }
    }
    public override void MagicAnimation()
    {
        // FireBall�� ���� �ִϸ��̼� ����
        Debug.Log("FireBall �ִϸ��̼� ����");

    }

    public override void MagicSetDamage()
    {
        // FireBall�� ���� ������ ���� ����
        Debug.Log("FireBall ������ ����");
    }

    private void endAction()
    {
        Destroy(gameObject);

    }
}
