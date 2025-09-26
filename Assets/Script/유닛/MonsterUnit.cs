using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.Mathematics;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using Random = UnityEngine.Random;



public class MonsterUnit : Unit
{



    public enum Position
    {
        Melee,
        Range,
        Surport
    }
    public Position position;


    [SerializeField]
    private PatternData movePattenData;

    public PatternData MovePattenData { get { return movePattenData; } set { movePattenData = value; } } 


    [SerializeField]
    private PatternData attackRangePattenData;
    [SerializeField]
    private MonsterScriptableObject ratioStatus;

    public MonsterScriptableObject RatioStatus { get { return ratioStatus; } }

    /// <summary>
    /// 몬스터가 남은 행동까지의 카운트
    /// </summary>
    private int actionCount;
    public int ActionCount 
    { get { return actionCount; }
        set
        {
            actionCount = value;

            if(IsAction)
            {
                if (currentAction.currentMagic == null)
                {
                    List<string> text = new List<string>() { "이동", ActionCount.ToString() };
                    hpbar.ActionSet(text);
                }
                else
                {
                    List<string> text = new List<string>() { currentAction.currentMagic.Name, ActionCount.ToString() };
                    hpbar.ActionSet(text);
                }
            }
            else
            {
                List<string> text = new List<string>() { "턴 대기중", maxActionCount.ToString() };
                hpbar.ActionSet(text);
            }
            
        }
    }
    //몬스터가 사용하는 액션 종류
    //공격 및 유닛에게 사용하는 서포트 마법이 들어갑니다.
    [SerializeField]
    private MonSterMagicScriptableObejct[] attackMagicArray;

    public MonSterMagicScriptableObejct[] AttackMagicArray { get { return attackMagicArray; } }

    //방어 마법이 들어갑니다.
    [SerializeField]
    private MonSterMagicScriptableObejct[] defenceMagicArray;

    public MonSterMagicScriptableObejct[] DefenceMagicArray  { get { return defenceMagicArray; } }

    //몬스터가 현재 사용하는 액션[마법]
    [SerializeField]
    private MonsterAction currentAction;

    public MonsterAction CurrentAcion { get { return currentAction; } }

    //사용하는 액션의 위치.
    private List<Vector3Int> targetPosList = new List<Vector3Int>();

    public List<Vector3Int> TargetPosList { get { return targetPosList; } }

    private List<Vector3Int> movePosPath;

    public List<Vector3Int> MovePosPath { get { return movePosPath; } }


    private bool isAction = false;

    public bool IsAction { get { return isAction; } set { isAction = value; } }

    public int maxActionCount;



    public int KillGold;

    /// <summary>
    /// 몬스터가 이동하는대에 행동 카운트.
    /// </summary>
    private int moveCount = 2;

    /// <summary>
    /// 몬스터가 다음애 행동할 랜덤액션값
    /// </summary>
    private int randomAction = 0;

    /// <summary>
    /// 몬스터가 감소시켜야할 행동 카운트
    /// </summary>
    private int disCount = 0;





    public override void Awake()
    {
        base.Awake();

        currentAction.Unit = this;
        hpbar.HpTextSet();
        ActionCheck();
        

    }
    public void Init(MonsterScriptableObject data)
    {
        SpriteRenderer.sprite = data.MonsterIcon;

        ratioStatus = data;
        if(status == null)
        {
            status = new UnitStatus(baseStatus);
        }
        //유닛이 공통적으로 적용 받는 스테이터스 적용
        if(data != null)
            status.effectRatio(data);


        //몬스터 유닛만이 가지고 있는 스텟 적용
        maxActionCount = data.ActionPoint;
        
        movePattenData = data.MovePattern[0];
        attackMagicArray = ratioStatus.UsingMagic;
        attackRangePattenData = attackMagicArray[0].MagicDamageRange;
        randomAction = Random.Range(0, attackMagicArray.Length);
     
        
     
    }
    private void OnMouseDown()
    {
        
        if (EventSystem.current != null && EventSystem.current.IsPointerOverGameObject())
        {
            return;
        }
        GameManager.instance.UnitInfoManager.TargetUnitSet(this);
    }


    public void ActionStart()
    {
        //행동 시작
        //몬스터가 행동을 시작하기 위해서 게임 매니저에 보내서 작동을 한다고 선언합니다.
        isAction = true;
        ActionCount = maxActionCount;
        monsterAction();
    }




    //몬스터 행동을 재탐색한뒤 실행하는 함수입니다.
    public void monsterAction()
    {
        GameManager.instance.BlockModeZone.unitBlockSet(this);
        if (currentAction.currentMagic == null)
            StartCoroutine(ActionMove());
        else
            currentAction.onAction();
    }
   
    //아래의 함수는 액션을 선택하고 설정합니다.
    //몬스터의 기본적인 AI입니다.

