using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

public class BlockModeZone : MonoBehaviour
{
    [SerializeField]
    private Grid grid;
    [SerializeField]
    private Tilemap tilemap;

    /// <summary>
    /// 해당 유닛의 블록을 보여줍니다.
    /// </summary>
    /// 
    public void ModeSetting(bool value)
    {
        if(value)
        {
            gameObject.SetActive(true);
            for (int i = 0; i < GameManager.instance.MonsterAIManager.Monsters.Count; i++)
            {
                unitBlockSet(GameManager.instance.MonsterAIManager.Monsters[i]);
            }
        }
        else 
        {
            gameObject.SetActive(true);
            tilemap.ClearAllTiles();
        }
        
    }
    public void unitBlockSet(MonsterUnit unit)
    {
        Debug.Log("유닛 블록 경로 활성화 하기전 사전에 보여주고 있는 타일을 제거합니다.");
        breakTile();

        if(unit.CurrentAcion.currentMagic == null)
        {
            if(unit.MovePosPath.Count>0)
            {
                for (int i = 0; i < unit.MovePosPath.Count; i++)
                {
                    enableTile(unit.MovePosPath[i]);
                }
                enableTile(unit.MovePosPath[unit.MovePosPath.Count - 1], 1);
            }
        }else
        {
            List<Vector3Int> targetAoePositions = unit.CurrentAcion.currentMagic.MagicAoe.points.Select(p =>
            {
                int x = Mathf.FloorToInt((p.x - 3) + unit.TargetPosList[0].x);
                int y = Mathf.FloorToInt((p.y - 3) + unit.TargetPosList[0].y);
                return new Vector3Int(x, y, 0);
            }).ToList();

            for (int i = 0; i < targetAoePositions.Count; i++)
            {
                enableTile(targetAoePositions[i], 1);
            }
            
        }
        
        

    }

    public void enableTile(Vector3Int center, int value = 0)
    {
        if(value != 0)
        {
            tilemap.SetTile(center, Resources.Load<Tile>("Tile/BreakTile"));
            return;
        }
            

        tilemap.SetTile(center, Resources.Load<Tile>("Tile/None"));
    }
    public void breakTile()
    {
        tilemap.ClearAllTiles();

    }
}
