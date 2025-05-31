using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.Tilemaps;



public class MapGenerator : MonoBehaviour
{
    public Grid grid;  // Scene에 Grid 할당
    public GameObject[] tilemapPrefabs;  // TileMap Prefabs 리스트
    public GameObject initialTilePrefab;  // 최초 사용할 타일 Prefab
    public int mapSize = 3;  // 생성할 맵의 크기 (가로/세로)
    private int tileCount = 0;  // 생성된 타일 수
    private Dictionary<Vector2Int, GameObject> spawnedTilemaps = new Dictionary<Vector2Int, GameObject>();  // 생성된 전체 타일 저장
    private Dictionary<Vector2Int,GameObject> progressTilemaps = new Dictionary<Vector2Int, GameObject>(); // 생성된 타일중 진행 방향. (타일 카운트 체크)

    void Start()
    {
        if (grid == null)
        {
            Debug.LogError("Grid 오브젝트가 할당되지 않았습니다.");
            return;
        }

        LoadTilemapPrefabs();
        GenerateMap();  // 맵 생성
    }

    // 타일맵 프리팹 로드
    void LoadTilemapPrefabs()
    {
        if (tilemapPrefabs.Length == 0)
        {
            Debug.LogError("타일맵 프리팹이 설정되지 않았습니다.");
        }
        else
        {
            Debug.Log("타일맵 프리팹 로딩 완료");
        }
    }

    void GenerateMap()
    {
        // 최초 타일 생성
        if (initialTilePrefab != null)
        {
            SpawnTile(Vector2Int.zero, Vector2Int.zero, initialTilePrefab);
        }

 

        

        //while (tileCount<mapSize)
        //{
        //    Vector2Int lastTilePos = spawnedTilemaps.Keys.Last();  // 마지막으로 생성된 타일

        //    // 이전 타일의 위치를 기반으로 새로운 타일의 위치를 결정
           
        //    List<Vector2Int> newTilePosList = GetNextTilePosition(lastTilePos);

        //    bool spawnSuccess = false;

        //    // 새로운 길을 전부 생성해야함. 단 이중 [진짜]의 길 하나만 체크.

        //    for (int i = 0; i < newTilePosList.Count; i++)
        //    {
        //        GameObject selectedTile = GetTilemapPrefabByDirection(lastTilePos, newTilePosList[i]);
        //        SpawnTile(newTilePosList[i], lastTilePos, selectedTile);  // 타일을 생성하고 이어짐
                
        //    }
        //}
        while (tileCount < mapSize)
        {
            Vector2Int lastTilePos = progressTilemaps.Keys.Last(); // 마지막 타일 위치
            List<Vector2Int> newTilePosList = GetNextTilePosition(lastTilePos);
            int RandomCount = Random.Range(0, newTilePosList.Count);

            

            for (int i = 0; i < newTilePosList.Count; i++)
            {
                Vector2Int nextPos = newTilePosList[i];
                GameObject selectedTile = GetTilemapPrefabByDirection(lastTilePos, nextPos);

                // 타일 생성 전 맵에 이미 존재하지 않는지 확인
                if (!spawnedTilemaps.ContainsKey(nextPos))
                {
                    int previousTileCount = tileCount;
                    SpawnTile(nextPos, lastTilePos, selectedTile);

                    
                }

            }
            Debug.Log($"랜덤번호 : {RandomCount} | 최대번호{newTilePosList.Count}");    
            //진행해야할 방향을 저장
            progressTilemaps[newTilePosList[RandomCount]]= spawnedTilemaps[newTilePosList[RandomCount]];
            if (progressTilemaps[newTilePosList[RandomCount]])
            {
                tileCount++; 
                Debug.Log($"{tileCount} 번호 타일 생성 | 진행 좌표 : {newTilePosList[RandomCount]}");
            }
            else
            {
                Debug.Log("에러. 더이상 생성할수 없습니다");
                break;
            }
                

        }






        //// 가장 최근에 생성된 타일의 위치를 가져옴
        //lastTilePos = spawnedTilemaps.Keys.Last();  // 마지막으로 생성된 타일

        //// 이전 타일의 위치를 기반으로 새로운 타일의 위치를 결정
        //newTilePos = GetNextTilePosition(lastTilePos);

        //if (newTilePos != Vector2Int.zero)  // 유효한 위치인 경우
        //{
        //    GameObject selectedTile = GetTilemapPrefabByDirection(lastTilePos);  // 이전 타일의 방향에 맞는 타일을 선택
        //    SpawnTile(newTilePos, lastTilePos, selectedTile);  // 타일을 생성하고 이어짐
        //}



    }

