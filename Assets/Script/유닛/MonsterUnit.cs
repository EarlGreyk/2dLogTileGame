using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEditor;
using UnityEngine;

using Random = UnityEngine.Random;



public class MonsterUnit : Unit
{
    [SerializeField]
    private PatternData movePattenData;
    [SerializeField]
    private PatternData attackRangePattenData;
    [SerializeField]
    private UnitStatusObject ratioStatus;

    /// <summary>
    /// 몬스터가 남은 행동까지의 카운트
    /// </summary>
    private int actionCount;
    public int ActionCount 
    { get { return actionCount; }
        set
        { 
            actionCount = value;
            if(actionCount > 0)
            {
                if (currentAction.currentMagic == null)
                {
                    List<string> text = new List<string>() { "이동", ActionCount.ToString() };
                    hpbar.ActionSet(text);
                }
                else
                {
                    List<string> text = new List<string>() { currentAction.currentMagic.MagicName, ActionCount.ToString() };
                    hpbar.ActionSet(text);
                }
            }else
            {
                List<string> text = new List<string>() { "순번 대기중", ActionCount.ToString() };
                hpbar.ActionSet(text);
            }
            
            
        }
    }
    /// <summary>
    /// 몬스터가 이동하는대에 행동 카운트.
    /// </summary>
    private int moveCount;
    //몬스터가 사용하는 액션 종류
    [SerializeField]
    private List<MonsterMagic> magicList = new List<MonsterMagic>();

    //몬스터가 현재 사용하는 액션
    [SerializeField]
    private MonsterAction currentAction;

    //사용하는 액션의 위치.
    private Vector3Int targetPos;
    public Vector3Int TargetPos { get { return targetPos; } }
    private List<Vector3Int> movePosPath;

    public List<Vector3Int> MovePosPath { get { return movePosPath; } }


    private bool isAction = false;

    public bool IsAction { get { return isAction; } set { isAction = value; } }

    public int maxActionCount;


    




    public override void Start()
    {
        base.Start();
        currentAction.Unit = this;
        status.effectRatio(ratioStatus);
        hpbar.HpTextSet();
        ActionCheck();
        
    }


 



    //actionCount 가 0이면 실행합니다.
    public void monsterAction()
    {
        //행동 시작
        //몬스터가 행동을 시작하기 위해서 게임 매니저에 보내서 작동을 한다고 선언합니다.
        isAction = true;
        if (currentAction.currentMagic == null)
            ActionMove();
        else
            currentAction.onAction();





        //사용이 완료 되엇음으로 actionCount를 올리고 GameManager에 완료되엇다고 신호를 보내줍니다.
    }
 
    //아래의 함수는 액션을 선택하고 설정합니다.

