using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TileMapInfo : MonoBehaviour
{
    
    public bool Up;
    public bool Down;
    public bool Left;
    public bool Right;


    /// <summary>
    /// 해당 지역의 전투를 체크합니다.
    /// </summary>
    //public bool Battle;

    /// <summary>
    /// 해당 지역의 정화를 체크합니다.
    /// </summary>
    public bool Clear;


    /// <summary>
    /// 해당 지역의 난이도를 체크합니다.
    /// 해당 난이도에 따라 몬스터가 정해집니다.
    /// 해당 난이도는 생성되는 번호순으로 체크됩니다.
    /// </summary>
    public int difficult;


    /// <summary>
    /// 정화 유닛입니다.
    /// </summary>
    public InteractionObject InterObj;


 


    /// <summary>
    /// 해당 타일맵이 맵 생성시 첫번째에 될수 있는지 없는지를 체크합니다.
    /// 만약 있다면 interObj을 생성해선 안됩니다.
    /// </summary>
    public bool FirstCheck;

    /// <summary>
    /// 해당 지역에서 다른지역으로 이동 가능한지 체크 여부를 판단합니다. 
    /// 기본값은 true이며 ClearObject가 있다면 false로 전환되어야합니다.
    /// </summary>
    public bool MoveCheck;

    /// <summary>
    /// 로드가 아닌 생성일때 초기화 합니다.
    /// </summary>
    public void Init()
    {
        Clear = false;
        MoveCheck = true;
        if(InterObj == null )
        {
            GameObject obj = null;
            //첫번쨰 지역 (베이스지역)
            if(!FirstCheck)
            {
                //상호작용 유닛 롤
                if (Random.Range(1, 10) <= 10)
                {
                    //정화유닛
                    obj = Instantiate<GameObject>(Resources.Load<GameObject>("Prefabs/Interaction/ClearUnit"), gameObject.transform);
                    MoveCheck = false;
                }
                else
                {
                    //상점
                    obj = Instantiate<GameObject>(Resources.Load<GameObject>("Prefabs/Interaction/ShopUnit"), gameObject.transform);
                }
                Debug.Log(obj);
                if (obj == null)
                    return;

                InterObj = obj.GetComponent<InteractionObject>();
                Debug.Log(InterObj);


                InterObj.value = difficult * 10 + 15;
                //상호작용 오브젝트는 반드시 중앙 좌표에 존재합니다.
                obj.transform.localPosition = new(7.5f, 7.5f);
                //모든 상호작용 설치 준비가 완료되었음으로 해당 상호작용 유닛을 초기화합니다

                InterObj.InteractSet();
            }else
            {
                obj = Instantiate<GameObject>(Resources.Load<GameObject>("Prefabs/Interaction/ClearUnit"), gameObject.transform);
                MoveCheck = true;
                if (obj == null)
                    return;

                InterObj = obj.GetComponent<InteractionObject>();
                Debug.Log(InterObj);
                InterObj.value = -1;
                //상호작용 오브젝트는 반드시 중앙좌표에 존재합니다.
            
                obj.transform.localPosition = new(7.5f, 7.5f);
                //모든 상호작용 설치 준비가 완료되었음으로 해당 상호작용 유닛을 초기화합니다

                InterObj.InteractSet();
            }
            


        }
        
    }

    /// <summary>
    /// 데이터를 받아서 초기화합니다. 로드에서 작동합니다.
    /// </summary>
    /// <param name="saveData"></데이터 파일 Json파싱>
    public void LoadData(TileMapInfoSaveData saveData)
    {
        
        Clear = saveData.Clear;
        MoveCheck = saveData.MoveCheck;
        GameObject obj = null;
        switch (saveData.Type)
        {
            case InteractionObject.Type.Clear:
                obj = Instantiate<GameObject>(Resources.Load<GameObject>("Prefabs/Interaction/ClearUnit"), gameObject.transform);
                ClearObject clearObject = obj.GetComponentInChildren<ClearObject>();
                clearObject.clear = saveData.Clear;
                clearObject.battle =saveData.battle;
                clearObject.victory = saveData.victory;
                for(int i =0; i<saveData.interMonsterNameList.Count;i++)
                {
                    MonsterScriptableObject monsterdata = Resources.Load<MonsterScriptableObject>("ScriptableObjects/monster_data/"+saveData.interMonsterNameList[i]);
                    clearObject.monsterList.Add(monsterdata);
                }
                clearObject.LampValue = saveData.lampValue;
                clearObject.ClearValue =saveData.clearValue; 
                break;
        }
        if (obj == null)
            return;
        InterObj = obj.GetComponent<InteractionObject>();
        InterObj.value = difficult * 10;
        InterObj.gameObject.SetActive(saveData.interObjSee);
    }

}