    // 타일 생성 함수


    void SpawnTile(Vector2Int pos, Vector2Int incomingDirection, GameObject customTilePrefab)
    {
        Debug.Log($"해당 좌표에 타일 생성을 시도합니다.: {pos}, 이전 타일 좌표: {incomingDirection}, 생성되는 프리팹 : {customTilePrefab}");

        GameObject selectedTilePrefab = null;
        GameObject newTilemapObject = null;
        TileMapDirection tilemapDirection = null;

        // incomingDirection이 Vector2Int.zero이면 첫 타일로 간주
        if (incomingDirection == Vector2Int.zero)
        {
            // 최초 타일은 지정된 initialTilePrefab로 생성
            selectedTilePrefab = customTilePrefab ? customTilePrefab : initialTilePrefab;
            if (selectedTilePrefab == null)
            {
                Debug.LogWarning("초기 타일이 지정되지 않았습니다.");
                return;
            }

            // 타일을 지정된 위치에 생성
            newTilemapObject = Instantiate(selectedTilePrefab, grid.transform);
            newTilemapObject.transform.position = new UnityEngine.Vector3(pos.x * 30, pos.y * 30, 0);

            // 최초 타일의 방향 정보 가져오기
            tilemapDirection = newTilemapObject.GetComponent<TileMapDirection>();
            if (tilemapDirection != null)
            {
                // 최초 타일의 방향 정보를 저장하거나 후속 타일 생성을 위한 처리를 할 수 있습니다.
            }

            // 생성된 타일을 맵에 등록
            spawnedTilemaps[pos] = newTilemapObject;
            progressTilemaps[pos] = newTilemapObject;
            return;
        }

        // `incomingDirection`이 Zero가 아닌 경우
        
        if (customTilePrefab == null)
        {
            Debug.LogWarning("이 위치에 맞는 타일을 찾을 수 없습니다.");
            return;
        }

        // 타일을 생성
        newTilemapObject = Instantiate(customTilePrefab, grid.transform);
        tilemapDirection = newTilemapObject.GetComponent<TileMapDirection>();

        // 타일 위치 설정 (15x15 크기 고려)
        newTilemapObject.transform.position = new UnityEngine.Vector3(pos.x * 30, pos.y * 30, 0);

        // 연결 가능 여부 확인
        if (!CanConnectTile(tilemapDirection, incomingDirection-pos))
        {
            Debug.LogWarning("타일을 이 위치에 배치할 수 없습니다.");
            return;
        }

        // 생성된 타일을 맵에 등록
        spawnedTilemaps[pos] = newTilemapObject;
    }

    // 타일이 연결 가능한지 확인하는 함수
    bool CanConnectTile(TileMapDirection tilemapDirection, Vector2Int incomingDirection)
    {
        Debug.Log($"현재타일 번호 {tileCount} -> 타일연결체크 {incomingDirection}");
        if (incomingDirection == Vector2Int.up && tilemapDirection.Up) return true;
        if (incomingDirection == Vector2Int.down && tilemapDirection.Down) return true;
        if (incomingDirection == Vector2Int.left && tilemapDirection.Left) return true;
        if (incomingDirection == Vector2Int.right && tilemapDirection.Right) return true;

        return false;
    }


    /// <summary>
    /// 이전 좌표와 다음 좌표를 비교하여 프리팹을 가져옵니다.
    /// 
    /// </summary>
    /// <param name="beforePos"></이전 좌표>
    /// <param name="nextPos"></새로 생성될 다음 좌표>
    /// <returns></returns>

