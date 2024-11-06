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
    /// �׼��� ����ǰ� ������ ���� �� ����.
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
            //�߰����� 2,2�̱� ������ -2�� ����
            Debug.Log(unit);
            Debug.Log(unit.TargetPos);
            int x = pos.x - 2 + unit.TargetPos.x;
            int y = pos.y - 2 + unit.TargetPos.y;
            Vector3Int tilepos = new Vector3Int(x, y);

            if (Math.Abs(x) <= lengthX && Math.Abs(y) <= lengthY)
            {
                if (GameManager.instance.BattleZone.BattleTiles[x, y].type == BattleTile.tileType.Break)
                {
                    Debug.Log($"position ({x}, {y}) �ȵ�! ");
                }
                else
                {
                    Debug.Log($"position ({x}, {y}) ���ݰ���. ");
                    if (GameManager.instance.BattleZone.BattleTiles[x, y].onUnit != null)
                        hitunits.Add(GameManager.instance.BattleZone.BattleTiles[x, y].onUnit);
                }


            }
            else
            {
                Debug.Log($"Ÿ�ϸ� �ش� ��ǥ�� {x},{y} �� Ÿ�ϸ� �ۿ� ������");
            }

        }
        
    }


    /// <summary>
    /// �׼��� �Ϸ�Ǿ����� �ش� �׼��� ȿ�� ����.
    /// 
    /// </summary>
    private void effectAction()
    {
        //���� ������ ���������� �������� ���� üũ�Ͽ� �׿� �´� ȿ�����ο� 
        //�ӽ������� ���������� ��.
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
    /// �׼ǿ� ����ϴ� �ڷ�ƾ ���Դϴ�.
    /// </summary>
    private IEnumerator Action()
    {
        List<string> text = new List<string>() { "�ൿ��...", "0" };
        unit.hpbar.ActionSet(text);
        yield return new WaitForSeconds(currentmagic.MagicTime);
        effectAction();
        endAction();
        yield break;
    }

   

    
    
    
    


}
