using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

public class MonsterAction : MonoBehaviour
{
    private MonsterUnit unit;

    public MonsterUnit Unit { get { return unit; } set { unit = value; } }


    private MonSterMagicScriptableObejct currentmagic;
    public MonSterMagicScriptableObejct currentMagic { get { return currentmagic; } set { currentmagic = value; } }

    private List<Unit> hitunits = new List<Unit>();

    private List<Vector3Int> hitPos = new List<Vector3Int>();



    
    /// <summary>
    /// 액션이 실행되고 유닛의 연출 , 목표 탐색
    /// </summary>
    public void onAction()
    {
        Debug.Log("몬스터 액션!!!");
        if (currentmagic == null)
            return;
        StartCoroutine(Action());
        if(currentMagic.Operating_type == 0)
        {
            List<PatternData.PatternPoint> pattern = currentMagic.MagicCastingRange.points;
            int lengthX = GameManager.instance.BattleZone.BattleTiles.GetLength(0);
            int lengthY = GameManager.instance.BattleZone.BattleTiles.GetLength(1);
            foreach (var pos in pattern)
            {
                int x = pos.x - 3 + unit.TargetPosList[0].x;
                int y = pos.y - 3 + unit.TargetPosList[0].y;
                Vector3Int tilepos = new Vector3Int(x, y);

                if (Math.Abs(x) <= lengthX && Math.Abs(y) <= lengthY && x>=0 && y>=0)
                {
                    if (GameManager.instance.BattleZone.BattleTiles[x, y].type == BattleTile.tileType.Break)
                    {
                    }
                    else
                    {   
                        if (GameManager.instance.BattleZone.BattleTiles[x, y].onUnit != null)
                            hitunits.Add(GameManager.instance.BattleZone.BattleTiles[x, y].onUnit);
                    }

                }
                else
                {
                    Debug.Log($"타일맵 해당 좌표값 {x},{y} 은 타일맵 밖에 존재함");
                }

            }
        }
        else
        {
            Debug.Log($"타입이 존재하지 않습니다.");
        }
        
        
    }



    /// <summary>
    /// 액션이 완료되었을때 해당 액션의 효과 적용.
    /// 
    /// </summary>
    public void effectAction()
    {
        
        
        //현재 마법이 데미지인지 버프인지 등을 체크하여 그에 맞는 효과를부여 
        //임시적으로 데미지만을 줌.
        if(currentMagic.Operating_type == 0)
        {
            for (int i = 0; i < hitunits.Count; i++)
            {
                if (hitunits[i] == GameManager.instance.PlayerUnit)
                    hitunits[i].HitDamage(currentMagic.MagicValue);
            }
        }else if(currentMagic.Operating_type == 2)
        {

        }else if(currentMagic.Operating_type == 5)
        {
            Debug.Log($"생성 가능 숫자 : {hitPos.Count}");
            //for(int i =0; i <hitPos.Count; i++)
            //{
            //    int random = Random.Range(0, currentmagic.MagicSumonPrefabs.Count);
            //    GameManager.instance.setMonster(hitPos[i], currentmagic.MagicSumonPrefabs[random],true);
            //}
        }

        
        hitunits.Clear();
        hitPos.Clear();
    }
    public void endAction()
    {
        unit.ReAction();
    }

    /// <summary>
    /// 액션에 사용하는 코루틴 문입니다.
    /// </summary>
    public IEnumerator Action()
    {
        List<string> text = new List<string>() { "행동중...", "0" };
        unit.hpbar.ActionSet(text);
        yield return new WaitForSeconds(currentmagic.MagicTime);
        effectAction();
        endAction();
        yield break;
    }

   

    
    
    
    


}
