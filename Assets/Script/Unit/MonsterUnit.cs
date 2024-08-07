using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using static UnityEditor.PlayerSettings;
using Random = UnityEngine.Random;

public class MonsterUnit : Unit
{
    private PatternData movePattenData;
    private PatternData attackRangePattenData;

    /// <summary>
    /// 몬스터가 남은 행동까지의 카운트
    /// </summary>
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
    /// <summary>
    /// 몬스터가 이동하는대에 행동 카운트.
    /// </summary>
    private int moveCount;
    //몬스터가 사용하는 액션 종류
    [SerializeField]
    private List<MonsterAction> actionList = new List<MonsterAction>();

    //몬스터가 현재 사용하는 액션
    private MonsterAction currentAction;

    private bool isAction = false;

    public bool IsAction { get { return isAction; } set { isAction = value; } }
   



    private void Start()
    {

        GameManager.instance.BattleZone.setTileUnit(transform.position,this);
        ActionCheck();
        
    }

 



    //actionCount 가 0이면 실행합니다.
    private void monsterAction()
    {
        //행동 시작
        //몬스터가 행동을 시작하기 위해서 게임 매니저에 보내서 작동을 한다고 선언합니다.
        isAction = true;
        GameManager.instance.addActionMonster(this);

        if (currentAction == null)
        {
            //이동
            movePosSet();
            ActionCheck();
            return;

        }
        else
        {
            currentAction.onAction();
            //기술 행동
            //몬스터 Action에서 몬스터 행동하는것을 가져와서 행동한다음 끝낫을때를 비교하여 ActionCheck를 돌려야합니다.
        }
        




        //사용이 완료 되엇음으로 actionCount를 올리고 GameManager에 완료되엇다고 신호를 보내줍니다.
        GameManager.instance.reamoveActionMonster(this);
    }
    private void movePosSet()
    {

    }

    //아래의 함수는 액션을 선택하고 설정합니다.

    private void ActionCheck()
    {
        //GameManager에 있는 플레이어의 좌표를 가져와 BattleZone에서 비교하여
        //현재 몬스터 유닛의 인식범위에들어와 있는지를 체크합니다.
        bool isCheck = false;

        List<PatternData.PatternPoint> pattern = attackRangePattenData.points;
        int lengthX = GameManager.instance.BattleZone.BattleTiles.GetLength(0) / 2;
        int lengthY = GameManager.instance.BattleZone.BattleTiles.GetLength(1) / 2;
        Vector3Int unitPos = new Vector3Int((int)transform.position.x, (int)transform.position.y, 0);
        foreach (var pos in pattern)
        {
            int x = pos.x - 2 + unitPos.x;
            int y = pos.y - 2 + unitPos.y;
            Vector3Int tilepos = new Vector3Int(x, y);
            if (Math.Abs(x) <= lengthX && Math.Abs(y) <= lengthY)
            {
                x = x + lengthX;
                y = y + lengthY;
                if (GameManager.instance.BattleZone.BattleTiles[x, y].onUnit == GameManager.instance.PlayerUnit)
                {
                    isCheck = true;
                }

            }
        }

        if(isCheck)
        {
            ActionSet(false);
        }
        else
        {
            ActionSet(true);
        }
    }
    /// <summary>
    /// 몬스터의 행동 알고리즘을 보여줍니다.
    /// 몬스터는 다음 행동을 미리 알려주어야 하기 때문에 행동이 끝나면 
    /// 함수를 작동 시켜야합니다.
    /// </summary>
    private void ActionSet(bool move)
    {
        if(move)
        {
            actionCount = moveCount;
            currentAction = null;
        }
        int random = Random.Range(0, actionList.Count);
        currentAction = actionList[random];
    }



}
