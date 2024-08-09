using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public enum KingSlimeKind
{
    Green,
    Red,
    Blue
}

public class KingSlimeAI : MonoBehaviour, IDamageable
{
    public event Action OnBossSlimeDeath;
    
    [Header("Ref")]
    public Transform playerTransform;
    public GameObject blueSlimeBullet;
    private SpriteRenderer sprite;
    private Rigidbody2D rb;
    private Animator animator;

    [Header("Common")] 
    public bool isReady = false;
    public int phase = 1;
    public float hp = 100f;
    public float maxHp = 100f;
    public float moveSpeed = 2f;
    public float stunGauge = 1f;
    public float stunTime = 5f;
    private float attackDamage = 3f;
    public float AttackDamage => attackDamage;
    
    public float attackSpeed;
    public float preAttackCT;
    public float postAttackCT;
    public KingSlimeKind kingSlimeKind;
    
    [Header("Green")]
    public float attackGreenSpeed = 10f;
    public float attackGreenDamage = 5f;
    public float preAttackGreenCT = 0.5f;
    public float postAttackGreenCT = 1f;
    
    [Header("Red")]
    public float attackRedSpeed = 30f;
    public float attackRedDamage = 10f;
    public float preAttackRedCT = 2f;
    public float postAttackRedCT = 3f;
    
    [Header("Blue")]
    public float attackBlueSpeed = 2f;
    public float attackBlueDamage = 5f;
    public float preAttackBlueCT = 2f;
    public float postAttackBlueCT = 1f;
    
    [Header("Obstacle")]
    public float ObstacleRaycastDistance = 5f;
    public LayerMask groundLayer;
    
