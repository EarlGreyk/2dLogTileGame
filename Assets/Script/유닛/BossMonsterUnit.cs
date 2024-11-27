using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossMonsterUnit : MonsterUnit
{

    [SerializeField]
    private List<MonsterMagic> specailMagicList = new List<MonsterMagic>();



    

    public override void Start()
    {
        base.Start();
    }


    protected override void ActionSet(bool move)
    {
        //�������� �����ϴ� �ڽ��� Ư�� ������ ������ ����� �� �ִ��� ������ üũ�ϰ� ����մϴ�.
        //���� ������� �ʴ´ٸ� �Ϲ� ������ AI�� �ൿ�մϴ�.
        if(specailMagicList.Count > 0)
        {
            if ((status.Health / status.MaxHealth) <= specailMagicList[0].MagicHpCon)
            {
                CurrentAcion.currentMagic = specailMagicList[0];
                ActionCount = specailMagicList[0].MagicCost;

                specailMagicList.RemoveAt(0);   // �ش� �ε����� �׸� ����
                targetPosSet();
                return;
            }
        }
        
        base.ActionSet(move);






    }
}
