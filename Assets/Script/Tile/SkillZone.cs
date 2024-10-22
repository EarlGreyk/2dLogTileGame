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

    //나중에 밑에 리스트 통합시켜야함.
    private List<Vector3Int> checkTilePos = new List<Vector3Int>();
    private List<int> checkTileType = new List<int>();

    private List<Vector3Int> checkTileRange = new List<Vector3Int>();
    


    /// <summary>
    /// 변경되는 타일 prfabs [아래]
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
                Debug.Log("같은 지역을 반환중입니다.");
                return;
            }
            if (tilemap.GetTile(cellPosition) == BreakTile)
            {
                Debug.Log("해당 구역은 스킬을 사용 할 수 없는 지역입니다.");
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

        // Grid의 셀 크기 (스케일)
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
            //중간값이 2,2이기 떄문에 -2씩 연산
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
    /// 가상 블록을 아무것도 아닌 상태로 변경합니다.
    /// </summary>
    /// <param name="center"></param>
    public void noneSkillTile(Vector3Int center)
    {
        tilemap.SetTile(center, NoneTile);
    }
    /// <summary>
    /// 가상 블록을 사용 가능한 표시로 변경합니다.
    /// </summary>
    /// <param name="center"></param>
    public void enableSkillTile(Vector3Int center)
    {
        tilemap.SetTile(center, UseTile);
    }
    /// <summary>
    /// 가상 블록을 사용 불가능한 표시로 변경합니다.
    /// </summary>
    /// <param name="center"></param>
    public void breakSkillTile(Vector3Int center)
    {
        Debug.Log($"좌표값 {center.x} , {center.y} 은 사용할 수 없는 좌표");
        tilemap.SetTile(center, BreakTile);
    }
    /// <summary>
    /// 현재 마우스 위치에 따라 가상 블록을 변경합니다.
    /// </summary>
    public void OnMouseChangeBlock()
    {

        //패턴 데이터에 따라 Block을 수정해야합니다.
        tilemap.SetTile(hitTilePos, NoneTile);
        List<PatternData.PatternPoint> pattern = currentMagic.MagicAoe.points;

        int lengthX = GameManager.instance.BattleZone.BattleTiles.GetLength(0);
        int lengthY = GameManager.instance.BattleZone.BattleTiles.GetLength(1);
        foreach (var pos in pattern)
        {
            //중간값이 2,2이기 떄문에 -2씩 연산
            int x = pos.x - 2 + hitTilePos.x;
            int y = pos.y - 2 + hitTilePos.y;
            Vector3Int tilepos = new Vector3Int(x, y);

            if (Math.Abs(x) <= lengthX && Math.Abs(y) <= lengthY)
            {
                if (GameManager.instance.BattleZone.BattleTiles[x, y].type == BattleTile.tileType.Break )
                {
                    Debug.Log($"position ({x}, {y}) 안됨! ");
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
                Debug.Log($"타일맵 해당 좌표값 {x},{y} 은 타일맵 밖에 존재함");
            }

        }
    }
    /// <summary>
    /// 가상 블록을 초기화 합니다.
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
   /// 현재 스킬 발동을 취소 하거나 스킬이 동작될때 더이상 인식하지 못하도록 막습니다.
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
