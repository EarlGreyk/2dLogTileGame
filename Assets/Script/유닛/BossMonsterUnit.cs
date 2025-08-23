using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossMonsterUnit : MonsterUnit
{

    [SerializeField]
    private MonSterMagicScriptableObejct[] SpecailMagicArray;



    

    public override void Awake()
    {
        base.Awake();
    }


    protected override void ActionSet(bool move)
    {
        //보스에만 존재하는 자신의 특수 패턴을 사전에 사용할 수 있는지 없는지 체크하고 사용합니다.
        //만약 사용하지 않는다면 일반 몬스터의 AI를 행동합니다.
        if (SpecailMagicArray.Length > 0)
        {
            if ((status.Health / status.MaxHealth) <= SpecailMagicArray[0].HPcon)
            {
                CurrentAcion.currentMagic = SpecailMagicArray[0];
                ActionCount = SpecailMagicArray[0].RequiredCost;

                //현재 해당 문구에서 사용한 특별 패턴을 제외하여 더이상 사용 못하게 설정해야합니다.
                targetPosSet();
                return;
            }
        }
        
        base.ActionSet(move);






    }
}
