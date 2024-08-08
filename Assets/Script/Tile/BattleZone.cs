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
        //타일맵이 변경 될 수 있음으로 타일맵의 크기를 조정해줍니다.
        tilemap = GetComponent<Tilemap>();
        Vector3Int min = new Vector3Int(int.MaxValue, int.MaxValue, int.MaxValue);
        Vector3Int max = new Vector3Int(int.MinValue, int.MinValue, int.MinValue);

        foreach (Vector3Int pos in tilemap.cellBounds.allPositionsWithin)
        {
            if (tilemap.HasTile(pos))
            {
                // 최소 좌표 갱신
                if (pos.x < min.x) min.x = pos.x;
                if (pos.y < min.y) min.y = pos.y;
                if (pos.z < min.z) min.z = pos.z;

                // 최대 좌표 갱신
                if (pos.x > max.x) max.x = pos.x;
                if (pos.y > max.y) max.y = pos.y;
                if (pos.z > max.z) max.z = pos.z;
            }
        }

        // 계산된 최소/최대 좌표로 BoundsInt 생성
        BoundsInt bouns = new BoundsInt(min, max - min + Vector3Int.one);
        tilemap.size = bouns.size;

        int rows = bouns.size.x;
        int cols = bouns.size.y;
        battleTiles = new BattleTile[rows, cols];
        Debug.Log($"{rows},{cols}");
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
        Vector3Int unitPos = new Vector3Int((int)pos.x, (int)pos.y, 0);

        Debug.Log(unitPos);

        int lengthX = battleTiles.GetLength(0) / 2;
        int lengthY = battleTiles.GetLength(1) / 2;
        int x = unitPos.x  + lengthX;
        int y = unitPos.y + lengthY;

        Debug.Log($"{unit.gameObject.name}의 배열 좌표값 : {x} {y}");
        if (battleTiles[x, y].onUnit == null)
            battleTiles[x, y].onUnit = unit;

    }
    public void removeTileUnit(Vector3 pos,Unit unit)
    {
        Vector3Int unitPos = new Vector3Int((int)pos.x, (int)pos.y, 0);

        Debug.Log(unitPos);

        int lengthX = battleTiles.GetLength(0) / 2;
        int lengthY = battleTiles.GetLength(1) / 2;
        int x = unitPos.x + lengthX;
        int y = unitPos.y + lengthY;
        if (battleTiles[x, y].onUnit == unit)
            battleTiles[x, y].onUnit = null;
    }

    
}
