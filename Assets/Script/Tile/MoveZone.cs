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
    /// Ÿ���� ��ȯ���� �÷��̾ �̵��� �� �ֵ��� �����մϴ�.
    /// </summary>

    public void enableMoveTile(Vector3Int center)
    {
        tilemap.SetTile(center, Resources.Load<Tile>("Tile/None"));
        movePos.Add(center);
    }

    public void breakMoveTile()
    {
        Debug.Log("��Ȱ��ȭ");
        //Ÿ�� �ѹ�.
        for (int i = 0; i < movePos.Count; i++)
        {
            tilemap.SetTile(movePos[i], null);
        }
        movePos.Clear();
        gameObject.SetActive(false);
    }
   
    //�÷��̾ Ŭ���� �ϸ� Ŭ���� ��ǥ�� �޾ƿ� �����մϴ�.
    //�̵��� �ϸ� ��� �ش� Ŭ������ ��Ȱ��ȭ ���� Update�� �ݺ��� �����ϴ�.
    public void setMovePos(Vector3Int cellPos)
    {
        //�÷��̾� �̵�
        GameManager.instance.BattleZone.removeTileUnit(GameManager.instance.PlayerUnit.transform.position, GameManager.instance.PlayerUnit);
        GameManager.instance.PlayerUnit.transform.position = cellPos;
        GameManager.instance.BattleZone.setTileUnit(cellPos,GameManager.instance.PlayerUnit);
        

        //�̵��� ������������ �÷��̾��� ��� �Ŵ����� �����Ͽ� ����� �����մϴ�.
        PlayerResource.instance.CurBlockRemove();
        //Ÿ�� �ѹ�.
        for (int i = 0; i < movePos.Count; i++)
        {
            tilemap.SetTile(movePos[i], null);
        }
        movePos.Clear();


        gameObject.SetActive(false);
    }

    public void SetBlock(MovePanel _blockPanel, Vector3Int _sellpos)
    {
        //���� ��� Ÿ�� �Ұ�.
        //��Ʋ�� setblock�۵�
        //2,2�� �߾Ӻ���̸� ���� ��ġ������ ���ϱ� ����� üũ�ؾ���

        List<PatternData.PatternPoint> pattern = _blockPanel.block.BlockInfo.Pattern;
        int lengthX = GameManager.instance.BattleZone.BattleTiles.GetLength(0) / 2;
        int lengthY = GameManager.instance.BattleZone.BattleTiles.GetLength(1) / 2;
        foreach (var pos in pattern)
        {
            //�߰����� 2,2�̱� ������ -2�� ����
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
