using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;
using static UnityEditor.PlayerSettings;
using Random = UnityEngine.Random;

public class MonsterUnit : Unit
{
    [SerializeField]
    private PatternData movePattenData;
    [SerializeField]
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

    //사용하는 액션의 위치.
    private Vector3Int targetPos;

    private bool isAction = false;

    public bool IsAction { get { return isAction; } set { isAction = value; } }



    public override void Start()
    {
        base.Start();
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
    /// <summary>
    /// 몬스터 유닛이 사용해야할 지점의 좌표를 설정합니다. 이는 사용하는 기술에 따라 다릅니다.
    /// </summary>
    private void targetPosSet()
    {
        ///플레이어와 유닛의 최단거리를 비교하여 작동합니다.
        ///몬스터의 MovePatten의 데이터를 가져온다음 그 내에서 플레이어와 가장 가까운 좌표로 이동합니다.
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
        if(actionList.Count > 0)
        {
            int random = Random.Range(0, actionList.Count);
            currentAction = actionList[random];
        }
    }



}
