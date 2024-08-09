using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EarthGolem : GeneralMonsterTest
{
    public GameObject attackPrefab;
    public int attackCount = 4;
    
    protected float startTime;
    protected Vector3 originalParticlePosition;
    protected FSMState readyState;
    
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
        Debug.Log("Flame Idle Enter");
    }

    protected virtual void ReadyEnter()
    {
        animator.SetTrigger("Ready");
        startTime = Time.time;
        Debug.Log("Flame Ready Enter");
    }
    
    protected virtual void ReadyUpdate()
    {
        if(Time.time - startTime < 2f)
        {
            //2초 대기 후 attack 상태로
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
        Debug.Log("Flame Attack Enter");
    }


    protected override void AttackUpdate()
    {
        base.AttackUpdate();
        
        if(Time.time - startTime < 3f)
        {
            //3초 대기 후 idle 상태로
        }
        else
        {
            nextState = idleState;
        }
    }
    

    protected override void Attack()
    {
        base.Attack();
        //attackParticle.Play();
        //Invoke("EarthAttack", 0.4f);
        StartCoroutine(EarthAttack());

    }
    
    
    IEnumerator EarthAttack()
    {
        if (currentState == attackState)
        {
            yield return new WaitForSeconds(0.5f);
            
            for (int i = 1; i <= attackCount; i++)
            {
                Vector3 spawnPosition = transform.position; // 프리팹 생성 위치
                var attack = Instantiate(attackPrefab, 
                    spawnPosition + ( (transform.position.x < generalMonsterData.targetTransform.position.x) ? 1 : -1 ) * 2.5f * i * Vector3.right  
                                  + Vector3.up * 1f, Quaternion.identity);
                attack.transform.localScale = 0.75f * Vector3.one;
                attack.SetActive(true);
                yield return new WaitForSeconds(0.3f);
            }
        }
    }
    
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
