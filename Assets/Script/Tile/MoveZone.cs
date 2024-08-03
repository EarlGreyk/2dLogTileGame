using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MoveZone : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    private Tilemap tilemap;

    private List<Vector3Int> movePos = new List<Vector3Int>();


    private void Update()
    {
        if(Input.GetButtonDown("Cancel"))
        {
            PlayerResource.instance.currentBlock = null;
            breakMoveTile();

            this.enabled = false;

            return;
        }
        if(Input.GetMouseButtonDown(0))
        {
            Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector3Int cellPosition = tilemap.WorldToCell(mouseWorldPos);

            TileBase clickedTile = tilemap.GetTile(cellPosition);

            if (clickedTile != null)
            {
                setMovePos(cellPosition);
                
            }
            else
            {
                Debug.Log("No tile at this position.");
            }
        }
    }
    /// <summary>
    /// 타일을 변환시켜 플레이어가 이동할 수 있도록 변경합니다.
    /// </summary>

    public void enableMoveTile(Vector3Int center)
    {
        tilemap.SetTile(center, Resources.Load<Tile>("Tile/None"));
        movePos.Add(center);
    }

    public void breakMoveTile()
    {
        //타일 롤백.
        for (int i = 0; i < movePos.Count; i++)
        {
            tilemap.SetTile(movePos[i], null);
        }
        movePos.Clear();
        this.enabled = false;
    }
   
    //플레이어가 클릭을 하면 클릭한 좌표를 받아와 동작합니다.
    //이동을 하면 즉시 해당 클래스를 비활성화 시켜 Update의 반복을 막습니다.
    public void setMovePos(Vector3Int cellPos)
    {
        //플레이어 이동
        GameManager.instance.PlayerUnit.transform.position = cellPos;
        

        //이동에 성공했음으로 플레이어의 블록 매니저에 접근하여 블록을 제거합니다.
        PlayerResource.instance.CurBlockRemove();
        //타일 롤백.
        for (int i = 0; i < movePos.Count; i++)
        {
            tilemap.SetTile(movePos[i], null);
        }
        movePos.Clear();

        
        this.enabled = false;
    }
    

}
