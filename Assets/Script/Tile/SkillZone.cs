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
    private Grid grid;
    [SerializeField]
    private Tilemap tilemap;
    public Tilemap Tilemap { get { return tilemap; } }

    public Magic currentMagic;
    public GameObject currentMagicEffect;

    private Vector3Int hitTilePos = new Vector3Int(0,0,10);

    //���߿� �ؿ� ����Ʈ ���ս��Ѿ���.
    private List<Vector3Int> checkTilePos = new List<Vector3Int>();
    private List<int> checkTileType = new List<int>();

    private List<Vector3Int> checkTileRange = new List<Vector3Int>();
    


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
        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3Int cellPosition = tilemap.WorldToCell(mouseWorldPos);
        if (Input.GetMouseButtonDown(0) && tilemap.HasTile(cellPosition))
        {
            UseSkill();
            return;
        }
        if (Input.GetButtonDown("Cancel"))
        {
            SkillStop();
            return;
        }

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
            if (hitTilePos == new Vector3Int(0, 0, 10) && checkTileRange.Contains(cellPosition))
            {
                ResetBlock();
                hitTilePos = cellPosition;
                OnMouseChangeBlock();
                return;
            }
            if (hitTilePos != cellPosition && checkTileRange.Contains(cellPosition))
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
        CameraSetting.instance.blockModeOff();

        // Grid�� �� ũ�� (������)
        Vector3 scale = grid.transform.localScale;

        currentMagic = magic;
        currentMagicEffect = magicEffect;

        List<PatternData.PatternPoint> pattern = magic.MagicRange.points;
        int lengthX = GameManager.instance.BattleZone.BattleTiles.GetLength(0);
        int lengthY = GameManager.instance.BattleZone.BattleTiles.GetLength(1);
        int unitx = Mathf.FloorToInt(GameManager.instance.PlayerUnit.transform.position.x );
        int unity = Mathf.FloorToInt(GameManager.instance.PlayerUnit.transform.position.y );
        Vector3Int unitPos = new Vector3Int(unitx, unity);
        foreach (var pos in pattern)
        {
            //�߰����� 2,2�̱� ������ -2�� ����
            int x = Mathf.FloorToInt((pos.x - 2) + unitPos.x / scale.x) ;
            int y = Mathf.FloorToInt((pos.y - 2) + unitPos.y / scale.y) ;
            Vector3Int tilepos = new Vector3Int(x, y);
            if (x < lengthX && y < lengthY)
            {
                if (GameManager.instance.BattleZone.BattleTiles[x, y].type == BattleTile.tileType.Break)
                {
                    breakSkillTile(tilepos);
                }else
                {
                    noneSkillTile(tilepos);
                    checkTileRange.Add(tilepos);
                }
                

            }
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
        List<PatternData.PatternPoint> pattern = currentMagic.MagicAoe.points;

        int lengthX = GameManager.instance.BattleZone.BattleTiles.GetLength(0);
        int lengthY = GameManager.instance.BattleZone.BattleTiles.GetLength(1);
        foreach (var pos in pattern)
        {
            //�߰����� 2,2�̱� ������ -2�� ����
            int x = pos.x - 2 + hitTilePos.x;
            int y = pos.y - 2 + hitTilePos.y;
            Vector3Int tilepos = new Vector3Int(x, y);

            if (Math.Abs(x) <= lengthX && Math.Abs(y) <= lengthY)
            {
                if (GameManager.instance.BattleZone.BattleTiles[x, y].type == BattleTile.tileType.Break )
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
            {
                breakSkillTile(checkTilePos[i]);
            }
            if(checkTileType[i] == 1 && checkTileRange.Contains(checkTilePos[i]))
            {
                noneSkillTile(checkTilePos[i]);
            }
            else
            {
                tilemap.SetTile(checkTilePos[i], null);
            }
                
        }

        checkTilePos.Clear();
        checkTileType.Clear();
    }



    public void UseSkill()
    {

        GameManager.instance.PlayerActionManager.SettingSkillAction
            (currentMagic,currentMagicEffect,hitTilePos, checkTilePos);

        GameManager.instance.LampUpdate(1);
        SkillStop();
    }
    

   /// <summary>
   /// ���� ��ų �ߵ��� ��� �ϰų� ��ų�� ���۵ɶ� ���̻� �ν����� ���ϵ��� �����ϴ�.
   /// </summary>

    public void SkillStop()
    {
        PlayerResource.instance.currentBlock = null;
        for(int i =0; i<checkTileRange.Count; i ++)
        {
            tilemap.SetTile(checkTileRange[i], null);
        }
        for(int i =0; i<checkTilePos.Count; i++)
        {
            tilemap.SetTile(checkTilePos[i], null);
        }
        checkTileRange.Clear();
        checkTilePos.Clear();
        checkTileType.Clear();
        gameObject.SetActive(false);
    }

   

}
