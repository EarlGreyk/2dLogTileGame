using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MonsterAction : MonoBehaviour
{
    private MonsterUnit unit;

    public MonsterUnit Unit { get { return unit; } set { unit = value; } }


    private MonsterMagic currentmagic;
    public MonsterMagic currentMagic { get { return currentmagic; } set { currentmagic = value; } }

    private List<Unit> hitunits = new List<Unit>();


    
    /// <summary>
    /// 액션이 실행되고 유닛의 연출 및 동작.
    /// </summary>
    public void onAction()
    {
        if (currentmagic == null)
            return;
        StartCoroutine(Action());
        List<PatternData.PatternPoint> pattern = currentMagic.MagicAoe.points;
        int lengthX = GameManager.instance.BattleZone.BattleTiles.GetLength(0);
        int lengthY = GameManager.instance.BattleZone.BattleTiles.GetLength(1);
        foreach (var pos in pattern)
        {
            //중간값이 2,2이기 떄문에 -2씩 연산
            Debug.Log(unit);
            Debug.Log(unit.TargetPos);
            int x = pos.x - 2 + unit.TargetPos.x;
            int y = pos.y - 2 + unit.TargetPos.y;
            Vector3Int tilepos = new Vector3Int(x, y);

            if (Math.Abs(x) <= lengthX && Math.Abs(y) <= lengthY)
            {
                if (GameManager.instance.BattleZone.BattleTiles[x, y].type == BattleTile.tileType.Break)
                {
                    Debug.Log($"position ({x}, {y}) 안됨! ");
                }
                else
                {
                    Debug.Log($"position ({x}, {y}) 공격가능. ");
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


    /// <summary>
    /// 액션이 완료되었을때 해당 액션의 효과 적용.
    /// 
    /// </summary>
    private void effectAction()
    {
        //현재 마법이 데미지인지 버프인지 등을 체크하여 그에 맞는 효과를부여 
        //임시적으로 데미지만을 줌.
        for(int i=0;i<hitunits.Count;i++)
        {
            hitunits[i].HitDamage(currentMagic.MagicValue);
        }
        hitunits.Clear();
    }
    private void endAction()
    {
        unit.ActionCheck();
    }




    /// <summary>
    /// 액션에 사용하는 코루틴 문입니다.
    /// </summary>
    private IEnumerator Action()
    {
        List<string> text = new List<string>() { "행동중...", "0" };
        unit.hpbar.ActionSet(text);
        yield return new WaitForSeconds(currentmagic.MagicTime);
        effectAction();
        endAction();
        yield break;
    }

   

    
    
    
    


}