    GameObject GetTilemapPrefabByDirection(Vector2Int beforePos, Vector2Int nextPos)
    {
        List<GameObject> Prefabs = new List<GameObject>();
        //이전타일과 현재의 타일의 좌표를 구해야함.
        Vector2Int direction = nextPos - beforePos;

        //북쪽으로 이동함 새로 생성될 프리팹은 남쪽이 반드시 비어져 있어야 하며 추가적으로 막혀있지 않아야한다.
        if(direction == Vector2Int.up)
        {
            foreach (var prefab in tilemapPrefabs)
            {
                TileMapDirection tilemapDirection = prefab.GetComponent<TileMapDirection>();
                if(tilemapDirection.Down)
                {
                    if(tilemapDirection.Left || tilemapDirection.Right || tilemapDirection.Up)
                        Prefabs.Add(prefab); Debug.Log($"현재 타일 번호{tileCount}->>북쪽이동 , 남쪽이 반드시 비어있음");
                }
                
            }
        }

        //남쪽으로 이동함 새로 생성될 프리팹은 북쪽이 반드시 비어져 있어야 하며 추가적으로 막혀있지 않아야한다.

        if (direction == Vector2Int.down)
        {
            foreach (var prefab in tilemapPrefabs)
            {
                TileMapDirection tilemapDirection = prefab.GetComponent<TileMapDirection>();
                if (tilemapDirection.Up)
                {
                    if (tilemapDirection.Left || tilemapDirection.Right || tilemapDirection.Down)
                        Prefabs.Add(prefab); Debug.Log($"현재 타일 번호{tileCount}->>남쪽이동 , 북쪽이 반드시 비어있음");
                }

            }

        }

        //서쪽으로 이동함 새로 생성될 프리팹은 동쪽이 반드시 비어져 있어야 하며 추가적으로 막혀있지 않아야한다.
        if (direction == Vector2Int.left)
        {
            foreach (var prefab in tilemapPrefabs)
            {
                TileMapDirection tilemapDirection = prefab.GetComponent<TileMapDirection>();
                if (tilemapDirection.Right)
                {
                    if (tilemapDirection.Left || tilemapDirection.Down || tilemapDirection.Up)
                        Prefabs.Add(prefab); Debug.Log($"현재 타일 번호{tileCount}->>서쪽이동 , 동쪽이 반드시 비어있음");
                }

            }
        }

        //동쪽으로 이동함 새로 생성될 프리팹은 서쪽이 반드시 비어져 있어야 하며 추가적으로 막혀있지 않아야한다.
        if (direction == Vector2Int.right)
        {
            foreach (var prefab in tilemapPrefabs)
            {
                TileMapDirection tilemapDirection = prefab.GetComponent<TileMapDirection>();
                if (tilemapDirection.Left)
                {
                    if (tilemapDirection.Down || tilemapDirection.Right || tilemapDirection.Up)
                        Prefabs.Add(prefab); Debug.Log($"현재 타일 번호{tileCount}->>동쪽이동 , 서쪽이 반드시 비어있음");
                }

            }

        }

        if(Prefabs.Count > 0)
        {
            int Count = Random.Range(0, Prefabs.Count);
            return Prefabs[Count];
        }else
        {
            return null;
        }


    }

    Vector2Int GetLastSpawnedTilePosition()
    {
        if (spawnedTilemaps.Count > 0)
        {
            // 딕셔너리의 마지막 요소를 가져옴
            return spawnedTilemaps.Keys.Last();
        }

        return Vector2Int.zero;
    }


    List<Vector2Int> GetNextTilePosition(Vector2Int lastTilePos)
    {
        // 이전 타일의 방향을 확인
        GameObject lastTile = spawnedTilemaps[lastTilePos];
        TileMapDirection lastTileDirection = lastTile.GetComponent<TileMapDirection>();
        List<Vector2Int> TileDirections = new List<Vector2Int>();

        // 이전 타일이 가진 방향에 맞는 위치를 계산하여 반환
        // 타일의 방향에 맞게 연결된 위치를 반환
        if (lastTileDirection.Up)
        {
            TileDirections.Add(lastTilePos + new Vector2Int(0, 1));
            Debug.Log($"{tileCount} 번호 타일 (0,1)이 연결되어 있습니다.");
        }
        if (lastTileDirection.Down)
        {
            TileDirections.Add(lastTilePos + new Vector2Int(0, -1)); // 아래로 이어짐
            Debug.Log($"{tileCount} 번호 타일 (0,-1)이 연결되어 있습니다.");
        }
        if (lastTileDirection.Left)
        {
            TileDirections.Add(lastTilePos + new Vector2Int(-1, 0)); // 왼쪽으로 이어짐
            Debug.Log($"{tileCount} 번호 타일 (-1,0)이 연결되어 있습니다.");
        }
        if (lastTileDirection.Right)
        {
            TileDirections.Add(lastTilePos + new Vector2Int(1, 0)); // 오른쪽으로 이어짐
            Debug.Log($"{tileCount} 번호 타일 (1,0)이 연결되어 있습니다.");
        }

        return TileDirections;  // 연결 불가능한 경우는 zero 반환
    }



}