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
        //보스에만 존재하는 자신의 특수 패턴을 사전에 사용할 수 있는지 없는지 체크하고 사용합니다.
        //만약 사용하지 않는다면 일반 몬스터의 AI를 행동합니다.
        if(specailMagicList.Count > 0)
        {
            if ((status.Health / status.MaxHealth) <= specailMagicList[0].MagicHpCon)
            {
                CurrentAcion.currentMagic = specailMagicList[0];
                ActionCount = specailMagicList[0].MagicCost;

                specailMagicList.RemoveAt(0);   // 해당 인덱스의 항목 삭제
                targetPosSet();
                return;
            }
        }
        
        base.ActionSet(move);






    }
}