    public void ActionCheck()
    {
        isAction = false;
        GameManager.instance.MonsterAIManager.CurrentMonster = null;
        //GameManager에 있는 플레이어의 좌표를 가져와 BattleZone에서 비교하여
        //현재 몬스터 유닛의 공격 사거리에들어와 있는지를 체크합니다.
        //이는 이동을 할지 액션 공격을 할지를 선정합니다. [인식범위]
        bool isCheck = false;

        Vector3Int unitPos = new Vector3Int((int)transform.position.x, (int)transform.position.y, 0);
        List<PatternData.PatternPoint> pattern = attackRangePattenData.points;

        foreach (var pos in pattern)
        {
            //중간값이 2,2이기 떄문에 -2씩 연산
            int x = Mathf.FloorToInt((pos.x - 2) + unitPos.x / GameManager.instance.Grid.transform.localScale.x);
            int y = Mathf.FloorToInt((pos.y - 2) + unitPos.y / GameManager.instance.Grid.transform.localScale.y);
            Vector3Int tilepos = new Vector3Int(x, y, 0);
            if (x < GameManager.instance.BattleZone.BattleTiles.GetLength(0) && y < GameManager.instance.BattleZone.BattleTiles.GetLength(1))
            {
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
            Debug.Log("공격범위내에 플레이어가 없습니다. 이동을 시작합니다");
            currentAction.currentMagic = null;
            ActionCount = maxActionCount;
            //List<string> text = new List<string>() { "이동", ActionCount.ToString() };
        }
        else if(magicList.Count > 0)
        {
            
            int random = Random.Range(0, magicList.Count);
            currentAction.currentMagic = magicList[random];
            ActionCount = currentAction.currentMagic.MagicCost;
            //List<string> text = new List<string>() { currentAction.currentMagic.MagicName, ActionCount.ToString() };
        }
        targetPosSet();
    }


    /// <summary>
    /// 몬스터 유닛이 사용해야할 지점의 좌표를 설정합니다. 이는 사용하는 기술에 따라 다릅니다.
    /// </summary>

   


    private void targetPosSet()
    {
        Vector3 unitPos = new Vector3(transform.position.x / GameManager.instance.Grid.transform.localScale.x,
                transform.position.y / GameManager.instance.Grid.transform.localScale.y, 0);
        Vector3 playerPos = new Vector3(
            GameManager.instance.PlayerUnit.transform.position.x / GameManager.instance.Grid.transform.localScale.x,
            GameManager.instance.PlayerUnit.transform.position.y / GameManager.instance.Grid.transform.localScale.y,
            0);
        if (currentAction.currentMagic == null)
        {

            List<PatternData.PatternPoint> pattern = movePattenData.points;

            List<Vector3Int> validPositions = movePattenData.points.Select(p =>
            {
                int x = Mathf.FloorToInt((p.x - 2) + unitPos.x);
                int y = Mathf.FloorToInt((p.y - 2) + unitPos.y);
                return new Vector3Int(x, y, 0);
            }).ToList();


            // BFS를 사용하여 유닛이 이동 가능한 위치를 탐색
            Vector3Int closestPosition = FindClosestPositionWithPattern(new Vector3Int((int)unitPos.x, (int)unitPos.y, 0), new Vector3Int((int)playerPos.x, (int)playerPos.y, 0), pattern);
            if (closestPosition != unitPos)
            {
                this.targetPos = closestPosition;
                movePosPath = FindPathWithBFS(new Vector3Int((int)unitPos.x, (int)unitPos.y, 0), closestPosition, validPositions);
            }
            else
            {
                Debug.Log("경로를 찾을 수 없습니다.");
            }
        }
        else
        {
            // 액션값이 있다면 해당 액션값에서의 공격범위내에 유닛이 있다는 것이니 플레이어의 유닛 지점을 저장해야합니다.
            this.targetPos = new Vector3Int((int)playerPos.x, (int)playerPos.y,0);
        }
    }


    /// <summary>
    /// 최단거리를 찾습니다.
    /// </summary>
    /// <param name="start"></param>
    /// <param name="goal"></param>
    /// <param name="pattern"></param>
    /// <returns></returns>

    private Vector3Int FindClosestPositionWithPattern(Vector3Int start, Vector3Int goal, List<PatternData.PatternPoint> pattern)
    {
        Vector3Int closestPosition = start;
        float closestDistance = float.MaxValue;

        foreach (var pos in pattern)
        {
            int newX = Mathf.FloorToInt((pos.x - 2) + start.x);
            int newY = Mathf.FloorToInt((pos.y - 2) + start.y);

            if (newX >= 0 && newY >= 0 && newX < GameManager.instance.BattleZone.BattleTiles.GetLength(0) && newY < GameManager.instance.BattleZone.BattleTiles.GetLength(1) &&
                GameManager.instance.BattleZone.BattleTiles[newX, newY].type != BattleTile.tileType.Break &&
                GameManager.instance.BattleZone.BattleTiles[newX, newY].onUnit == null)
            {
                float distanceToGoal = Vector3.Distance(new Vector3(newX, newY, 0), new Vector3(goal.x, goal.y, 0));
                if (distanceToGoal < closestDistance)
                {
                    closestDistance = distanceToGoal;
                    closestPosition = new Vector3Int(newX, newY, 0);
                }
            }
        }

        return closestPosition;
    }


    /// <summary>
    /// 해당 지점까지의 경로를 찾습니다.
    /// </summary>
    /// <param name="start"></param>
    /// <param name="goal"></param>
    /// <param name="validPositions"></param>
    /// <returns></returns>
    private List<Vector3Int> FindPathWithBFS(Vector3Int start, Vector3Int goal, List<Vector3Int> validPositions)
    {
        int rows = GameManager.instance.BattleZone.BattleTiles.GetLength(0);
        int cols = GameManager.instance.BattleZone.BattleTiles.GetLength(1);
        bool[,] visited = new bool[rows, cols];
        Vector3Int[,] parent = new Vector3Int[rows, cols];
        Queue<Vector3Int> queue = new Queue<Vector3Int>();

        queue.Enqueue(start);
        visited[start.x, start.y] = true;
        parent[start.x, start.y] = new Vector3Int(-1, -1, 0);

        while (queue.Count > 0)
        {
            var current = queue.Dequeue();

            if (current == goal)
            {
                // 경로를 추적하여 반환
                List<Vector3Int> path = new List<Vector3Int>();
                var step = goal;
                while (step != new Vector3Int(-1, -1, 0))
                {
                    path.Add(step);
                    step = parent[step.x, step.y];
                }
                path.Reverse();
                return path;
            }

            // 탐색할 방향 정의
            int[,] directions = { { 0, 1 }, { 1, 0 }, { 0, -1 }, { -1, 0 } };

            for (int i = 0; i < directions.GetLength(0); i++)
            {
                int newX = current.x + directions[i, 0];
                int newY = current.y + directions[i, 1];
                Vector3Int newPos = new Vector3Int(newX, newY, 0);

                if (newX >= 0 && newY >= 0 && newX < rows && newY < cols &&
                    validPositions.Contains(newPos) &&
                    !visited[newX, newY])
                {
                    queue.Enqueue(newPos);
                    visited[newX, newY] = true;
                    parent[newX, newY] = current;
                }
            }
          
        }

        return new List<Vector3Int>(); // 경로를 찾을 수 없는 경우
    }


    /// <summary>
    /// 이동 액션입니다.
    /// 만약 내가 가야하는 경로에 유닛이 있다면 재탐색 이후 다시 해당 함수를 시행합니다.
    /// </summary>
    private void ActionMove()
    {

        if (!GameManager.instance.BattleZone.SerchTileUnit(targetPos))
        {
            Vector3Int scaledCellPos = new Vector3Int(
            Mathf.FloorToInt(targetPos.x * GameManager.instance.Grid.transform.localScale.x),
            Mathf.FloorToInt(targetPos.y * GameManager.instance.Grid.transform.localScale.y),
            0);
            GameManager.instance.BattleZone.removeTileUnit(transform.position, this);
            transform.position = scaledCellPos;
            GameManager.instance.BattleZone.setTileUnit(scaledCellPos, this);
            ActionCheck();
        }else
        {
            targetPosSet();
            ActionMove();
        }

        

    }

    public override void UnitDie()
    {
        base.UnitDie();
        GameManager.instance.MonsterAIManager.MonsterRevmoe(this);
        
    }



}