    public void ActionCheck()
    {
        
        //GameManager에 있는 플레이어의 좌표를 가져와 BattleZone에서 비교하여
        //현재 몬스터 유닛의 공격 사거리에들어와 있는지를 체크합니다.
        //이는 이동을 할지 액션 공격을 할지를 선정합니다. [인식범위]
        bool isCheck = false;
        Vector3Int unitPos = new Vector3Int((int)transform.position.x, (int)transform.position.y, 0);
        List<PatternData.PatternPoint> pattern = attackRangePattenData.points;
        foreach (var pos in pattern)
        {
            //중간값이 2,2이기 떄문에 -2씩 연산
            int x = Mathf.FloorToInt((pos.x - 3) + unitPos.x / GameManager.instance.Grid.transform.localScale.x);
            int y = Mathf.FloorToInt((pos.y - 3) + unitPos.y / GameManager.instance.Grid.transform.localScale.y);
            Vector3Int tilepos = new Vector3Int(x, y, 0);
            if (x < GameManager.instance.BattleZone.BattleTiles.GetLength(0) && y < GameManager.instance.BattleZone.BattleTiles.GetLength(1) 
                & x>=0 & y>=0)
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
            Debug.Log("다음 행동 : 공격");
        }
        else
        {
            ActionSet(true);
            Debug.Log("다음 행동 : 이동");
        }
    }
    /// <summary>
    /// 몬스터의 행동 알고리즘을 보여줍니다.
    /// 매 플레이어의 행동 체크마다 해당 액션을 해서 현재 행동값으로 보여주어야 합니다.
    /// </summary>
    protected virtual void ActionSet(bool move)
    {

        if (move)
        {
            currentAction.currentMagic = null;
            ActionCount -= moveCount;
        }
        else
        {
            bool dcheck = false;

            if (randomAction > 7 && (status.Health / status.MaxHealth) < 0.6 && defenceMagicArray.Length > 0)
                dcheck = true;
          

            if (!dcheck)
            {
                currentAction.currentMagic = attackMagicArray[randomAction];
            }
            

        }

        if(isAction)
        {
            if(move)
            {
                disCount= moveCount;
            }else
            {
                disCount = currentAction.currentMagic.RequiredCost;
            }
        }
        targetPosSet();
    }


    /// <summary>
    /// 몬스터 유닛이 사용해야할 지점의 좌표를 설정합니다. 이는 사용하는 기술에 따라 다릅니다.
    /// 또한 기술 혹은 이동을 같이 시행합니다.
    /// </summary>

  

    protected void targetPosSet()
    {
        //타겟리스트 초기화
        for(int i =0; i < targetPosList.Count;i++)
        {
            GameManager.instance.BattleZone.removeTempTile(targetPosList[i],false);
        }
        targetPosList.Clear();
        //

        Vector3 unitPos = new Vector3(transform.position.x / GameManager.instance.Grid.transform.localScale.x,
                transform.position.y / GameManager.instance.Grid.transform.localScale.y, 0);
        Vector3 playerPos = new Vector3(
            GameManager.instance.PlayerUnit.transform.position.x / GameManager.instance.Grid.transform.localScale.x,
            GameManager.instance.PlayerUnit.transform.position.y / GameManager.instance.Grid.transform.localScale.y,
            0);

            ///이동 타겟지점 설정
        if (currentAction.currentMagic == null)
        {
            

            List<PatternData.PatternPoint> pattern = movePattenData.points;

            List<Vector3Int> validPositions = movePattenData.points.Select(p =>
            {
                int x = Mathf.FloorToInt((p.x - 3) + unitPos.x);
                int y = Mathf.FloorToInt((p.y - 3) + unitPos.y);
                return new Vector3Int(x, y, 0);

            }).ToList();


            // BFS를 사용하여 유닛이 이동 가능한 위치를 탐색
            Vector3Int closestPosition = FindClosestPositionWithPattern(new Vector3Int((int)unitPos.x, (int)unitPos.y, 0), new Vector3Int((int)playerPos.x, (int)playerPos.y, 0), pattern);
            if (closestPosition != unitPos)
            {
                this.targetPosList.Add(closestPosition);
                movePosPath = FindPathWithBFS(new Vector3Int((int)unitPos.x, (int)unitPos.y, 0), closestPosition, validPositions);
                GameManager.instance.BattleZone.setTempTile(targetPosList[0],false);
            }
            else
            {
                Debug.Log("경로를 찾을 수 없습니다.");
            }



        }
        else
        {
            // 액션값이 있다면 해당 액션값에서의 공격범위내에 유닛이 있다는 것이니 타겟이 되는 대상자의 지점을 체크해야합니다.
            //서포팅 기술은 추가로 체크해야합니다.
            // 타겟이 다중인 기술은 현재 자신의 유닛을 위치를 기점으로 랜덤한 범위를 산출해야합니다.
            if(currentAction.currentMagic.Target ==1)
            {
                //공격타입입니다.
                if(currentAction.currentMagic.Operating_type == 0)
                {
                    targetPosList.Add(new Vector3Int((int)playerPos.x, (int)playerPos.y,0));
                }
            }
            else if(currentAction.currentMagic.Target == 1 && currentAction.currentMagic.Operating_type ==3)
            {
                List<PatternData.PatternPoint> pattern = currentAction.currentMagic.MagicDamageRange.points;

                List<Vector3Int> validPositions = currentAction.currentMagic.MagicDamageRange.points.Select(p =>
                {
                    int x = Mathf.FloorToInt((p.x - 3) + unitPos.x);
                    int y = Mathf.FloorToInt((p.y - 3) + unitPos.y);
                    return new Vector3Int(x, y, 0);
                }).ToList();


                // BFS를 사용하여 유닛이 공격 위치를 탐색
                for(int i =0;i<currentAction.currentMagic.Target;i++)
                {
                    Vector3Int RandomPosition = FindRandomPositionWithPattern(new Vector3Int((int)unitPos.x, (int)unitPos.y, 0), validPositions);
                    if (RandomPosition != unitPos)
                    {
                        this.targetPosList.Add(RandomPosition);
                    }
                    else
                    {
                        Debug.Log("경로를 찾을 수 없습니다.");
                    }
                }
               
            }


            
        }

   

    }


