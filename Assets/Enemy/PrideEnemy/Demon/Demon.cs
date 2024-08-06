using UnityEngine;

public class Demon : GeneralMonsterTest
{
    protected float startTime;

    protected override void IdleEnter()
    {
        base.IdleEnter();
        Debug.Log("Demon Idle Enter");
    }

    #region AttackState

    protected override void AttackEnter()
    {
        base.AttackEnter();
        startTime = Time.time;
        Debug.Log("Demon Attack Enter");
    }


    protected override void AttackUpdate()
    {
        base.AttackUpdate();
        
        if(Time.time - startTime < 2f)
        {
            //2초 대기 후 idle 상태로
        }
        else
        {
            nextState = idleState;
        }
    }
    

    protected override void Attack()
    {
        base.Attack();
        
    }

    #endregion
}
