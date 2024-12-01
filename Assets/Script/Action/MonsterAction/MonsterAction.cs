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


    private MonsterMagic currentmagic;
    public MonsterMagic currentMagic { get { return currentmagic; } set { currentmagic = value; } }

    private List<Unit> hitunits = new List<Unit>();

    private List<Vector3Int> hitPos = new List<Vector3Int>();



    
    /// <summary>
    /// �׼��� ����ǰ� ������ ���� , ��ǥ Ž��
    /// </summary>
    public void onAction()
    {
        if (currentmagic == null)
            return;
        StartCoroutine(Action());

        if(currentMagic.AoeType == MonsterMagic.MagicAoeType.Target)
        {
            List<PatternData.PatternPoint> pattern = currentMagic.MagicAoe.points;
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
        }else if(currentMagic.AoeType == MonsterMagic.MagicAoeType.LocAoe)
        {
            List<PatternData.PatternPoint> pattern = currentMagic.MagicAoe.points;
            int lengthX = GameManager.instance.BattleZone.BattleTiles.GetLength(0);
            int lengthY = GameManager.instance.BattleZone.BattleTiles.GetLength(1);
            for(int i =0; i<unit.TargetPosList.Count;i++)
            {
                foreach (var pos in pattern)
                {
                    //�߰����� 2,2�̱� ������ -2�� ����
                    int x = unit.TargetPosList[i].x;
                    int y = unit.TargetPosList[i].y;
                    Vector3Int tilepos = new Vector3Int(x, y);

                    if (Math.Abs(x) <= lengthX && Math.Abs(y) <= lengthY)
                    {
                        if (GameManager.instance.BattleZone.BattleTiles[x, y].type == BattleTile.tileType.Break)
                        {
                            Debug.Log($"position ({x}, {y}) �ȵ�! ");
                        }
                        else
                        {
                            Debug.Log($"position ({x}, {y}) ���� �� ��������. ");
                            if (GameManager.instance.BattleZone.BattleTiles[x, y].onUnit == null)
                                hitPos.Add(tilepos);
                        }


                    }
                    else
                    {
                        Debug.Log($"Ÿ�ϸ� �ش� ��ǥ�� {x},{y} �� Ÿ�ϸ� �ۿ� ������");
                    }

                }
            }
           
        }
        else
        {
            Debug.Log($"Ÿ���� �������� �ʽ��ϴ�.");
        }
        
        
    }



    /// <summary>
    /// �׼��� �Ϸ�Ǿ����� �ش� �׼��� ȿ�� ����.
    /// 
    /// </summary>
    public void effectAction()
    {
        //���� ������ ���������� �������� ���� üũ�Ͽ� �׿� �´� ȿ�����ο� 
        //�ӽ������� ���������� ��.
        if(currentMagic.Type == MonsterMagic.MagicType.Attack)
        {
            for (int i = 0; i < hitunits.Count; i++)
            {
                if (hitunits[i] == GameManager.instance.PlayerUnit)
                    hitunits[i].HitDamage(currentMagic.MagicValue);
            }
        }else if(currentMagic.Type == MonsterMagic.MagicType.Surport)
        {

        }else if(currentMagic.Type == MonsterMagic.MagicType.Summon)
        {
            Debug.Log($"���� ���� ���� : {hitPos.Count}");
            for(int i =0; i <hitPos.Count; i++)
            {
                int random = Random.Range(0, currentmagic.MagicSumonPrefabs.Count);
                GameManager.instance.setMonster(hitPos[i], currentmagic.MagicSumonPrefabs[random],true);
            }
        }

        hitunits.Clear();
        hitPos.Clear();
    }
    public void endAction()
    {
        unit.ActionCheck();
    }




    /// <summary>
    /// �׼ǿ� ����ϴ� �ڷ�ƾ ���Դϴ�.
    /// </summary>
    public IEnumerator Action()
    {
        List<string> text = new List<string>() { "�ൿ��...", "0" };
        unit.hpbar.ActionSet(text);
        yield return new WaitForSeconds(currentmagic.MagicTime);
        effectAction();
        endAction();
        yield break;
    }

   

    
    
    
    


}
