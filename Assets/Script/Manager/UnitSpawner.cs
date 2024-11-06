using System.Collections;
using System.Collections.Generic;
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
        Vector3 scale = grid.transform.localScale;
        // Ÿ�ϸ��� Ÿ�� ��ǥ�� ���� ��ǥ�� ��ȯ
        Vector3 worldPosition = UnitMap.CellToWorld(tilePosition);
        int x = Mathf.FloorToInt(worldPosition.x / scale.x);
        int y = Mathf.FloorToInt(worldPosition.y / scale.y);
        Vector3Int unitPos = new Vector3Int(x, y, 0);
        Debug.Log("������ ��ġ" + unitPos);
        // ���� ����
        GameObject unit = Instantiate(unitPrefab, unitPos, Quaternion.identity);
        unit.transform.SetParent(UnitMap.transform);

    }
    public PlayerUnit SpawnPlayer(Vector3Int tilePosition, GameObject unitPrefab)
    {
        Vector3 scale = grid.transform.localScale;
        // Ÿ�ϸ��� Ÿ�� ��ǥ�� ���� ��ǥ�� ��ȯ
        Vector3 worldPosition = UnitMap.CellToWorld(tilePosition);
        int x = Mathf.FloorToInt(worldPosition.x / scale.x);
        int y = Mathf.FloorToInt(worldPosition.y / scale.y);
        Vector3Int unitPos = new Vector3Int(x, y, 0);
        Debug.Log("������ ��ġ" + unitPos);

        // ���� ����
        GameObject unit = Instantiate(unitPrefab, unitPos, Quaternion.identity);
        unit.transform.SetParent(UnitMap.transform);
        return unit.GetComponent<PlayerUnit>();
    }

    public MonsterUnit SpawnMonster(Vector3Int tilePosition, GameObject unitPrefab)
    {
        Vector3 worldPosition = UnitMap.CellToWorld(tilePosition);

        // ���� ����
        GameObject unit = Instantiate(unitPrefab, worldPosition, Quaternion.identity);
        unit.transform.SetParent(UnitMap.transform);
        MonsterUnit monster = unit.GetComponent<MonsterUnit>();
        GameManager.instance.MonsterAIManager.MonsterSet(monster);
        return monster;
    }



    public Vector3 PosUnitSet(Vector3Int vector3)
    {
        Vector3 scale = grid.transform.localScale;
        // Ÿ�ϸ��� Ÿ�� ��ǥ�� ���� ��ǥ�� ��ȯ
        Vector3 worldPosition = UnitMap.CellToWorld(vector3);
        int x = Mathf.FloorToInt(worldPosition.x / scale.x);
        int y = Mathf.FloorToInt(worldPosition.y / scale.y);
        Vector3Int unitPos = new Vector3Int(x, y, 0);


        return unitPos;
    }
    
}
