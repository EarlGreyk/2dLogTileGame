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
    /// 유닛이 생성되거나 유닛이 이동될때 BattleZone에 해당 유닛을 넣어줍니다.
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>

    public void setTileUnit(Vector3 pos,Unit unit)
    {
        Vector3Int unitPos = new Vector3Int((int)transform.position.x, (int)transform.position.y, 0);
        int x = unitPos.x - 2;
        int y = unitPos.y - 2;

        battleTiles[x, y].onUnit = unit;
    }
    public void removeTileUnit(Vector3 pos,Unit unit)
    {
        Vector3Int unitPos = new Vector3Int((int)transform.position.x, (int)transform.position.y, 0);
        int x = unitPos.x - 2;
        int y = unitPos.y - 2;
        if (battleTiles[x, y].onUnit == unit)
            battleTiles[x, y].onUnit = null;
    }

    
}
