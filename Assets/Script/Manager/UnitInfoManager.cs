using System.Collections;
using System.Collections.Generic;
using System.Threading;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class UnitInfoManager : MonoBehaviour
 
{

    private MonsterUnit TargetUnit;

    [SerializeField]
    private RectTransform PanelRect;

    /// <summary>
    /// 클릭한 유닛의 정보를 보여주는 패널
    /// </summary>
    [SerializeField]
    private GameObject UnitInfoInterPanel;

    [SerializeField]
    private GameObject UnitInfoActionPanel;


    /// <summary>
    /// 현재 존재하는 몬스터의 이미지 패널
    /// </summary>
    [SerializeField]
    private Image[] MonsterImageArray;

    /// <summary>
    /// 몬스터 AI에서 받아온 몬스터의 목록값.
    /// </summary>
    private List<MonsterUnit> MonsterUnitArray = new List<MonsterUnit>();


    [SerializeField]
    private List<MonsterActionInterFace> MonsterActionInterFaces = new List<MonsterActionInterFace>();


    /// <summary>
    /// 대상자 이름
    /// </summary>

    [SerializeField]
    private TextMeshProUGUI targetName;


    [SerializeField]
    private Image targetHpbar;

    [SerializeField]
    private TextMeshProUGUI targetHpText;



    private void Update()
    {
        if (GameManager.instance.IsPlayer == false)
            return;


        if (Input.GetButtonDown("Cancel"))
        {
            if(UnitInfoInterPanel.activeSelf)
                TargetUnitRemove();

            if(UnitInfoActionPanel.activeSelf)
            {
                UnitInfoActionPanel.gameObject.SetActive(false);
                UnitInfoInterPanel.gameObject.SetActive(true);
            }


        }
    }

  
    public void MonsterInfoAdd(MonsterUnit monster)
    {
        for (int i = 0; i < MonsterImageArray.Length; i++)
        {
            if (MonsterImageArray[i].gameObject.activeSelf == false)
            {
                MonsterImageArray[i].gameObject.SetActive(true);
                MonsterImageArray[i].sprite = monster.Sprite;
                MonsterUnitArray.Add(monster);
                break;
            }

        }

        PanelRect.anchoredPosition = new Vector2((-50 * MonsterUnitArray.Count - 1), 0);

    }
    public void MonsterInfoRemove(MonsterUnit monster)
    {
        for (int i = 0; i < MonsterUnitArray.Count; i++)
        {
            if (MonsterUnitArray[i] == monster)
            {
                MonsterImageArray[i].gameObject.SetActive(false);
                MonsterImageArray[i].sprite = null;
                MonsterUnitArray.Remove(monster);
                break;
            }


        }

        PanelRect.anchoredPosition = new Vector2((-50 * MonsterUnitArray.Count - 1), 0);
    }
    
    /// <summary>
    /// 유닛을 선택했을때 해당 유닛의 정보값을 보여줍니다.
    /// 기본적으로 다른 유닛을 선택할때 마다 스텟정보값으로 전환됩니다.
    /// </summary>
    /// <param name="targetUnit"></param>
    public void  TargetUnitSet(MonsterUnit targetUnit)
    {
        if(this.TargetUnit != null)
            this.TargetUnit.SpriteRenderer.color = Color.white;

        this.TargetUnit = targetUnit;
        this.TargetUnit.SpriteRenderer.color = Color.blue;
        UnitInfoInterPanel.gameObject.SetActive(true);


        CameraSetting.instance.unitFocusSet(TargetUnit.transform.position);
        TargetMonsterActionPredict();



    }
    public void TargetUnitSet(int i)
    {
        if (this.TargetUnit != null)
            this.TargetUnit.SpriteRenderer.color = Color.white;

        this.TargetUnit = MonsterUnitArray[i];
        this.TargetUnit.SpriteRenderer.color = Color.blue;
        UnitInfoInterPanel.gameObject.SetActive(true);
        CameraSetting.instance.unitFocusSet(TargetUnit.transform.position);

        TargetMonsterActionPredict();
    }

    private void TargetUnitRemove()
    {
        if(TargetUnit != null)
        {
            this.TargetUnit.SpriteRenderer.color = Color.white;
            TargetUnit = null;
            UnitInfoInterPanel.SetActive(false);
            GameManager.instance.BlockModeZone.ModeSetting(false);
        }

    }
    

    public void TagetMonsterActionInfoSet()
    {
        UnitInfoActionPanel.gameObject.SetActive(true);
        UnitInfoInterPanel.gameObject.SetActive(false);


        for (int i =0; i<TargetUnit.AttackMagicArray.Length; i++)
        {
            MonsterActionInterFaces[i].gameObject.SetActive(true);
            MonsterActionInterFaces[i].InteFaceSet(TargetUnit.AttackMagicArray[i]);

        }
        if (TargetUnit.DefenceMagicArray.Length == 0)
            return;

        for(int i =-1; i<TargetUnit.DefenceMagicArray.Length; i++)
        {
            MonsterActionInterFaces[i+ TargetUnit.AttackMagicArray.Length].gameObject.SetActive(true);
            MonsterActionInterFaces[i + TargetUnit.AttackMagicArray.Length].InteFaceSet(TargetUnit.DefenceMagicArray[i]);
        }
        
    }


    /// <summary>
    /// 대상이 행동을 할때 어떻게 행할지 필드에 보여줍니다.
    /// </summary>

    public void TargetMonsterActionPredict()
    {
        
        targetName.text = TargetUnit.RatioStatus.MosterName;
        targetHpbar.fillAmount = TargetUnit.status.Health / TargetUnit.status.MaxHealth;
        targetHpText.text = TargetUnit.status.Health.ToString() + " / " + TargetUnit.status.MaxHealth.ToString();
        GameManager.instance.BlockModeZone.unitBlockSet(TargetUnit);
    }

    
}
