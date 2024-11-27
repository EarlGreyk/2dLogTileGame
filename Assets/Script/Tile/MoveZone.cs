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
    private Grid grid;
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

    public void breakMoveTile(bool istrue = false)
    {
        //타일 롤백.
        for (int i = 0; i < movePos.Count; i++)
        {
            tilemap.SetTile(movePos[i], null);
        }
        movePos.Clear();
        if(istrue!=true)
            gameObject.SetActive(false);
    }
   
    //플레이어가 클릭을 하면 클릭한 좌표를 받아와 동작합니다.
    //이동을 하면 즉시 해당 클래스를 비활성화 시켜 Update의 반복을 막습니다.
    public void setMovePos(Vector3Int cellPos)
    {

        // Grid의 셀 크기 (스케일)
        Vector3 scale = grid.transform.localScale;
        //현재 플레이어 위치 잡기.
        Vector3 currentPos = GameManager.instance.PlayerUnit.transform.position;
        // 스케일을 고려한 셀 위치 계산
        Vector3Int scaledCellPos = new Vector3Int(
            Mathf.FloorToInt(cellPos.x * scale.x),
            Mathf.FloorToInt(cellPos.y * scale.y),
            0);
        Debug.Log($"유닛의 현재 위치 : {currentPos} , 이동해야하는 셀위치 고려값 : {scaledCellPos}");
        //플레이어 이동
        GameManager.instance.BattleZone.removeTileUnit(currentPos, GameManager.instance.PlayerUnit);
        GameManager.instance.PlayerUnit.transform.position = scaledCellPos;
        GameManager.instance.BattleZone.setTileUnit(scaledCellPos, GameManager.instance.PlayerUnit);
        

        //이동에 성공했음으로 플레이어의 블록 매니저에 접근하여 블록을 제거합니다.
        PlayerResource.instance.CurBlockRemove();
        //타일 롤백.
        for (int i = 0; i < movePos.Count; i++)
        {
            Vector3Int rollbackPos = new Vector3Int(
            Mathf.FloorToInt(movePos[i].x ),
            Mathf.FloorToInt(movePos[i].y ),
            0
             );
            tilemap.SetTile(rollbackPos, null);
        }
        movePos.Clear();

        GameManager.instance.LampUpdate(1);
        gameObject.SetActive(false);
    }

    public void SetBlock(MovePanel _blockPanel, Vector3Int _sellpos)
    {
        CameraSetting.instance.blockModeOff();
        //기존 블록 타일 소거.
        breakMoveTile(true);
        //배틀존 setblock작동
        //2,2가 중앙블록이며 현재 위치값에서 더하기 빼기로 체크해야함
        Vector3 scale = grid.transform.localScale;

       
        List<PatternData.PatternPoint> pattern = _blockPanel.block.BlockInfo.Pattern;
        foreach (var pos in pattern)
        {
            //중간값이 2,2이기 떄문에 -2씩 연산
            int x = Mathf.FloorToInt((pos.x - 3)  + _sellpos.x / scale.x);
            int y = Mathf.FloorToInt((pos.y - 3)  + _sellpos.y / scale.y);
            Vector3Int tilepos = new Vector3Int(x, y, 0);

            if (x < GameManager.instance.BattleZone.BattleTiles.GetLength(0) && y < GameManager.instance.BattleZone.BattleTiles.GetLength(1) && x>=0 && y>=0)
            {
                if (GameManager.instance.BattleZone.BattleTiles[x, y].type != BattleTile.tileType.Break && GameManager.instance.BattleZone.BattleTiles[x, y].onUnit == null)
                {
                    enableMoveTile(tilepos);
                }
            }
           

        }

    }

}
