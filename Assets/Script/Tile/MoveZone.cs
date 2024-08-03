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
    /// Ÿ���� ��ȯ���� �÷��̾ �̵��� �� �ֵ��� �����մϴ�.
    /// </summary>

    public void enableMoveTile(Vector3Int center)
    {
        tilemap.SetTile(center, Resources.Load<Tile>("Tile/None"));
        movePos.Add(center);
    }

    public void breakMoveTile()
    {
        //Ÿ�� �ѹ�.
        for (int i = 0; i < movePos.Count; i++)
        {
            tilemap.SetTile(movePos[i], null);
        }
        movePos.Clear();
        this.enabled = false;
    }
   
    //�÷��̾ Ŭ���� �ϸ� Ŭ���� ��ǥ�� �޾ƿ� �����մϴ�.
    //�̵��� �ϸ� ��� �ش� Ŭ������ ��Ȱ��ȭ ���� Update�� �ݺ��� �����ϴ�.
    public void setMovePos(Vector3Int cellPos)
    {
        //�÷��̾� �̵�
        GameManager.instance.PlayerUnit.transform.position = cellPos;
        

        //�̵��� ������������ �÷��̾��� ��� �Ŵ����� �����Ͽ� ����� �����մϴ�.
        PlayerResource.instance.CurBlockRemove();
        //Ÿ�� �ѹ�.
        for (int i = 0; i < movePos.Count; i++)
        {
            tilemap.SetTile(movePos[i], null);
        }
        movePos.Clear();

        
        this.enabled = false;
    }
    

}
