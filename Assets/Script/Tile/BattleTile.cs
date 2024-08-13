using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class BattleTile : TileBase
{
    public enum tileType
    {
        None,
        Break
    }
    public tileType type;
    public bool ontileObject;

    public Tile originTile;
    public Tile changeTile;
    public Vector3Int gridPos;
    public Unit onUnit;
    public BattleTile(Vector3Int _gridPos)
    {
        type = tileType.None;
        ontileObject = false;
        changeTile = Resources.Load<Tile>("Tile/None");
        gridPos = _gridPos;
    }

}
