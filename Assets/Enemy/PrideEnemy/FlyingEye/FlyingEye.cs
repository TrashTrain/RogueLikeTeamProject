using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class FlyingEye : GeneralMonsterTest
{
    protected float startTime;
    // -----
    private Transform laserTrans;
    // -----

    protected override void IdleEnter()
    {
        base.IdleEnter();
        Debug.Log("FlyingEye Idle Enter");
    }

    #region AttackState

    protected override void AttackEnter()
    {
        base.AttackEnter();
        base.CheckTarget();
        startTime = Time.time;
        Debug.Log("FlyingEye Attack Enter");
        
        // ----------
        
        laserTrans = transform.Find("Body/Laser/Line");
        if (laserTrans == null)
        {
            Debug.LogError("Laser not found!");
        }
        
        laserTrans.GetComponent<FlyingEyeLaser>().ActiveState();
        // ----------
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
            
            // -----
            laserTrans = transform.Find("Body/Laser/Line");
            if (laserTrans == null)
            {
                Debug.LogError("Laser not found!");
            }
        
            laserTrans.GetComponent<FlyingEyeLaser>().DisabledState();
            // -----

        }
    }
    

    protected override void Attack()
    {
        base.Attack();
       
    }

    public bool ExistTarget()
    {
        if (FindTarget == true)
        {
            return true;
        }
        return false;
    }

    #endregion
}
