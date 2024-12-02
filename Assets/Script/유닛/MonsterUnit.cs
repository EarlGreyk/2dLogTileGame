using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEditor;
using UnityEngine;

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
    [SerializeField]
    private PatternData attackRangePattenData;
    [SerializeField]
    private UnitStatusObject ratioStatus;

    /// <summary>
    /// ���Ͱ� ���� �ൿ������ ī��Ʈ
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
                    List<string> text = new List<string>() { "�̵�", ActionCount.ToString() };
                    hpbar.ActionSet(text);
                }
                else
                {
                    List<string> text = new List<string>() { currentAction.currentMagic.MagicName, ActionCount.ToString() };
                    hpbar.ActionSet(text);
                }
            }else
            {
                List<string> text = new List<string>() { "���� �����", ActionCount.ToString() };
                hpbar.ActionSet(text);
            }
            
            
        }
    }
    /// <summary>
    /// ���Ͱ� �̵��ϴ´뿡 �ൿ ī��Ʈ.
    /// </summary>
    private int moveCount;
    //���Ͱ� ����ϴ� �׼� ����
    //���� �� ���ֿ��� ����ϴ� ����Ʈ ������ ���ϴ�.
    [SerializeField]
    private List<MonsterMagic> attackMagicList = new List<MonsterMagic>();

    //��� ������ ���ϴ�.
    [SerializeField]
    private List<MonsterMagic> defenceMagicList = new List<MonsterMagic>();

    //���Ͱ� ���� ����ϴ� �׼�[����]
    [SerializeField]
    private MonsterAction currentAction;

    public MonsterAction CurrentAcion { get { return currentAction; } }

    //����ϴ� �׼��� ��ġ.
    private List<Vector3Int> targetPosList = new List<Vector3Int>();

    public List<Vector3Int> TargetPosList { get { return targetPosList; } }

    private List<Vector3Int> movePosPath;

    public List<Vector3Int> MovePosPath { get { return movePosPath; } }


    private bool isAction = false;

    public bool IsAction { get { return isAction; } set { isAction = value; } }

    public int maxActionCount;



    public int KillGold;

    




    public override void Start()
    {
        base.Start();
        currentAction.Unit = this;
        status.effectRatio(ratioStatus);
        hpbar.HpTextSet();
        ActionCheck();
       

    }
    private void OnMouseDown()
    {
        GameManager.instance.BlockModeZone.unitBlockSet(this);
    }







    //actionCount �� 0�̸� �����մϴ�.
    //�̴� ���� AI�޴������� �����մϴ�.
    public void monsterAction()
    {
        //�ൿ ����
        //���Ͱ� �ൿ�� �����ϱ� ���ؼ� ���� �Ŵ����� ������ �۵��� �Ѵٰ� �����մϴ�.
        isAction = true;
        GameManager.instance.BlockModeZone.unitBlockSet(this);
        if (currentAction.currentMagic == null)
            StartCoroutine(ActionMove());
        else
            currentAction.onAction();





        //����� �Ϸ� �Ǿ������� actionCount�� �ø��� GameManager�� �Ϸ�Ǿ��ٰ� ��ȣ�� �����ݴϴ�.
    }
 
    //�Ʒ��� �Լ��� �׼��� �����ϰ� �����մϴ�.
    //������ �⺻���� AI�Դϴ�.

    public void ActionCheck()
    {
        isAction = false;
        GameManager.instance.MonsterAIManager.CurrentMonster = null;
        //GameManager�� �ִ� �÷��̾��� ��ǥ�� ������ BattleZone���� ���Ͽ�
        //���� ���� ������ ���� ��Ÿ������� �ִ����� üũ�մϴ�.
        //�̴� �̵��� ���� �׼� ������ ������ �����մϴ�. [�νĹ���]
        bool isCheck = false;
        Vector3Int unitPos = new Vector3Int((int)transform.position.x, (int)transform.position.y, 0);
        List<PatternData.PatternPoint> pattern = attackRangePattenData.points;
        foreach (var pos in pattern)
        {
            //�߰����� 2,2�̱� ������ -2�� ����
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
        }
        else
        {
            ActionSet(true);
        }
    }
    /// <summary>
    /// ������ �ൿ �˰����� �����ݴϴ�.
    /// ���ʹ� ���� �ൿ�� �̸� �˷��־�� �ϱ� ������ �ൿ�� ������ 
    /// �Լ��� �۵� ���Ѿ��մϴ�.
    /// </summary>
    protected virtual void ActionSet(bool move)
    {
        if (move)
        {
            currentAction.currentMagic = null;
            ActionCount = maxActionCount;
        }
        else
        {
            int random = Random.Range(0, 10);
            bool dcheck = false;

            if (random > 7 && (status.Health / status.MaxHealth) < 0.6 && defenceMagicList.Count > 0)
                dcheck = true;

            if (!dcheck)
            {
                random = Random.Range(0, attackMagicList.Count);
                currentAction.currentMagic = attackMagicList[random];
                ActionCount = currentAction.currentMagic.MagicCost;
            }
            else
            {
                random = Random.Range(0, defenceMagicList.Count);
                currentAction.currentMagic = defenceMagicList[random];
                ActionCount = currentAction.currentMagic.MagicCost;
                return;
            }

        }
        targetPosSet();
    }


    /// <summary>
    /// ���� ������ ����ؾ��� ������ ��ǥ�� �����մϴ�. �̴� ����ϴ� ����� ���� �ٸ��ϴ�.
    /// </summary>

   


    protected void targetPosSet()
    {
        //Ÿ�ٸ���Ʈ �ʱ�ȭ
        targetPosList.Clear();
        //

        Vector3 unitPos = new Vector3(transform.position.x / GameManager.instance.Grid.transform.localScale.x,
                transform.position.y / GameManager.instance.Grid.transform.localScale.y, 0);
        Vector3 playerPos = new Vector3(
            GameManager.instance.PlayerUnit.transform.position.x / GameManager.instance.Grid.transform.localScale.x,
            GameManager.instance.PlayerUnit.transform.position.y / GameManager.instance.Grid.transform.localScale.y,
            0);

            ///�̵� Ÿ������ ����
        if (currentAction.currentMagic == null)
        {
            

            List<PatternData.PatternPoint> pattern = movePattenData.points;

            List<Vector3Int> validPositions = movePattenData.points.Select(p =>
            {
                int x = Mathf.FloorToInt((p.x - 3) + unitPos.x);
                int y = Mathf.FloorToInt((p.y - 3) + unitPos.y);
                return new Vector3Int(x, y, 0);
            }).ToList();


            // BFS�� ����Ͽ� ������ �̵� ������ ��ġ�� Ž��
            Vector3Int closestPosition = FindClosestPositionWithPattern(new Vector3Int((int)unitPos.x, (int)unitPos.y, 0), new Vector3Int((int)playerPos.x, (int)playerPos.y, 0), pattern);
            if (closestPosition != unitPos)
            {
                this.targetPosList.Add(closestPosition);
                movePosPath = FindPathWithBFS(new Vector3Int((int)unitPos.x, (int)unitPos.y, 0), closestPosition, validPositions);
            }
            else
            {
                Debug.Log("��θ� ã�� �� �����ϴ�.");
            }
        }
        else
        {
            // �׼ǰ��� �ִٸ� �ش� �׼ǰ������� ���ݹ������� ������ �ִٴ� ���̴� Ÿ���� �Ǵ� ������� ������ üũ�ؾ��մϴ�.
            //������ ����� �߰��� üũ�ؾ��մϴ�.
            // Ÿ���� ������ ����� ���� �ڽ��� ������ ��ġ�� �������� ������ ������ �����ؾ��մϴ�.
            if(currentAction.currentMagic.MagicCount <=1 && currentAction.currentMagic.AoeType == MonsterMagic.MagicAoeType.Target)
            {
                //1�ΰ��� Ÿ���Դϴ�.
                if(currentAction.currentMagic.Type == MonsterMagic.MagicType.Attack)
                {
                    targetPosList.Add(new Vector3Int((int)playerPos.x, (int)playerPos.y,0));
                }
            }
            else if(currentAction.currentMagic.MagicCount>=1 && currentAction.currentMagic.AoeType == MonsterMagic.MagicAoeType.LocAoe)
            {
                List<PatternData.PatternPoint> pattern = currentAction.currentMagic.MagicRange.points;

                Debug.Log("ī��Ʈ ��ŭ ����Ž��");
                List<Vector3Int> validPositions = currentAction.currentMagic.MagicRange.points.Select(p =>
                {
                    int x = Mathf.FloorToInt((p.x - 3) + unitPos.x);
                    int y = Mathf.FloorToInt((p.y - 3) + unitPos.y);
                    return new Vector3Int(x, y, 0);
                }).ToList();


                // BFS�� ����Ͽ� ������ ���� ��ġ�� Ž��
                for(int i =0;i<currentAction.currentMagic.MagicCount;i++)
                {
                    Vector3Int RandomPosition = FindRandomPositionWithPattern(new Vector3Int((int)unitPos.x, (int)unitPos.y, 0), validPositions);
                    if (RandomPosition != unitPos)
                    {
                        this.targetPosList.Add(RandomPosition);
                    }
                    else
                    {
                        Debug.Log("��θ� ã�� �� �����ϴ�.");
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
                GameManager.instance.BattleZone.BattleTiles[pos.x, pos.y].onUnit == null)
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
    /// �ִܰŸ��� Ž���ϱ����� Ž�� ������ �����մϴ�.
    /// </summary>
    /// <param name="start"></���� Ž���� ������>
    /// <param name="goal"></������ �� �������� ���� Ž�� �ۿ��� Ž���ص� �Ǵ� ��ġ�Դϴ�.>
    /// <param name="pattern"></Ž���ؾ��ϴ� ����>
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
    /// �ش� ���������� �ִܰ�θ� ã���ϴ�.
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
                // ��θ� �����Ͽ� ��ȯ
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

            // Ž���� ���� ����
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

        return new List<Vector3Int>(); // ��θ� ã�� �� ���� ���
    }


    /// <summary>
    /// �̵� �׼��Դϴ�.
    /// ���� ���� �����ϴ� ��ο� ������ �ִٸ� ��Ž�� ���� �ٽ� �ش� �Լ��� �����մϴ�.
    /// </summary>
    /// 

    IEnumerator ActionMove()
    {
        yield return new WaitForSeconds(0.5f);
        if (!GameManager.instance.BattleZone.SerchTileUnit(targetPosList[0]))
        {
            Vector3Int scaledCellPos = new Vector3Int(
            Mathf.FloorToInt(targetPosList[0].x * GameManager.instance.Grid.transform.localScale.x),
            Mathf.FloorToInt(targetPosList[0].y * GameManager.instance.Grid.transform.localScale.y),
            0);
            GameManager.instance.BattleZone.removeTileUnit(transform.position, this);
            transform.position = scaledCellPos;
            GameManager.instance.BattleZone.setTileUnit(scaledCellPos, this);
            ActionCheck();
        }else
        {
            targetPosSet();
            monsterAction();
        }
        yield return null;
        yield break;

        

    }

    public override void UnitDie()
    {
        base.UnitDie();
        GameManager.instance.MonsterAIManager.MonsterRevmoe(this);
        
    }



}