    private Vector3Int FindRandomPositionWithPattern(Vector3Int start, List<Vector3Int> PosionList)
    {
        List<Vector3Int> PositionList = new List<Vector3Int>();
        Vector3Int RandomPosition = start;
        
        foreach (var pos in PosionList)
        {

            if (pos.x >= 0 && pos.y >= 0 && pos.x < GameManager.instance.BattleZone.BattleTiles.GetLength(0) && 
                pos.y < GameManager.instance.BattleZone.BattleTiles.GetLength(1) &&
                GameManager.instance.BattleZone.BattleTiles[pos.x, pos.y].type != BattleTile.tileType.Break &&
                GameManager.instance.BattleZone.BattleTiles[pos.x, pos.y].onUnit == null && GameManager.instance.BattleZone.BattleTiles[pos.x, pos.y].tempTile == false)
            {
                PositionList.Add(new Vector3Int(pos.x, pos.y, 0));
            }
        }
        Debug.Log(PosionList);
        int random = Random.Range(0, PositionList.Count);

        RandomPosition = PositionList[random];



        return RandomPosition;
    }


    /// <summary>
    /// 최단거리를 탐색하기전에 탐색 범위를 제한합니다.
    /// </summary>
    /// <param name="start"></범위 탐색의 시작점>
    /// <param name="goal"></도착점 이 도착점은 범위 탐색 밖에서 탐색해도 되는 위치입니다.>
    /// <param name="pattern"></탐색해야하는 범위>
    /// <returns></returns>

    private Vector3Int FindClosestPositionWithPattern(Vector3Int start, Vector3Int goal, List<PatternData.PatternPoint> pattern)
    {
        Vector3Int closestPosition = start;
        float closestDistance = float.MaxValue;
        foreach (var pos in pattern)
        {
            int newX = Mathf.FloorToInt((pos.x - 3) + start.x);
            int newY = Mathf.FloorToInt((pos.y - 3) + start.y);
            if (newX >= 0 && newY >= 0 && newX < GameManager.instance.BattleZone.BattleTiles.GetLength(0) &&
                newY < GameManager.instance.BattleZone.BattleTiles.GetLength(1) &&
                GameManager.instance.BattleZone.BattleTiles[newX, newY].type != BattleTile.tileType.Break &&
                GameManager.instance.BattleZone.BattleTiles[newX, newY].onUnit == null &&
                GameManager.instance.BattleZone.BattleTiles[newX, newY].tempTile == false)
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
    /// 해당 지점까지의 최단경로를 찾습니다.
    /// </summary>
    /// <param name="start"></시작점>
    /// <param name="goal"></종료지점>
    /// <param name="validPositions"></인자>
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
    /// 

    IEnumerator ActionMove()
    {
        yield return new WaitForSeconds(0.5f);
        Debug.Log($"몬스터이동 유닛여부체크: {targetPosList[0]}");
        if (!GameManager.instance.BattleZone.SerchTileUnit(targetPosList[0]))
        {
            Vector3Int scaledCellPos = new Vector3Int(
            Mathf.FloorToInt(targetPosList[0].x * GameManager.instance.Grid.transform.localScale.x),
            Mathf.FloorToInt(targetPosList[0].y * GameManager.instance.Grid.transform.localScale.y),
            0);
            GameManager.instance.BattleZone.removeTileUnit(transform.position, this);
            transform.position = scaledCellPos;
            GameManager.instance.BattleZone.setTileUnit(scaledCellPos, this);
            ReAction();
        }
        else
        {
            targetPosSet();
        }
        yield return null;
        yield break;

        

    }


    public void ReAction()
    {
        ActionCount -= disCount;
        if (ActionCount < moveCount)
        {
            isAction = false;
            GameManager.instance.MonsterAIManager.CurrentMonster = null;
            ActionCount = 0;

            Debug.Log("몬스터의 행동이 종료되엇습니다");
        }
        else
        {
            Debug.Log("현재 액션값이 이동보다 높음으로 다시 재 탐색을 시행합니다.");
            ActionCheck();
            monsterAction();


        }
    }

    public override void UnitDie()
    {
        base.UnitDie();
        GameManager.instance.MonsterAIManager.MonsterRevmoe(this);
        
    }



}
