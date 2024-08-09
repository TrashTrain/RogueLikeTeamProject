using System;
using UnityEngine;

public class Demon : GeneralMonsterTest
{
    public ParticleSystem attackParticle;

    protected float startTime;

    protected Vector3 originalParticlePosition;
    protected FSMState readyState;

    protected override void Start()
    {
        base.Start();
        originalParticlePosition = attackParticle.transform.localPosition;
    }
    
    protected override void StateInit()
    {
        base.StateInit();
        readyState = new FSMState( ReadyEnter, ReadyUpdate, null);
    }
    
    protected override bool TransitionCheck()
    {
        if (currentState == idleState)
        {
            if (FindTarget)
            {
                FindTarget = false;
                return true;
            }
        }

        if (currentState == readyState)
        {
            if (nextState == attackState)
            {
                return true;
            }
        }
        
        if (currentState == attackState)
        {
            if (nextState == idleState)
            {
                return true;
            }
        }
        
        return false;
    }

    protected override void IdleEnter()
    {
        base.IdleEnter();
        Debug.Log("Demon Idle Enter");
    }

    protected virtual void ReadyEnter()
    {
        animator.SetTrigger("Ready");
        startTime = Time.time;
        Debug.Log("Demon Ready Enter");
    }
    
    protected virtual void ReadyUpdate()
    {
        if(Time.time - startTime < 2f)
        {
            //2초 대기 후 idle 상태로
        }
        else
        {
            nextState = attackState;
        }
    }

    
    #region AttackState

    protected override void AttackEnter()
    {
        base.AttackEnter();
        startTime = Time.time;
        Debug.Log("Demon Attack Enter");
        
        // sound effect
        SFXManager.Instance.PlaySound(SFXManager.Instance.demonAttack);
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
    
    //
    protected override void Attack()
    {
        base.Attack();
        Invoke("DemonAttack", 0.4f);

        attackParticle.transform.localPosition = new Vector3(((generalMonsterData.targetTransform.position.x < transform.position.x) ? -originalParticlePosition.x : originalParticlePosition.x), originalParticlePosition.y, originalParticlePosition.z);
        attackParticle.transform.localScale = new Vector3(((generalMonsterData.targetTransform.position.x < transform.position.x) ? -1 : 1), 1, 1);
    }
    
    public void DemonAttack()
    {
        if (currentState == attackState)
        {
            attackParticle.Play();
        }
    }
    //
    #endregion
    

    protected override void CheckTarget()
    {
        if ( currentState != idleState) return;

        Collider2D target = Physics2D.OverlapCircle(rb.position,  generalMonsterData.recognizeRadius,  generalMonsterData.targetLayer);
        if (target != null)
        {
            generalMonsterData.targetTransform = target.transform;
            nextState = readyState;
            FindTarget = true;
        }
        
        Invoke("CheckTarget", 1f);
    }
}
