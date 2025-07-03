using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Tilemaps;
using UnityEngine.UIElements;
using static UnityEngine.UI.CanvasScaler;

public class BattleZone : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    private Grid grid;
    private Tilemap tilemap; // 타일맵 참조
    public Tilemap Tilemap {  get { return tilemap; } }
    private BattleTile[,] battleTiles ;

    public BattleTile[,] BattleTiles { get { return battleTiles; } }

    private Vector2Int north;
    private Vector2Int south;
    private Vector2Int west;   
    private Vector2Int east;

    [SerializeField]
    private Sprite breakTileSprite;
    [SerializeField]
    private Sprite playerSponeTileSprite;
    [SerializeField]
    private Sprite monsterSponeTileSprite;

    private Vector3Int playerSponePos;
    public Vector3Int PlayerSponePos { get { return playerSponePos; } }


    private List<Vector3Int> monsterSponePosList = new List<Vector3Int>();
    public List<Vector3Int> MonsterSponePosList { get { return monsterSponePosList; } }

    private void Awake()
    {
        grid = GameManager.instance.Grid;
        //타일맵이 변경 될 수 있음으로 타일맵의 크기를 조정해줍니다.
        tilemap = GetComponent<Tilemap>();
        Vector3 scale = grid.transform.localScale;

        // 그리드의 스케일 가져오기
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

        BoundsInt bounds = tilemap.cellBounds;
        TileBase[] allTiles = tilemap.GetTilesBlock(bounds);

        int rows = bouns.size.x;
        int cols = bouns.size.y;

        battleTiles = new BattleTile[rows, cols];

        for (int i = 0; i<rows; i++)
        {
            for(int j = 0; j<cols; j++)
            {
                Vector3Int gridPos = new Vector3Int(
                Mathf.FloorToInt((i ) * scale.x),Mathf.FloorToInt((j ) * scale.y),0);
                TileBase tile = allTiles[i + j * rows];
                battleTiles[i, j] = new BattleTile(gridPos);
                if (tile != null)
                {
                    Sprite sprite = ((Tile)tile).sprite;

                    if (sprite != null && sprite == breakTileSprite)
                    {
                        battleTiles[i, j].type = BattleTile.tileType.Break;
                        
                    }
                    if (sprite != null && sprite == playerSponeTileSprite)
                    {
                        playerSponePos = battleTiles[i, j].gridPos;
                    }
                    if (sprite != null && sprite == monsterSponeTileSprite)
                    {
                        monsterSponePosList.Add(battleTiles[i, j].gridPos);
                    }else
                    {
                        
                    }
                }
                else
                {
                }



            }
        }
    }
    /// <summary>
    /// 유닛이 생성되거나 유닛이 이동될때 BattleZone에 해당 유닛을 넣어줍니다.
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>

    public void setTileUnit(Vector3 pos,Unit unit)
    {
        Vector3 scale = grid.transform.localScale;
        Vector3Int unitPos = new Vector3Int((int)pos.x, (int)pos.y, 0);

        
        int x = Mathf.FloorToInt(unitPos.x / scale.x);
        int y = Mathf.FloorToInt(unitPos.y / scale.y);
        if (battleTiles[x, y].onUnit == null)
            battleTiles[x, y].onUnit = unit;


        setTempTile(pos);

    }
    /// <summary>
    /// 유닛이 파괴되거나 유닛이 이동될때 BattleZone에 해당 유닛을 제거합니다.
    /// 유닛이 파괴되거나 이동을 완료되었음으로 해당 유닛의 가이동처리를 제거해야합니다.
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    public void removeTileUnit(Vector3 pos,Unit unit)
    {
        Vector3 scale = grid.transform.localScale;
        Vector3Int unitPos = new Vector3Int((int)pos.x, (int)pos.y, 0);


        int x = Mathf.FloorToInt(unitPos.x / scale.x);
        int y = Mathf.FloorToInt(unitPos.y / scale.y);
        if (battleTiles[x, y].onUnit == unit)
            battleTiles[x, y].onUnit = null;


        removeTempTile(pos);

    }
    public void setTempTile(Vector3 pos, bool b = true)
    {
        Vector3 scale = grid.transform.localScale;
        Vector3Int unitPos = new Vector3Int((int)pos.x, (int)pos.y, 0);

        int x = unitPos.x;
        int y = unitPos.y;

        if(b)
        {
            x = Mathf.FloorToInt(unitPos.x / scale.x);
            y = Mathf.FloorToInt(unitPos.y / scale.y);
        }

        
        battleTiles[x, y].tempTile = true;
        //DebugTempTest();
    }

    public void removeTempTile(Vector3 pos, bool b = true)
    {
        Vector3 scale = grid.transform.localScale;
        Vector3Int unitPos = new Vector3Int((int)pos.x, (int)pos.y, 0);


        int x = unitPos.x;
        int y = unitPos.y;

        if (b)
        {
            x = Mathf.FloorToInt(unitPos.x / scale.x);
            y = Mathf.FloorToInt(unitPos.y / scale.y);
        }


        battleTiles[x, y].tempTile = false;
        //DebugTempTest();
    }





    public Unit SerchTileUnit(Vector3 pos)
    {
        Unit serchUnit = null;
        Vector3 scale = grid.transform.localScale;
        Vector3Int unitPos = new Vector3Int((int)pos.x, (int)pos.y, 0);


        int x = Mathf.FloorToInt(unitPos.x / scale.x);
        int y = Mathf.FloorToInt(unitPos.y / scale.y);

        if (battleTiles[x, y].onUnit != null) 
            serchUnit = battleTiles[x, y].onUnit;


        return serchUnit;
    }

    public Unit SerchTileUnit(Vector3Int pos)
    {
        Vector3 scale = grid.transform.localScale;
        Unit serchUnit = null;


        int x = Mathf.FloorToInt(pos.x / scale.x);
        int y = Mathf.FloorToInt(pos.y / scale.y);

        if (battleTiles[x, y].onUnit != null)
            serchUnit = battleTiles[x, y].onUnit;


        return serchUnit;
    }






    private void DebugUnitTest()
    {

        BattleTile[,] Tiles = battleTiles;

       // Tiles = Rotate90Clockwise(Tiles);

        string s = "";
        for (int x = 0; x < Tiles.GetLength(0); x++)
        {
            s += "\n";
            for (int y = 0;y < Tiles.GetLength(1); y++)
            {
                if(Tiles[x, y].onUnit == null)
                {
                    if(battleTiles[x, y].type == BattleTile.tileType.Break)
                    {
                        s += "■";
                    }
                    else
                    {
                        s += "□";
                    }
                    
                }else
                {
                    s += "▦";
                }

                
            }
        }




        Debug.Log(s);
    }

    private void DebugTempTest()
    {

        BattleTile[,] Tiles = battleTiles;

        // Tiles = Rotate90Clockwise(Tiles);

        string s = "";
        for (int x = 0; x < Tiles.GetLength(0); x++)
        {
            s += "\n";
            for (int y = 0; y < Tiles.GetLength(1); y++)
            {
                if (Tiles[x, y].tempTile == false)
                {
                    if (battleTiles[x, y].type == BattleTile.tileType.Break)
                    {
                        s += "■";
                    }
                    else
                    {
                        s += "□";
                    }

                }
                else
                {
                    s += "▣";
                }


            }
        }




        Debug.Log(s);
    }

    BattleTile[,] Rotate90Clockwise(BattleTile[,] input)
    {
        int rows = input.GetLength(0);
        int cols = input.GetLength(1);

        BattleTile[,] rotated = new BattleTile[cols, rows];

       

        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < cols; j++)
            {
                rotated[cols - 1 - j, i] = input[i, j];
            }
        }

        return rotated;
    }





}
