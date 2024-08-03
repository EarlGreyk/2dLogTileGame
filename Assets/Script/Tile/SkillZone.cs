using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using static UnityEditor.PlayerSettings;

public class SkillZone : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    private Tilemap tilemap;

    public Magic currentMagic;
    public GameObject currentMagicEffect;

    private Vector3Int hitTilePos = new Vector3Int(0,0,10);

    //���߿� �ؿ� ����Ʈ ���ս��Ѿ���.
    private List<Vector3Int> checkTilePos = new List<Vector3Int>();
    private List<int> checkTileType = new List<int>();


    /// <summary>
    /// ����Ǵ� Ÿ�� prfabs [�Ʒ�]
    /// </summary>

    [SerializeField]
    private TileBase NoneTile;
    [SerializeField]
    private TileBase UseTile;
    [SerializeField]
    private TileBase BreakTile;



    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && hitTilePos != new Vector3Int(0, 0, 10))
        {
            UseSkill();
            return;
        }
        if (Input.GetButtonDown("Cancel"))
        {
            SkillStop();
            return;
        }
        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3Int cellPosition = tilemap.WorldToCell(mouseWorldPos);

        if (tilemap.HasTile(cellPosition))
        {
            if (hitTilePos == cellPosition)
            {
                Debug.Log("���� ������ ��ȯ���Դϴ�.");
                return;
            }
            if (tilemap.GetTile(cellPosition) == BreakTile)
            {
                Debug.Log("�ش� ������ ��ų�� ��� �� �� ���� �����Դϴ�.");
                hitTilePos = new Vector3Int(0,0,10);
                ResetBlock();
                return;
            }
            if (hitTilePos == new Vector3Int(0, 0, 10))
            {
                ResetBlock();
                hitTilePos = cellPosition;
                OnMouseChangeBlock();
                return;
            }
            if (hitTilePos != cellPosition)
            {
                ResetBlock();
                hitTilePos = cellPosition;
                OnMouseChangeBlock();
                return;
            }
        }
    }

    public void SettingSkillZone(Magic magic,GameObject magicEffect)
    {
        currentMagic = magic;
        currentMagicEffect = magicEffect;

        BoundsInt bounds = tilemap.cellBounds;
        for (int x = bounds.xMin; x < bounds.xMax; x++)
        {
            for (int y = bounds.yMin; y < bounds.yMax; y++)
            {
                Vector3Int position = new Vector3Int(x, y, 0);
                CompareTile(position);
            }
        }
    }


    private void CompareTile(Vector3Int _sellpos)
    {
        //��Ʋ���� �����ϴ� ���� ���Ͽ� �ش� ��Ʋ���� �÷��̾ �ִٸ� �ش� ��ų ����� �����ؾ��մϴ�.
        int x = 0;
        int y = 0;

        
        if (Math.Abs(_sellpos.x) <= GameManager.instance.BattleZone.BattleTiles.GetLength(0) / 2 && Math.Abs(_sellpos.y) <= GameManager.instance.BattleZone.BattleTiles.GetLength(1) / 2)
        {
            x = _sellpos.x + GameManager.instance.BattleZone.BattleTiles.GetLength(0) / 2;
            y = _sellpos.y + GameManager.instance.BattleZone.BattleTiles.GetLength(1) / 2;

            if (GameManager.instance.BattleZone.BattleTiles[x, y].type == BattleTile.tileType.Break)
            {
                breakSkillTile(_sellpos);
                return;
            }

            noneSkillTile(_sellpos);


        }
    }
    /// <summary>
    /// ���� ����� �ƹ��͵� �ƴ� ���·� �����մϴ�.
    /// </summary>
    /// <param name="center"></param>
    public void noneSkillTile(Vector3Int center)
    {
        tilemap.SetTile(center, NoneTile);
    }
    /// <summary>
    /// ���� ����� ��� ������ ǥ�÷� �����մϴ�.
    /// </summary>
    /// <param name="center"></param>
    public void enableSkillTile(Vector3Int center)
    {
        tilemap.SetTile(center, UseTile);
    }
    /// <summary>
    /// ���� ����� ��� �Ұ����� ǥ�÷� �����մϴ�.
    /// </summary>
    /// <param name="center"></param>
    public void breakSkillTile(Vector3Int center)
    {
        Debug.Log($"��ǥ�� {center.x} , {center.y} �� ����� �� ���� ��ǥ");
        tilemap.SetTile(center, BreakTile);
    }
    /// <summary>
    /// ���� ���콺 ��ġ�� ���� ���� ����� �����մϴ�.
    /// </summary>
    public void OnMouseChangeBlock()
    {
        //���� �����Ϳ� ���� Block�� �����ؾ��մϴ�.
        tilemap.SetTile(hitTilePos, NoneTile);
        List<PatternData.PatternPoint> pattern = currentMagic.PatternData.points;

        int lengthX = GameManager.instance.BattleZone.BattleTiles.GetLength(0) / 2;
        int lengthY = GameManager.instance.BattleZone.BattleTiles.GetLength(1) / 2;
        foreach (var pos in pattern)
        {
            //�߰����� 2,2�̱� ������ -2�� ����
            int x = pos.x - 2 + hitTilePos.x;
            int y = pos.y - 2 + hitTilePos.y;
            Vector3Int tilepos = new Vector3Int(x, y);

            Debug.Log(tilepos);
            if (Math.Abs(x) <= lengthX && Math.Abs(y) <= lengthY)
            {
                x = x + lengthX;
                y = y + lengthY;

                if (GameManager.instance.BattleZone.BattleTiles[x, y].type == BattleTile.tileType.Break)
                {
                    Debug.Log($"position ({x}, {y}) �ȵ�! ");
                    breakSkillTile(tilepos);
                    checkTileType.Add(0);
                }
                else
                {
                    enableSkillTile(tilepos);
                    checkTileType.Add(1);
                }

               
                if(tilepos != null)
                    checkTilePos.Add(tilepos);

            }
            else
            {
                Debug.Log($"Ÿ�ϸ� �ش� ��ǥ�� {x},{y} �� Ÿ�ϸ� �ۿ� ������");
            }

        }
    }
    /// <summary>
    /// ���� ����� �ʱ�ȭ �մϴ�.
    /// </summary>
    public void ResetBlock()
    {
        for(int i =0; i<checkTilePos.Count; i++)
        {
            if (checkTileType[i] == 0)
                breakSkillTile(checkTilePos[i]);
            else
                noneSkillTile(checkTilePos[i]);
                
        }

        checkTilePos.Clear();
        checkTileType.Clear();
    }



    public void UseSkill()
    {
        GameManager.instance.PlayerActionManager.SettingSkillAction
            (currentMagic,currentMagicEffect,hitTilePos, checkTilePos);

        SkillStop();
    }
    

   /// <summary>
   /// ���� ��ų �ߵ��� ��� �ϰų� ��ų�� ���۵ɶ� ���̻� �ν����� ���ϵ��� �����ϴ�.
   /// </summary>

    public void SkillStop()
    {
        PlayerResource.instance.currentBlock = null;
        gameObject.SetActive(false);
    }

   

}
