using UnityEngine;

public class Pebble : GeneralMonsterTest
{
    protected override void StateInit()
    {
        base.StateInit();
        
    }
    protected override bool TransitionCheck()
    {
        return base.TransitionCheck();
    }

    protected override void Attack()
    {
        Debug.Log("AttackPebble");
        base.Attack();
    }
}
