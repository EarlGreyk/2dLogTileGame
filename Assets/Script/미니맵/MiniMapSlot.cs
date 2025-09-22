using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MiniMapSlot : MonoBehaviour
{
    [SerializeField]
    private Image slotImage;
    [SerializeField]
    private Image showEnableImage;

    public bool show;

    private Vector2 teleportPos;

    private Button teleportButton;

    //연결된 통로의 bool값을 받아서 sprite를 불러와 적용합니다.
    public void SlotSet(bool up,bool down,bool left, bool right, Vector2 telPos, bool show = false)
    {

        Sprite sprite = null;
        string path = "Art/MiniMap/";


        if(up)
        {
            path += "Up";
        }
        if(down)
        {
            path += "Down"; 
        }
        if(left)
        {
            path += "Left";
        }
        if(right)
        {
            path += "Right";
        }
        Debug.Log(path);

        sprite = Resources.Load<Sprite>(path);

        this.show = show;
        slotImage.sprite = sprite;
        if(show)
        {
            showEnableImage.enabled = false;
        }else
        {
            showEnableImage.enabled = true;
        }
        teleportPos = telPos;




        teleportButton = GetComponent<Button>();

        teleportButton.onClick.AddListener(() => MapTeleport());
        teleportButton.interactable = show;

    }

    //비활성화 되어있다가 보여줍니다.
    public void Show()
    {
        if(!show)
        {
            showEnableImage.enabled = false;
            teleportButton.interactable = true;
            show = true;
        }
        
    }



    public void MapTeleport()
    {
        Debug.Log("텔포시작");
        if(show)
        {
            StartCoroutine(teleport());
            //텔레 포트 연출필요.
        }
    }



    IEnumerator teleport()
    {
        PopUpManager.instance.LastClosePopUp();
        Vector2 pos = new Vector2(teleportPos.x * 15f + 7.5f, teleportPos.y * 15f + 7.5f);
        Debug.Log(pos);
        //yield return new WaitForSeconds(0.5f);

        GameManager.instance.StayPlayerUnit.transform.localPosition = pos;

        int x = (int)teleportPos.x;
        int y = (int)teleportPos.y;
        GameManager.instance.CurrentPos = new Vector2Int(x, y);
        Debug.Log(GameManager.instance.CurrentPos);
        yield break;
    }
}
