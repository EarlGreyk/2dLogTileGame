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






    //actionCount 가 0이면 실행합니다.
    private void monsterAction()
    {
        //몬스터가 행동을 시작하기 위해서 게임 매니저에 보내서 작동을 한다고 선언합니다.
        GameManager.instance.addActionMonster(this);
        //해당 몬스터가 가지고 있는 패턴 기술을 가져와서 사용합니다.



        //사용이 완료 되엇음으로 actionCount를 올리고 GameManager에 완료되엇다고 신호를 보내줍니다.
        actionCount = maxActionCount;
    }




}
