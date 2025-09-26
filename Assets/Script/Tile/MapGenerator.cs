using System.Collections;
using System.Collections.Generic;
using System.Linq;

using UnityEngine;




public class MapGenerator : MonoBehaviour
{
    public static MapGenerator Instance;

    public Grid grid;  // Scene에 Grid 할당
    public GameObject[] tilemapPrefabs;  // TileMap Prefabs 리스트
    public GameObject[] westTileMapPrefabs;
    public GameObject[] eastTileMapPrefabs;
    public GameObject[] northTileMapPrefabs;
    public GameObject[] southTileMapPrefabs;
    public GameObject initialTilePrefab;  // 최초 사용할 타일 Prefab
    public int mapSize;  // 생성할 맵의 크기 (가로/세로)
    private int tileCount = 0;  // 생성된 타일 수

    public Dictionary<Vector2Int, GameObject> spawnedTilemaps = new Dictionary<Vector2Int, GameObject>();  // 생성된 전체 타일 저장
    
    private Dictionary<Vector2Int, GameObject> progressTilemaps = new Dictionary<Vector2Int, GameObject>(); // 생성된 타일중 진행 방향. (타일 카운트 체크)
    public Dictionary<Vector2Int,TileMapInfo> tileMapInfo = new Dictionary<Vector2Int, TileMapInfo>(); 
    private Vector2Int lastbeforePos;
    private Vector2Int lastPos;

    public GameObject BattleField;


    private int stage;
    public int Stage { get { return stage; } set { stage = value; } }


