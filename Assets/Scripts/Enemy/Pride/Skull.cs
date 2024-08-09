using UnityEngine;

public class Skull : GeneralMonsterTest
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
        Debug.Log("AttackSkull");
        base.Attack();
    }
}