using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MoveZone : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    private Tilemap tilemap;

    private List<Vector3Int> movePos = new List<Vector3Int>();


    private void Update()
    {
        if(Input.GetButtonDown("Cancel"))
        {
            PlayerResource.instance.currentBlock = null;
            breakMoveTile();

            gameObject.SetActive(false);

            return;
        }
        if(Input.GetMouseButtonDown(0))
        {
            Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector3Int cellPosition = tilemap.WorldToCell(mouseWorldPos);

            TileBase clickedTile = tilemap.GetTile(cellPosition);

            if (clickedTile != null)
            {
                setMovePos(cellPosition);
                
            }
            else
            {
                Debug.Log("No tile at this position.");
            }
        }
    }
    /// <summary>
    /// 타일을 변환시켜 플레이어가 이동할 수 있도록 변경합니다.
    /// </summary>

    public void enableMoveTile(Vector3Int center)
    {
        tilemap.SetTile(center, Resources.Load<Tile>("Tile/None"));
        movePos.Add(center);
    }

    public void breakMoveTile()
    {
        Debug.Log("비활성화");
        //타일 롤백.
        for (int i = 0; i < movePos.Count; i++)
        {
            tilemap.SetTile(movePos[i], null);
        }
        movePos.Clear();
        gameObject.SetActive(false);
    }
   
    //플레이어가 클릭을 하면 클릭한 좌표를 받아와 동작합니다.
    //이동을 하면 즉시 해당 클래스를 비활성화 시켜 Update의 반복을 막습니다.
    public void setMovePos(Vector3Int cellPos)
    {
        //플레이어 이동
        GameManager.instance.BattleZone.removeTileUnit(GameManager.instance.PlayerUnit.transform.position, GameManager.instance.PlayerUnit);
        GameManager.instance.PlayerUnit.transform.position = cellPos;
        GameManager.instance.BattleZone.setTileUnit(cellPos,GameManager.instance.PlayerUnit);
        

        //이동에 성공했음으로 플레이어의 블록 매니저에 접근하여 블록을 제거합니다.
        PlayerResource.instance.CurBlockRemove();
        //타일 롤백.
        for (int i = 0; i < movePos.Count; i++)
        {
            tilemap.SetTile(movePos[i], null);
        }
        movePos.Clear();


        gameObject.SetActive(false);
    }

    public void SetBlock(MovePanel _blockPanel, Vector3Int _sellpos)
    {
        //기존 블록 타일 소거.
        //배틀존 setblock작동
        //2,2가 중앙블록이며 현재 위치값에서 더하기 빼기로 체크해야함

        List<PatternData.PatternPoint> pattern = _blockPanel.block.BlockInfo.Pattern;
        int lengthX = GameManager.instance.BattleZone.BattleTiles.GetLength(0) / 2;
        int lengthY = GameManager.instance.BattleZone.BattleTiles.GetLength(1) / 2;
        foreach (var pos in pattern)
        {
            //중간값이 2,2이기 떄문에 -2씩 연산
            int x = pos.x - 2 + _sellpos.x;
            int y = pos.y - 2 + _sellpos.y;
            Vector3Int tilepos = new Vector3Int(x, y);
            if (Math.Abs(x) <= lengthX && Math.Abs(y) <= lengthY)
            {
                x = x + lengthX;
                y = y + lengthY;

                if (GameManager.instance.BattleZone.BattleTiles[x, y].type != BattleTile.tileType.Break && GameManager.instance.BattleZone.BattleTiles[x, y].onUnit == null)
                {
                    enableMoveTile(tilepos);
                }

            }

        }

    }

}
