using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.Tilemaps;
using static UnityEditor.PlayerSettings;

public class UnitSpawner : MonoBehaviour
{
    [SerializeField]
    private Grid grid;
    [SerializeField]
    private Tilemap UnitMap;

    public void SpawnUnit(Vector3Int tilePosition,GameObject unitPrefab)
    {
        Debug.Log($"À¯´Ö »ý¼º ÁÂÇ¥ : {tilePosition}");
       
        Vector3 scale = grid.transform.localScale;
        // Å¸ÀÏ¸ÊÀÇ Å¸ÀÏ ÁÂÇ¥¸¦ ¿ùµå ÁÂÇ¥·Î º¯È¯
        Vector3 worldPosition = UnitMap.CellToWorld(tilePosition);
        int x = Mathf.FloorToInt(worldPosition.x / scale.x);
        int y = Mathf.FloorToInt(worldPosition.y / scale.y);
        Vector3Int unitPos = new Vector3Int(x, y, 0);

        // À¯´Ö »ý¼º
        GameObject unit = Instantiate(unitPrefab, unitPos, Quaternion.identity);
        unit.transform.SetParent(UnitMap.transform);

    }
    public PlayerUnit SpawnPlayer(Vector3Int tilePosition, GameObject unitPrefab)
    {
        Vector3 scale = grid.transform.localScale;
        // Å¸ÀÏ¸ÊÀÇ Å¸ÀÏ ÁÂÇ¥¸¦ ¿ùµå ÁÂÇ¥·Î º¯È¯
        Vector3 worldPosition = UnitMap.CellToWorld(tilePosition);
        int x = Mathf.FloorToInt(worldPosition.x / scale.x);
        int y = Mathf.FloorToInt(worldPosition.y / scale.y);
        Vector3Int unitPos = new Vector3Int(x, y, 0);

        // À¯´Ö »ý¼º
        GameObject unit = Instantiate(unitPrefab, unitPos, Quaternion.identity);
        unit.transform.SetParent(UnitMap.transform);
        PlayerUnit player = unit.GetComponent<PlayerUnit>();
        //GameManager.instance.BattleZone.setTileUnit(tilePosition, player);
        return player;
    }

    public MonsterUnit SpawnMonster(Vector3Int tilePosition, GameObject unitPrefab)
    {
        Vector3 worldPosition = UnitMap.CellToWorld(tilePosition);
        Debug.Log($"À¯´Ö »ý¼º ÁÂÇ¥ : {tilePosition}      :   º¯È¯ ÁÂÇ¥ : {worldPosition}");
        // À¯´Ö »ý¼º
        GameObject unit = Instantiate(unitPrefab, worldPosition, Quaternion.identity);
        unit.transform.SetParent(UnitMap.transform);
        MonsterUnit monster = unit.GetComponent<MonsterUnit>();
        GameManager.instance.MonsterAIManager.MonsterSet(monster);
        GameManager.instance.BattleZone.setTileUnit(tilePosition, monster);
        return monster;
    }



    public Vector3 PosUnitSet(Vector3Int vector3)
    {
        Vector3 scale = grid.transform.localScale;
        // Å¸ÀÏ¸ÊÀÇ Å¸ÀÏ ÁÂÇ¥¸¦ ¿ùµå ÁÂÇ¥·Î º¯È¯
        Vector3 worldPosition = UnitMap.CellToWorld(vector3);
        int x = Mathf.FloorToInt(worldPosition.x / scale.x);
        int y = Mathf.FloorToInt(worldPosition.y / scale.y);

        Debug.Log($"{x} : {y}");
        Vector3Int unitPos = new Vector3Int(x, y, 0);


        return unitPos;
    }
    
}
