using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class FileBall : MagicAcion
{
    private void Start()
    {

        endtime = 2f;

    }
    private void Update()
    {
        time += Time.deltaTime;
        if (time > endtime)
        {
            endAction();
        }
    }
    public override void MagicAnimation()
    {
        // FireBall에 대한 애니메이션 구현
        Debug.Log("FireBall 애니메이션 실행");

    }

    public override void MagicSetDamage()
    {
        // FireBall에 대한 데미지 설정 구현
        Debug.Log("FireBall 데미지 설정");
    }

    private void endAction()
    {
        Destroy(gameObject);

    }
}
