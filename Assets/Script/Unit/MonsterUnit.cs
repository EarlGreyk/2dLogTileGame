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
    /// ���Ͱ� ���� �ൿ������ ī��Ʈ
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
    /// ���Ͱ� �̵��ϴ´뿡 �ൿ ī��Ʈ.
    /// </summary>
    private int moveCount;
    //���Ͱ� ����ϴ� �׼� ����
    [SerializeField]
    private List<MonsterAction> actionList = new List<MonsterAction>();

    //���Ͱ� ���� ����ϴ� �׼�
    private MonsterAction currentAction;

    //����ϴ� �׼��� ��ġ.
    private Vector3Int targetPos;

    private bool isAction = false;

    public bool IsAction { get { return isAction; } set { isAction = value; } }



    public override void Start()
    {
        base.Start();
        ActionCheck();
    }


 



    //actionCount �� 0�̸� �����մϴ�.
    private void monsterAction()
    {
        //�ൿ ����
        //���Ͱ� �ൿ�� �����ϱ� ���ؼ� ���� �Ŵ����� ������ �۵��� �Ѵٰ� �����մϴ�.
        isAction = true;
        GameManager.instance.addActionMonster(this);

        if (currentAction == null)
        {
            //�̵�
            ActionCheck();
            return;

        }
        else
        {
            currentAction.onAction();
            //��� �ൿ
            //���� Action���� ���� �ൿ�ϴ°��� �����ͼ� �ൿ�Ѵ��� ���������� ���Ͽ� ActionCheck�� �������մϴ�.
        }
        




        //����� �Ϸ� �Ǿ������� actionCount�� �ø��� GameManager�� �Ϸ�Ǿ��ٰ� ��ȣ�� �����ݴϴ�.
        GameManager.instance.reamoveActionMonster(this);
    }
    /// <summary>
    /// ���� ������ ����ؾ��� ������ ��ǥ�� �����մϴ�. �̴� ����ϴ� ����� ���� �ٸ��ϴ�.
    /// </summary>
    private void targetPosSet()
    {
        ///�÷��̾�� ������ �ִܰŸ��� ���Ͽ� �۵��մϴ�.
        ///������ MovePatten�� �����͸� �����´��� �� ������ �÷��̾�� ���� ����� ��ǥ�� �̵��մϴ�.
    }

    //�Ʒ��� �Լ��� �׼��� �����ϰ� �����մϴ�.

    private void ActionCheck()
    {
        //GameManager�� �ִ� �÷��̾��� ��ǥ�� ������ BattleZone���� ���Ͽ�
        //���� ���� ������ �νĹ��������� �ִ����� üũ�մϴ�.
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
    /// ������ �ൿ �˰����� �����ݴϴ�.
    /// ���ʹ� ���� �ൿ�� �̸� �˷��־�� �ϱ� ������ �ൿ�� ������ 
    /// �Լ��� �۵� ���Ѿ��մϴ�.
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
