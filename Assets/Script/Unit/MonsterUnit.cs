using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterUnit : Unit
{

    private int maxActionCount;

    private int actionCount;

    public int ActionCount 
    { get { return actionCount; }
        set
        { 
            if (actionCount == 0 && value != 0)
            { 
                monsterAction(); 
            }
            actionCount = value;
        } 
    }






    //actionCount �� 0�̸� �����մϴ�.
    private void monsterAction()
    {
        //���Ͱ� �ൿ�� �����ϱ� ���ؼ� ���� �Ŵ����� ������ �۵��� �Ѵٰ� �����մϴ�.
        GameManager.instance.addActionMonster(this);
        //�ش� ���Ͱ� ������ �ִ� ���� ����� �����ͼ� ����մϴ�.



        //����� �Ϸ� �Ǿ������� actionCount�� �ø��� GameManager�� �Ϸ�Ǿ��ٰ� ��ȣ�� �����ݴϴ�.
        actionCount = maxActionCount;
    }




}
