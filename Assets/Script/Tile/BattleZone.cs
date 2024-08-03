using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Tilemaps;
using UnityEngine.UIElements;

public class BattleZone : MonoBehaviour
{
    // Start is called before the first frame update
    private Tilemap tilemap; // Ÿ�ϸ� ����
    private BattleTile[,] battleTiles = new BattleTile[15,7];

    public BattleTile[,] BattleTiles { get { return battleTiles; } }

    private Vector2Int north;
    private Vector2Int south;
    private Vector2Int west;   
    private Vector2Int east;

    private void Start()
    {
        tilemap = GetComponent<Tilemap>();
        int rows = battleTiles.GetLength(0);
        int cols = battleTiles.GetLength(1);
        for(int i = 0; i<rows; i++)
        {
            for(int j = 0; j<cols; j++)
            {
                battleTiles[i, j] = new BattleTile(new Vector2Int(i-(rows/2),j-(cols/2)));
            }
        }
        battleTiles[3, 3].type = BattleTile.tileType.Break;
    }

    /// <summary>
    /// ����� �巡�� �ϰ� �ش� ��ġ�� ���� �� �ִ� ��ġ���� Ȯ���մϴ�.
    /// [�÷��̾�]�� ������ �����մϴ�. �ش� �Լ����� ������ �ʿ䰡�ֽ��ϴ�.
    /// </summary>
    /// <param name="draggable"></param>
    public void SetBlock(BlockPanel _blockPanel,Vector3Int _sellpos)
    {
        //��Ʋ�� setblock�۵�
        //2,2�� �߾Ӻ���̸� ���� ��ġ������ ���ϱ� ����� üũ�ؾ���
        bool isMove = false;
        MoveZone moveZone = GameManager.instance.MoveZone;
        
        List<PatternData.PatternPoint> pattern = _blockPanel.block.BlockInfo.Pattern;
        foreach (var pos in pattern)
        {
            //�߰����� 2,2�̱� ������ -2�� ����
            int x = pos.x-2 + _sellpos.x;
            int y = pos.y-2 + _sellpos.y;
            Vector3Int tilepos = new Vector3Int(x, y);
            if (Math.Abs(x) <= battleTiles.GetLength(0) / 2 && Math.Abs(y) <= battleTiles.GetLength(1) / 2)
            {
                x = x + battleTiles.GetLength(0)/2;
                y = y + battleTiles.GetLength(1) / 2;
                if (battleTiles[x, y].type != BattleTile.tileType.None)
                {
                    Debug.Log($"position ({x}, {y}) �ȵ�! ");
                    moveZone.breakMoveTile();
                    return;
                }
                else
                {
                    //���� ��Ų Ÿ���� ���� ��ü�� MoveZone���� �Űܾ���.
                    moveZone.enableMoveTile(tilepos);
                    if (GameManager.instance.PlayerUnit.transform.position.x == battleTiles[x, y].gridPos.x && GameManager.instance.PlayerUnit.transform.position.y == battleTiles[x, y].gridPos.y)
                    {
                        isMove = true;
                    }
                }

            }
            else
            {
                Debug.Log($"Ÿ�ϸ� �ش� ��ǥ�� {x},{y} �� Ÿ�ϸ� �ۿ� ������");
                moveZone.breakMoveTile();
                return;
            }
           
        }

        if(isMove == false)
        {
            moveZone.breakMoveTile();
            return;
        }

    }
    
}