    private SlimeState currentState = SlimeState.Idle;
    private Vector2 moveDirection = Vector2.left; // 초기 이동 방향
    private Color currentColor = Color.white;
    
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        sprite = GetComponent<SpriteRenderer>();
        groundLayer = 1 << 7;
    }

    private void Start()
    {
        sprite.color = Color.white;
        TurnToPlayer();
    }

    private void Update()
    {
        if (!isReady) return;
        
        switch (currentState)
        {
            case SlimeState.Idle:
                Idle();
                break;
        }
    }

    private void LateUpdate()
    {
        if(!isReady) return;
        if (DetectObstacle() == true)
        {
            if (currentState == SlimeState.Idle)
            {
                TurnBack();
            }
        }
    }

    public void ReadyEvent()
    {
        isReady = true;
        
    }

    private void TurnToPlayer()
    {
        if (isReady)
        {
            moveDirection = (playerTransform.position.x >= transform.position.x) ? Vector2.right : Vector2.left;
        }
        
        Invoke("TurnToPlayer", 2f);
    }
    
    private void Idle()
    {
        rb.transform.Translate( moveSpeed * Time.deltaTime * moveDirection);
    }
    
    private void TurnBack()
    {
        moveDirection = -moveDirection;
        sprite.flipX = (moveDirection.x < 0 ) ? true : false;
    }

    private bool DetectObstacle()
    {
        RaycastHit2D hit = Physics2D.Raycast(rb.transform.position, moveDirection, ObstacleRaycastDistance, groundLayer);
        Debug.DrawRay(rb.transform.position, (moveDirection) * ObstacleRaycastDistance, Color.blue);
        
        if (hit.collider != null) return true;

        return false;
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (currentState == SlimeState.Death) return;
        
        
        if (other.gameObject.layer == 9)
        {
            TurnBack();
        }
        
        if (other.gameObject.layer == 6)
        {
            TurnBack();
            var pushDir = new Vector2(other.transform.position.x - transform.position.x >= 0 ? 1 : -1, 1).normalized;
            
            other.gameObject.GetComponent<PlayerController>().GetDamaged(AttackDamage, this.gameObject, 50 * pushDir);
        }
        
    }
    
    
    public void StopRoulette()
    {
        animator.SetTrigger("PreAttack");
    }
    
    public void ResetEvent()
    {
        currentState = SlimeState.Idle;
        var randomTime = Random.Range(15, 50);
        //Debug.Log((float)randomTime/10);
        Invoke("StopRoulette", (float)randomTime/10);
    }

    public void SetAttackTrue(){ animator.SetBool("Attack", true);}
    public void SetAttackFalse(){ animator.SetBool("Attack", false);}

    public void PreAttackEvent()
    {
        currentState = SlimeState.Attack;
        Invoke("SetAttackTrue", preAttackCT);
    }

    public void SyncCommonData(float atkSpeed, float atkDamage, float preAtkCT, float postAtkCT)
    {
        attackSpeed = atkSpeed * phase;
        attackDamage = atkDamage * phase;
        preAttackCT = preAtkCT / phase;
        postAttackCT = postAtkCT / phase;
        
        PreAttackEvent();
    }
    
    public void PreAttackGreenEvent()
    {
        kingSlimeKind = KingSlimeKind.Green;
        SyncCommonData(attackGreenSpeed, attackGreenDamage, preAttackGreenCT, postAttackGreenCT);
        
    }
    public void PreAttackRedEvent()
    {
        kingSlimeKind = KingSlimeKind.Red;
        SyncCommonData(attackRedSpeed, attackRedDamage, preAttackRedCT, postAttackRedCT);
    }
    public void PreAttackBlueEvent()
    {
        kingSlimeKind = KingSlimeKind.Blue;
        SyncCommonData(attackBlueSpeed, attackBlueDamage, preAttackBlueCT, postAttackBlueCT);
    }
    

    public void PostAttackEvent()
    {
        Invoke("SetAttackFalse", postAttackCT);
    }
    
    private void AttackEvent()
    {
        if (playerTransform.position != null)
        {
            Vector2 attackDir = new Vector2(playerTransform.position.x - rb.position.x, playerTransform.position.y - rb.position.y).normalized;
            sprite.flipX = (attackDir.x < 0) ? true : false;

            if (kingSlimeKind == KingSlimeKind.Blue)
            {
                for (int i = 0; i < 5 * phase; i++)
                {
                    GameObject bullet = Instantiate(blueSlimeBullet, transform.position + new Vector3(moveDirection.x, 0.5f * i, 0) * 3, Quaternion.identity);
                    bullet.transform.localScale *= 3;
                    bullet.gameObject.GetComponent<Rigidbody2D>().AddForce(  attackSpeed * new Vector3(moveDirection.x, 0, 0) + Vector3.up , ForceMode2D.Impulse);
                }
            }

            else
            {
                rb.AddForce(  attackSpeed * (attackDir + Vector2.up) , ForceMode2D.Impulse);
            }
            
            
            SoundManager.instance.PlaySound("Slime_Jump", transform.position);
        }
    }

    public void ResetStunGauge()
    {
        stunGauge = 1;
        animator.SetBool("Stun", false);
    }

    public void GetStunned()
    {
        animator.SetBool("Stun", true);
        Invoke("ResetStunGauge", stunTime);
        currentState = SlimeState.Hurt;
        SetAttackFalse();
    }
    
    public void ReduceStunGauge(float gauge)
    {
        if (stunGauge > 0)
        {
            stunGauge -= gauge;

            if (stunGauge <= 0)
            {
                GetStunned();
            }
        }
    }
    
    public void GetDamaged(float damage)
    {
        if (damage <= 0) return;
        if (currentState == SlimeState.Death) return;
        
        //animator.SetTrigger("Hurt");
        //currentState = SlimeState.Hurt;
        SoundManager.instance.PlaySound("Slime_Damaged", transform.position);
        
        this.hp -= damage;
        
        ReduceStunGauge(0.1f);

        //Phase 1 & half Hp (condition)
        if (phase == 1 && hp / maxHp <= 0.5)
        {
            //Set to Phase 2
            phase = 2;
            moveSpeed = 6;
            animator.SetFloat("Phase", 2);
            currentColor = Color.gray;
        }

        //Phase 2
        if (phase >= 2)
        {
            //Phase 2 & 10% Hp(condition)
            if (hp / maxHp <= 0.1)
            {
                currentColor = Color.black;
            }
        }
        
        
        sprite.color = Color.red;
        Invoke("ReturnColor", 0.1f);
        UIManager.instance.hitDamageInfo.PrintHitDamage(transform, damage);
        
        if (hp <= 0)
        {
            Die();
        }
    }

    public void ReturnColor()
    {
        sprite.color = currentColor;
    }
    
    public void Die()
    {
        sprite.color = Color.black;
        currentState = SlimeState.Death;
        animator.SetTrigger("Death");
        SoundManager.instance.PlaySound("Slime_Destroyed", transform.position);
        
        // 보스 슬라임이 죽었을 때 이벤트 발생
        if (OnBossSlimeDeath != null)
        {
            OnBossSlimeDeath.Invoke();
        }
    }

    public void DestroyEvent()
    {
        // openDoor();
        Destroy(this.gameObject);
    }

    public void SetReady()
    {
        isReady = true;
        animator.SetTrigger("Ready");
    }

    // public void openDoor()
    // {
    //     GameObject.Find("Stage_1").transform.Find("Boss Stage").gameObject.SetActive(false);
    // }
}
