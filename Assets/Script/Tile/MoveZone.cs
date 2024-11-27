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
    /// Ÿ���� ��ȯ���� �÷��̾ �̵��� �� �ֵ��� �����մϴ�.
    /// </summary>

    public void enableMoveTile(Vector3Int center)
    {
        tilemap.SetTile(center, Resources.Load<Tile>("Tile/None"));
        movePos.Add(center);
    }

    public void breakMoveTile(bool istrue = false)
    {
        //Ÿ�� �ѹ�.
        for (int i = 0; i < movePos.Count; i++)
        {
            tilemap.SetTile(movePos[i], null);
        }
        movePos.Clear();
        if(istrue!=true)
            gameObject.SetActive(false);
    }
   
    //�÷��̾ Ŭ���� �ϸ� Ŭ���� ��ǥ�� �޾ƿ� �����մϴ�.
    //�̵��� �ϸ� ��� �ش� Ŭ������ ��Ȱ��ȭ ���� Update�� �ݺ��� �����ϴ�.
    public void setMovePos(Vector3Int cellPos)
    {

        // Grid�� �� ũ�� (������)
        Vector3 scale = grid.transform.localScale;
        //���� �÷��̾� ��ġ ���.
        Vector3 currentPos = GameManager.instance.PlayerUnit.transform.position;
        // �������� ����� �� ��ġ ���
        Vector3Int scaledCellPos = new Vector3Int(
            Mathf.FloorToInt(cellPos.x * scale.x),
            Mathf.FloorToInt(cellPos.y * scale.y),
            0);
        Debug.Log($"������ ���� ��ġ : {currentPos} , �̵��ؾ��ϴ� ����ġ ����� : {scaledCellPos}");
        //�÷��̾� �̵�
        GameManager.instance.BattleZone.removeTileUnit(currentPos, GameManager.instance.PlayerUnit);
        GameManager.instance.PlayerUnit.transform.position = scaledCellPos;
        GameManager.instance.BattleZone.setTileUnit(scaledCellPos, GameManager.instance.PlayerUnit);
        

        //�̵��� ������������ �÷��̾��� ��� �Ŵ����� �����Ͽ� ����� �����մϴ�.
        PlayerResource.instance.CurBlockRemove();
        //Ÿ�� �ѹ�.
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
        //���� ��� Ÿ�� �Ұ�.
        breakMoveTile(true);
        //��Ʋ�� setblock�۵�
        //2,2�� �߾Ӻ���̸� ���� ��ġ������ ���ϱ� ����� üũ�ؾ���
        Vector3 scale = grid.transform.localScale;

       
        List<PatternData.PatternPoint> pattern = _blockPanel.block.BlockInfo.Pattern;
        foreach (var pos in pattern)
        {
            //�߰����� 2,2�̱� ������ -2�� ����
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
