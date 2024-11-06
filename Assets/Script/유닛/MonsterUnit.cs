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
    [SerializeField]
    private List<MonsterMagic> magicList = new List<MonsterMagic>();

    //���Ͱ� ���� ����ϴ� �׼�
    [SerializeField]
    private MonsterAction currentAction;

    //����ϴ� �׼��� ��ġ.
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


 



    //actionCount �� 0�̸� �����մϴ�.
    public void monsterAction()
    {
        //�ൿ ����
        //���Ͱ� �ൿ�� �����ϱ� ���ؼ� ���� �Ŵ����� ������ �۵��� �Ѵٰ� �����մϴ�.
        isAction = true;
        if (currentAction.currentMagic == null)
            ActionMove();
        else
            currentAction.onAction();





        //����� �Ϸ� �Ǿ������� actionCount�� �ø��� GameManager�� �Ϸ�Ǿ��ٰ� ��ȣ�� �����ݴϴ�.
    }
 
    //�Ʒ��� �Լ��� �׼��� �����ϰ� �����մϴ�.

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
    /// ������ �ൿ �˰����� �����ݴϴ�.
    /// ���ʹ� ���� �ൿ�� �̸� �˷��־�� �ϱ� ������ �ൿ�� ������ 
    /// �Լ��� �۵� ���Ѿ��մϴ�.
    /// </summary>
    private void ActionSet(bool move)
    {
        if(move)
        {
            Debug.Log("���ݹ������� �÷��̾ �����ϴ�. �̵��� �����մϴ�");
            currentAction.currentMagic = null;
            ActionCount = maxActionCount;
            //List<string> text = new List<string>() { "�̵�", ActionCount.ToString() };
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
    /// ���� ������ ����ؾ��� ������ ��ǥ�� �����մϴ�. �̴� ����ϴ� ����� ���� �ٸ��ϴ�.
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


            // BFS�� ����Ͽ� ������ �̵� ������ ��ġ�� Ž��
            Vector3Int closestPosition = FindClosestPositionWithPattern(new Vector3Int((int)unitPos.x, (int)unitPos.y, 0), new Vector3Int((int)playerPos.x, (int)playerPos.y, 0), pattern);
            if (closestPosition != unitPos)
            {
                this.targetPos = closestPosition;
                movePosPath = FindPathWithBFS(new Vector3Int((int)unitPos.x, (int)unitPos.y, 0), closestPosition, validPositions);
            }
            else
            {
                Debug.Log("��θ� ã�� �� �����ϴ�.");
            }
        }
        else
        {
            // �׼ǰ��� �ִٸ� �ش� �׼ǰ������� ���ݹ������� ������ �ִٴ� ���̴� �÷��̾��� ���� ������ �����ؾ��մϴ�.
            this.targetPos = new Vector3Int((int)playerPos.x, (int)playerPos.y,0);
        }
    }


    /// <summary>
    /// �ִܰŸ��� ã���ϴ�.
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
    /// �ش� ���������� ��θ� ã���ϴ�.
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
