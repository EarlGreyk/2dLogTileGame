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
    private Tilemap tilemap; // 타일맵 참조
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
    /// 블록을 드래그 하고 해당 위치가 놓을 수 있는 위치인지 확인합니다.
    /// [플레이어]만 접근이 가능합니다. 해당 함수명을 변경할 필요가있습니다.
    /// </summary>
    /// <param name="draggable"></param>
    public void SetBlock(BlockPanel _blockPanel,Vector3Int _sellpos)
    {
        //배틀존 setblock작동
        //2,2가 중앙블록이며 현재 위치값에서 더하기 빼기로 체크해야함
        bool isMove = false;
        MoveZone moveZone = GameManager.instance.MoveZone;
        
        List<PatternData.PatternPoint> pattern = _blockPanel.block.BlockInfo.Pattern;
        foreach (var pos in pattern)
        {
            //중간값이 2,2이기 떄문에 -2씩 연산
            int x = pos.x-2 + _sellpos.x;
            int y = pos.y-2 + _sellpos.y;
            Vector3Int tilepos = new Vector3Int(x, y);
            if (Math.Abs(x) <= battleTiles.GetLength(0) / 2 && Math.Abs(y) <= battleTiles.GetLength(1) / 2)
            {
                x = x + battleTiles.GetLength(0)/2;
                y = y + battleTiles.GetLength(1) / 2;
                if (battleTiles[x, y].type != BattleTile.tileType.None)
                {
                    Debug.Log($"position ({x}, {y}) 안됨! ");
                    moveZone.breakMoveTile();
                    return;
                }
                else
                {
                    //변경 시킨 타일을 하위 객체인 MoveZone으로 옮겨야함.
                    moveZone.enableMoveTile(tilepos);
                    if (GameManager.instance.PlayerUnit.transform.position.x == battleTiles[x, y].gridPos.x && GameManager.instance.PlayerUnit.transform.position.y == battleTiles[x, y].gridPos.y)
                    {
                        isMove = true;
                    }
                }

            }
            else
            {
                Debug.Log($"타일맵 해당 좌표값 {x},{y} 은 타일맵 밖에 존재함");
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