    [SerializeField]
    private MiniMapManager miniMapManager;


    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        else 
        {
            Destroy(this);
        }
    }

    private void Start()
    {
        if (grid == null)
        {
            Debug.LogError("Grid 오브젝트가 할당되지 않았습니다.");
            return;
        }
        if (SettingData.Load == false)
        {
            Stage = 1; ;
            LoadTilemapPrefabs();
            GenerateMap();  // 맵 생성
            GameManager.instance.setStayPlayer();
        }else
        {
            Stage = SaveLoadManager.instance.MapGeneratorSaveData.Stage;
            LoadTilemapPrefabs();
            GenerateLoadMap();
            GameManager.instance.setStayPlayer();
        }
            
    }

    // 타일맵 프리팹 로드
    private void LoadTilemapPrefabs()
    {
   
        
        string stage = Stage.ToString() + "Stage/";
        Debug.Log(stage);

        initialTilePrefab = Resources.Load<GameObject>("Prefabs/TileMap/" + stage + "base");

        eastTileMapPrefabs = Resources.LoadAll<GameObject>("Prefabs/TileMap/" + stage + "East");
        westTileMapPrefabs = Resources.LoadAll<GameObject>("Prefabs/TileMap/" + stage + "West");
        northTileMapPrefabs = Resources.LoadAll<GameObject>("Prefabs/TileMap/" + stage + "North");
        southTileMapPrefabs = Resources.LoadAll<GameObject>("Prefabs/TileMap/" + stage + "South");


        if (tilemapPrefabs.Length == 0)
        {
            Debug.LogError("타일맵 프리팹이 설정되지 않았습니다.");
        }
        else
        {
            Debug.Log("타일맵 프리팹 로딩 완료");
        }
    }

    


    //상호작용 가능 유닛을 파괴 합니다.
    //상호작용 가능한 유닛이 있는 지역의 이동 가능여부를 활성화합니다.
    public void DestroyInteraction(Vector2Int targetPos)
    {
        tileMapInfo[targetPos].InterObj.gameObject.SetActive(false);
        //
        tileMapInfo[GameManager.instance.CurrentPos].MoveCheck = true;
    }
    /// <summary>
    /// 전투 타일 생성
    /// 전투시 전투에 필요한 타일을 현재 상호작용한 지역과 대응되게 생성합니다.
    /// </summary>
    /// <returns></returns>

    public BattleZone BattleZoneSet()
    {

        BattleField = Instantiate<GameObject>(spawnedTilemaps[GameManager.instance.CurrentPos], GameManager.instance.Grid.transform);
        BattleField.transform.position = Vector3.zero;
        BattleZone value = BattleField.GetComponentInChildren<BattleZone>();
        TileMapInfo info = BattleField.GetComponentInChildren<TileMapInfo>();
        info.InterObj.gameObject.SetActive(false);




        return value;
    }



    private void GenerateLoadMap()
    {
        Dictionary<Vector2Int, string> SpwanTileDic = SaveLoadManager.instance.MapGeneratorSaveData.GetSpawnTileDict();
        Dictionary<Vector2Int, bool> SpawnTileShow = SaveLoadManager.instance.MapGeneratorSaveData.GetSpawnTileShow();
        Dictionary<Vector2Int, TileMapInfoSaveData> TileMapInfoDic = SaveLoadManager.instance.MapGeneratorSaveData.GetTileMapDict();
        


        foreach (var map in SpwanTileDic)
        {
            Vector2Int pos = map.Key;
            char dic = map.Value[0];
            string path ="";
            string stage = Stage.ToString() + "Stage/";
            switch (dic)
            {
                case 'W':
                    path = "Prefabs/TileMap/" + stage + "West/" + map.Value;
                    break;
                case 'E':
                    path = "Prefabs/TileMap/" + stage + "East/" + map.Value;
                    break;
                case 'N':
                    path = "Prefabs/TileMap/" + stage + "North/" + map.Value;
                    break;
                case 'S':
                    path = "Prefabs/TileMap/" + stage + "South/" + map.Value;
                    break;
                 
            }
            GameObject obj = Resources.Load<GameObject>(path);


            //Debug.Log($"방향 : {dic} , 경로 : {path} , 오브젝트 : {obj}, 좌표{pos}");

            TileMapInfoSaveData saveData = TileMapInfoDic[pos];
            bool MinimapShow = SpawnTileShow[pos];
            SpawnTile(pos, obj, saveData, MinimapShow);
        }
    }
    /// <summary>
    /// 맵 생성 함수. 로드하여 데이터를 받아서 생성할때가 아닌 직접 생성하는 방식입ㄴ다.
    /// </summary>
    private void GenerateMap()
    {
        // 최초 타일 생성
        if (initialTilePrefab != null)
        {
            SpawnTile(Vector2Int.zero, Vector2Int.zero, initialTilePrefab);
        }




        while (tileCount < mapSize)
        {

            Vector2Int lastTilePos = lastPos; // 마지막 타일 위치
            List<Vector2Int> newTilePosList = GetNextTilePosition(lastTilePos);
            int RandomCount = Random.Range(0, newTilePosList.Count);
            GameObject selectedTile = null;
            bool endcheck = false;


            //Debug.Log($"{tileCount} 의 생성갯수 : {newTilePosList.Count}");


            for (int i = 0; i < newTilePosList.Count; i++)
            {
                Vector2Int nextPos = newTilePosList[i];
                if (i == RandomCount)
                {
                    Debug.Log("진행방향 : " + newTilePosList[i]);
                    if (tileCount != mapSize - 1)
                        selectedTile = GetTilemapPrefabByDirection(lastTilePos, nextPos);
                    else
                        selectedTile = GetTilemapPrefabByDirection(lastTilePos, nextPos, true);
                }
                else
                {
                    selectedTile = GetTilemapPrefabByDirection(lastTilePos, nextPos, true);
                }

                if (selectedTile == null)
                {
                    endcheck = true;
                    //더이상 맵생성을 못하는 구간에 도달 하여 마지막으로 생성한뒤 생성을 중지합니다
                    selectedTile = GetTilemapPrefabByDirection(lastTilePos, nextPos, true);
                    if (!spawnedTilemaps.ContainsKey(nextPos))
                    {
                        int previousTileCount = tileCount;
                        SpawnTile(nextPos, lastTilePos, selectedTile);

                    }

                }
                else
                {
                    // 타일 생성 전 맵에 이미 존재하지 않는지 확인
                    if (!spawnedTilemaps.ContainsKey(nextPos))
                    {
                        int previousTileCount = tileCount;
                        SpawnTile(nextPos, lastTilePos, selectedTile);

                    }
                }


            }

            if (endcheck)
                break;

            //진행해야할 방향을 저장
            //다음 진행방향을 저장하기 전에 현재 마지막 값을 저장하여 마지막값의 앞의 순서를 확인할수 있도록함.   

            lastbeforePos = lastPos;
            lastPos = newTilePosList[RandomCount];
            progressTilemaps[newTilePosList[RandomCount]] = spawnedTilemaps[newTilePosList[RandomCount]];


            if (progressTilemaps[newTilePosList[RandomCount]])
            {
                tileCount++;
            }
            else
            {
                Debug.Log("에러. 더이상 생성할수 없습니다");
                break;
            }


        }






    }
    /// <summary>
    /// 게임 로드시에 작동하는 타일 생성
    /// </summary>
    /// <param name="pos"></param>
    /// <param name="customTilePrefab"></param>
    /// <param name="saveData"></param>
    private void SpawnTile(Vector2Int pos, GameObject customTilePrefab,TileMapInfoSaveData saveData,bool mapShow)
    {
        TileMapInfo tilemapGetCompnent = null;
        GameObject newTilemapObject = null;
        if (pos == Vector2Int.zero)
        {
            // 최초 타일은 지정된 initialTilePrefab로 생성
            if (initialTilePrefab == null)
            {
                Debug.LogWarning("초기 타일이 지정되지 않았습니다.");
                return;
            }

            // 타일을 지정된 위치에 생성
            newTilemapObject = Instantiate(initialTilePrefab, grid.transform);
            newTilemapObject.transform.position = new UnityEngine.Vector3(pos.x * 30, pos.y * 30, 0);

            // 최초 타일의 방향 정보 가져오기
            tilemapGetCompnent = newTilemapObject.GetComponent<TileMapInfo>();

            //로드로 생성된 타일 정보값 갱신하기.
            tilemapGetCompnent.LoadData(saveData);

            ////////////////////////////////
            // 생성된 타일을 맵에 등록
            spawnedTilemaps[pos] = newTilemapObject;
            tileMapInfo[pos] = tilemapGetCompnent;
            //생성된 타일 미니맵에 등록
            miniMapManager.MiniMapSetting(pos, tilemapGetCompnent,mapShow);

            return;
        }else
        {
            // 타일을 지정된 위치에 생성
            newTilemapObject = Instantiate(customTilePrefab, grid.transform);
            newTilemapObject.transform.position = new UnityEngine.Vector3(pos.x * 30, pos.y * 30, 0);

            // 최초 타일의 방향 정보 가져오기
            tilemapGetCompnent = newTilemapObject.GetComponent<TileMapInfo>();

            //로드로 생성된 타일 정보값 갱신하기.
            tilemapGetCompnent.LoadData(saveData);
           

            ////////////////////////////////
            // 생성된 타일을 맵에 등록
            spawnedTilemaps[pos] = newTilemapObject;
            tileMapInfo[pos] = tilemapGetCompnent;
            //생성된 타일 미니맵에 등록
            miniMapManager.MiniMapSetting(pos, tilemapGetCompnent, mapShow);
        }
    }
    /// <summary>
    /// 게임을 로드가 아닌 방식으로 시작할때 생성하는 타일 생성
    /// </summary>
    /// <param name="pos"></param>
    /// <param name="incomingDirection"></param>
    /// <param name="customTilePrefab"></param>


    void SpawnTile(Vector2Int pos, Vector2Int incomingDirection, GameObject customTilePrefab)
    {

        GameObject selectedTilePrefab = null;
        GameObject newTilemapObject = null;
        TileMapInfo tilemapGetCompnent = null;

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
            tilemapGetCompnent = newTilemapObject.GetComponent<TileMapInfo>();
            //타일 난이도 조정
            tilemapGetCompnent.difficult = tileCount;
            //타일 초기화 시작
            tilemapGetCompnent.Init();


            // 생성된 타일을 맵에 등록
            
            spawnedTilemaps[pos] = newTilemapObject;
            progressTilemaps[pos] = newTilemapObject;

            tileMapInfo[pos] = tilemapGetCompnent;
            //생성된 타일 미니맵에 등록
            miniMapManager.MiniMapSetting(pos, tilemapGetCompnent);

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
        tilemapGetCompnent = newTilemapObject.GetComponent<TileMapInfo>();
        //타일 난이도 조정
        tilemapGetCompnent.difficult = tileCount;
        //초기화
        tilemapGetCompnent.Init();

        
        // 타일 위치 설정 (15x15 크기 고려)
        newTilemapObject.transform.position = new UnityEngine.Vector3(pos.x * 30, pos.y * 30, 0);

        // 연결 가능 여부 확인
        if (!CanConnectTile(tilemapGetCompnent, incomingDirection - pos))
        {
            Debug.LogWarning("타일을 이 위치에 배치할 수 없습니다.");
            return;
        }

        // 생성된 타일을 맵에 등록
        spawnedTilemaps[pos] = newTilemapObject;
     
      
        tileMapInfo[pos] = tilemapGetCompnent;
        //생성된 타일 미니맵에 등록
        miniMapManager.MiniMapSetting(pos, tilemapGetCompnent);

    }

   


    // 타일이 연결 가능한지 확인하는 함수
    bool CanConnectTile(TileMapInfo tilemapDirection, Vector2Int incomingDirection)
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
    /// 다음 좌표에 설치해야될때 해당 좌표에서 동서남북을 비교하여 해당 좌표에 타일맵이 있다면 해당좌표에 빈공간이 열려있는 프리팹은 가져올 수 없습니다.
    /// </summary>
    /// <param name="beforePos"></이전 좌표>
    /// <param name="nextPos"></새로 생성될 다음 좌표>
    /// <returns></returns>

    GameObject GetTilemapPrefabByDirection(Vector2Int beforePos, Vector2Int nextPos, bool check = false)
    {
        List<GameObject> Prefabs = new List<GameObject>();
        //이전타일과 현재의 타일의 좌표를 구해야함.
        Vector2Int direction = nextPos - beforePos;


        //북쪽으로 이동함 새로 생성될 프리팹은 남쪽이 반드시 비어져 있어야 하며 추가적으로 막혀있지 않아야한다.
        if (direction == Vector2Int.up)
        {
            if (check == false)
            {
                foreach (var prefab in southTileMapPrefabs)
                {
                    TileMapInfo tilemapDirection = prefab.GetComponent<TileMapInfo>();
                    if (tilemapDirection.Down)
                    {

                        if (tilemapDirection.Left)
                        {
                            if (spawnedTilemaps.ContainsKey(nextPos + Vector2Int.left))
                            {
                                Debug.Log("error : " + prefab);
                                continue;
                            }

                        }

                        if (tilemapDirection.Right)
                        {
                            if (spawnedTilemaps.ContainsKey(nextPos + Vector2Int.right))
                            {
                                Debug.Log("error : " + prefab);
                                continue;
                            }


                        }


                        if (tilemapDirection.Up)
                        {
                            if (spawnedTilemaps.ContainsKey(nextPos + Vector2Int.up))
                            {
                                Debug.Log("error : " + prefab);
                                continue;
                            }

                        }

                        if (tilemapDirection.Left || tilemapDirection.Up || tilemapDirection.Right)
                            Prefabs.Add(prefab);

                    }

                }
            }
            else
            {
                Prefabs.Add(southTileMapPrefabs[southTileMapPrefabs.Length - 1]);
            }

        }

        //남쪽으로 이동함 새로 생성될 프리팹은 북쪽이 반드시 비어져 있어야 하며 추가적으로 막혀있지 않아야한다.

        if (direction == Vector2Int.down)
        {
            if (check == false)
            {
                foreach (var prefab in northTileMapPrefabs)
                {
                    TileMapInfo tilemapDirection = prefab.GetComponent<TileMapInfo>();
                    Debug.Log(prefab);
                    if (tilemapDirection.Up)
                    {

                        if (tilemapDirection.Left)
                        {
                            if (spawnedTilemaps.ContainsKey(nextPos + Vector2Int.left))
                            {
                                Debug.Log("error : " + prefab);
                                continue;
                            }

                        }

                        if (tilemapDirection.Right)
                        {
                            if (spawnedTilemaps.ContainsKey(nextPos + Vector2Int.right))
                            {
                                Debug.Log("error : " + prefab);
                                continue;
                            }

                        }

                        if (tilemapDirection.Down)
                        {
                            if (spawnedTilemaps.ContainsKey(nextPos + Vector2Int.down))
                            {
                                Debug.Log("error : " + prefab);
                                continue;
                            }

                        }

                        if (tilemapDirection.Down || tilemapDirection.Left || tilemapDirection.Right)
                            Prefabs.Add(prefab);

                    }

                }
            }
            else
            {
                Prefabs.Add(northTileMapPrefabs[northTileMapPrefabs.Length - 1]);
            }


        }

        //서쪽으로 이동함 새로 생성될 프리팹은 동쪽이 반드시 비어져 있어야 하며 추가적으로 막혀있지 않아야한다.
        if (direction == Vector2Int.left)
        {
            if (check == false)
            {
                foreach (var prefab in eastTileMapPrefabs)
                {
                    TileMapInfo tilemapDirection = prefab.GetComponent<TileMapInfo>();
                    if (tilemapDirection.Right)
                    {


                        if (tilemapDirection.Left)
                        {

                            if (spawnedTilemaps.ContainsKey(nextPos + Vector2Int.left))
                            {
                                Debug.Log("error : " + prefab);
                                continue;
                            }

                        }

                        if (tilemapDirection.Up)
                        {
                            if (spawnedTilemaps.ContainsKey(nextPos + Vector2Int.up))
                            {
                                Debug.Log("error : " + prefab);
                                continue;
                            }

                        }

                        if (tilemapDirection.Down)
                        {
                            if (spawnedTilemaps.ContainsKey(nextPos + Vector2Int.down))
                            {
                                Debug.Log("error : " + prefab);
                                continue;
                            }

                        }
                        if (tilemapDirection.Up || tilemapDirection.Down || tilemapDirection.Left)
                            Prefabs.Add(prefab);

                    }
                }
            }
            else
            {
                Prefabs.Add(eastTileMapPrefabs[eastTileMapPrefabs.Length - 1]);
            }
        }

        //동쪽으로 이동함 새로 생성될 프리팹은 서쪽이 반드시 비어져 있어야 하며 추가적으로 막혀있지 않아야한다.
        if (direction == Vector2Int.right)
        {
            if (check == false)
            {
                foreach (var prefab in westTileMapPrefabs)
                {
                    TileMapInfo tilemapDirection = prefab.GetComponent<TileMapInfo>();

                    if (tilemapDirection.Left)
                    {

                        if (tilemapDirection.Right)
                        {
                            if (spawnedTilemaps.ContainsKey(nextPos + Vector2Int.right))
                            {
                                Debug.Log("error : " + prefab);
                                continue;
                            }
                        }


                        if (tilemapDirection.Up)
                        {
                            if (spawnedTilemaps.ContainsKey(nextPos + Vector2Int.up))
                            {
                                Debug.Log("error : " + prefab);
                                continue;
                            }
                        }

                        if (tilemapDirection.Down)
                        {
                            if (spawnedTilemaps.ContainsKey(nextPos + Vector2Int.down))
                            {
                                Debug.Log("error : " + prefab);
                                continue;
                            }
                        }
                        if (tilemapDirection.Up || tilemapDirection.Down || tilemapDirection.Right)
                            Prefabs.Add(prefab);
                    }

                }
            }
            else
            {
                Prefabs.Add(westTileMapPrefabs[westTileMapPrefabs.Length - 1]);
            }

        }

        if (Prefabs.Count > 0)
        {
            int Count = Random.Range(0, Prefabs.Count);
            return Prefabs[Count];
        }
        else
        {
            return null;
        }


    }

    private Vector2Int GetLastSpawnedTilePosition()
    {
        if (spawnedTilemaps.Count > 0)
        {
            // 딕셔너리의 마지막 요소를 가져옴
            return spawnedTilemaps.Keys.Last();
        }

        return Vector2Int.zero;
    }
    /// <summary>
    /// 연결 되어있는 좌표를 체크합니다.
    /// GetNextTilePosition : {lastbeforePos - lastTilePos}
    /// </summary>
    /// <param name="lastTilePos"></GetNextTilePosition의 비교좌표>
    /// <returns></returns>

    List<Vector2Int> GetNextTilePosition(Vector2Int lastTilePos)
    {
        // 이전 타일의 방향을 확인
        GameObject lastTile = spawnedTilemaps[lastTilePos];
        TileMapInfo lastTileInfo = lastTile.GetComponent<TileMapInfo>();
        List<Vector2Int> TileDirections = new List<Vector2Int>();

        // 이전 타일이 가진 방향에 맞는 위치를 계산하여 반환
        // 타일의 방향에 맞게 연결된 위치를 반환
        if (lastTileInfo.Up)
        {
            if (lastbeforePos - lastTilePos != Vector2Int.up)
                TileDirections.Add(lastTilePos + new Vector2Int(0, 1)); 
        }
        if (lastTileInfo.Down)
        {
            if (lastbeforePos - lastTilePos != Vector2Int.down)
                TileDirections.Add(lastTilePos + new Vector2Int(0, -1)); 
        }
        if (lastTileInfo.Left)
        {
            if (lastbeforePos - lastTilePos != Vector2Int.left)
                TileDirections.Add(lastTilePos + new Vector2Int(-1, 0)); 
        }
        if (lastTileInfo.Right)
        {
            if (lastbeforePos - lastTilePos != Vector2Int.right)
                TileDirections.Add(lastTilePos + new Vector2Int(1, 0)); 
        }

        return TileDirections;  // 연결 불가능한 경우는 zero 반환
    }



}