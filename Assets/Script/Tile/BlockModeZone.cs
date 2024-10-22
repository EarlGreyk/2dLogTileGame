using System.Collections;
using System.Collections.Generic;
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
        for(int i = 0; i < unit.MovePosPath.Count; i++)
        {
            enableTile(unit.MovePosPath[i]);
        }
        enableTile(unit.MovePosPath[unit.MovePosPath.Count - 1],1);
        

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
