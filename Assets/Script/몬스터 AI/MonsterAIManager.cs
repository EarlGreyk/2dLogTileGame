using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterAIManager : MonoBehaviour
{
    // Start is called before the first frame update
    private List<MonsterUnit> monsters = new List< MonsterUnit>();
    public List<MonsterUnit> Monsters { get { return monsters; } }
    private Queue<MonsterUnit> actionMonsters = new Queue<MonsterUnit>();

    private MonsterUnit currentMonster;
    public MonsterUnit CurrentMonster { get { return currentMonster; } set { currentMonster = value; } }


    public void MonsterSet(MonsterUnit monster)
    {
        monsters.Add(monster);
    }


    public void MonsterRevmoe(MonsterUnit monster)
    {
        monsters.Remove(monster);
        GameManager.instance.GameProsessManager.killMonsterAdd(monster.Icon.name);
        if (monsters.Count == 0)
        {
            GameManager.instance.GameProsessManager.ProsessSet();


        }
    }

    private void Update()
    {
        if (currentMonster == null && actionMonsters.Count > 0)
        {
            AiEnable();
        }

        if(Input.GetKeyDown(KeyCode.P))
        {
            Debug.Log("���� óġ ���");
            MonsterRevmoe(monsters[monsters.Count - 1]);

        }
            
    }


    /// <summary>
    /// ������ �ൿ ī��Ʈ�� ���ҽ�ŵ�ϴ�.
    /// ���� ī��Ʈ�� ���ҽ������� 0�� �� ���Ͱ� �ִٸ� �ش� ���͸� Ȱ��ȭ ���� ����Ʈ �׷쿡 �ֽ��ϴ�.
    /// </summary>
    public void MonsterCount()
    {
        for (int i = 0; i < monsters.Count; i++)
        {
            monsters[i].ActionCount--;
            if (monsters[i].ActionCount == 0)
            {
                actionMonsters.Enqueue(monsters[i]);   
            }
        }
    }

    public void AiEnable()
    {
        if (actionMonsters.Count > 0)
        {
            GameManager.instance.onMonsterAction();
            currentMonster = actionMonsters.Peek();
            actionMonsters.Dequeue();
            currentMonster.monsterAction();

        }
            
    }





}
