using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.Tilemaps;
//using static UnityEditor.PlayerSettings;

public class UnitSpawner : MonoBehaviour
{
    [SerializeField]
    private Grid grid;
    [SerializeField]
    private Tilemap UnitMap;

    public void SpawnUnit(Vector3Int tilePosition,GameObject unitPrefab)
    {
        Debug.Log($"유닛 생성 좌표 : {tilePosition}");
       
        Vector3 scale = grid.transform.localScale;
        // 타일맵의 타일 좌표를 월드 좌표로 변환
        Vector3 worldPosition = UnitMap.CellToWorld(tilePosition);
        int x = Mathf.FloorToInt(worldPosition.x / scale.x);
        int y = Mathf.FloorToInt(worldPosition.y / scale.y);
        Vector3Int unitPos = new Vector3Int(x, y, 0);

        // 유닛 생성
        GameObject unit = Instantiate(unitPrefab, unitPos, Quaternion.identity);
        unit.transform.SetParent(UnitMap.transform);

    }
    public PlayerUnit SpawnPlayer(Vector3Int tilePosition, GameObject unitPrefab, bool stay = false)
    {
        Vector3 scale = grid.transform.localScale;
        // 타일맵의 타일 좌표를 월드 좌표로 변환
        Vector3 worldPosition = UnitMap.CellToWorld(tilePosition);
        int x = Mathf.FloorToInt(worldPosition.x / scale.x);
        int y = Mathf.FloorToInt(worldPosition.y / scale.y);
        Vector3Int unitPos = new Vector3Int(x, y, 0);
        // 유닛 생성
        GameObject unit = Instantiate(unitPrefab, unitPos, Quaternion.identity);
        unit.transform.SetParent(UnitMap.transform);
        PlayerUnit player = unit.GetComponent<PlayerUnit>();
        if(!stay)
            GameManager.instance.BattleZone.setTileUnit(tilePosition, player);
        //로드라면 유닛 좌표를 다시 수정해줘야함.
        if (SettingData.Load)
            unit.transform.localPosition = unitPos;

        return player;
    }

    public MonsterUnit SpawnMonster(Vector3Int tilePosition, GameObject unitPrefab, MonsterScriptableObject monsterdata)
    {
        Vector3 worldPosition = UnitMap.CellToWorld(tilePosition);
        Debug.Log($"유닛 생성 좌표 : {tilePosition}      :   변환 좌표 : {worldPosition}");
        // 유닛 생성
        GameObject unit = Instantiate(unitPrefab, tilePosition, Quaternion.identity);
        unit.transform.SetParent(UnitMap.transform);
        MonsterUnit monster = unit.GetComponent<MonsterUnit>();
        monster.Init(monsterdata);
        monster.transform.position = PosUnitSet(tilePosition);
        GameManager.instance.MonsterAIManager.MonsterSet(monster);
        GameManager.instance.BattleZone.setTileUnit(tilePosition, monster);

        return monster;
    }



    public Vector3 PosUnitSet(Vector3Int vector3)
    {
        
        Vector3 scale = grid.transform.localScale;
        // 타일맵의 타일 좌표를 월드 좌표로 변환
        Vector3 worldPosition = UnitMap.CellToWorld(vector3);
        int x = Mathf.FloorToInt(worldPosition.x / scale.x);
        int y = Mathf.FloorToInt(worldPosition.y / scale.y);

        Debug.Log($"{x} : {y}");
        Vector3Int unitPos = new Vector3Int(x, y, 0);

        Debug.Log($"변동전 좌표 {vector3}, 스케일 변동 좌표 {unitPos}");

        return unitPos;
    }
    
}
