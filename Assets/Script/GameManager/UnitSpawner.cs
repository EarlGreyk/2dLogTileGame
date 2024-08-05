using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class UnitSpawner : MonoBehaviour
{
    [SerializeField]
    private Tilemap UnitMap;

    public void SpawnUnit(Vector3Int tilePosition,GameObject unitPrefab)
    {
        // Å¸ÀÏ¸ÊÀÇ Å¸ÀÏ ÁÂÇ¥¸¦ ¿ùµå ÁÂÇ¥·Î º¯È¯
        Vector3 worldPosition = UnitMap.CellToWorld(tilePosition);

        // À¯´Ö »ý¼º
        GameObject unit = Instantiate(unitPrefab, worldPosition, Quaternion.identity);
        unit.transform.SetParent(UnitMap.transform);

    }
    public PlayerUnit SpawnPlayer(Vector3Int tilePosition, GameObject unitPrefab)
    {
        Vector3 worldPosition = UnitMap.CellToWorld(tilePosition);

        // À¯´Ö »ý¼º
        GameObject unit = Instantiate(unitPrefab, worldPosition, Quaternion.identity);
        unit.transform.SetParent(UnitMap.transform);
        return unit.GetComponent<PlayerUnit>();
    }

    public MonsterUnit SpawnMonster(Vector3Int tilePosition, GameObject unitPrefab)
    {
        Vector3 worldPosition = UnitMap.CellToWorld(tilePosition);

        // À¯´Ö »ý¼º
        GameObject unit = Instantiate(unitPrefab, worldPosition, Quaternion.identity);
        unit.transform.SetParent(UnitMap.transform);
        return unit.GetComponent<MonsterUnit>();
    }


}
