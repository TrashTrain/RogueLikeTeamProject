using System;
using UnityEngine;

public class BlueSlimeAI : MonoBehaviour, IDamageable
{
    public GameObject blueSlimeBullet;
    
    public SpriteRenderer sprite;
    public Rigidbody2D rb;
    public Animator animator;
    
    public float moveSpeed = 6f;
    public float attackSpeed = 9f;
    public float hp = 20f;
    
    private float attackDamage = 3f;
    public float AttackDamage => attackDamage;
    
    public float CliffRaycastDistance = 1f; // 발판 끝을 감지하기 위한 레이캐스트 거리
    public float ObstacleRaycastDistance = 1f;
    public float recognizeRadius = 8f;
    public LayerMask groundLayer;
    public LayerMask playerLayer;
    
    private Transform playerTransform;
    
    private SlimeState currentState = SlimeState.Idle;
    private Vector2 moveDirection = Vector2.right; // 초기 이동 방향

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        sprite = GetComponent<SpriteRenderer>();
        groundLayer = 1 << 7;
        playerLayer = 1 << 6;
    }

    private void Start()
    {
        Invoke("CheckPlayer", 1f);
    }

    private void Update()
    {
        switch (currentState)
        {
            case SlimeState.Idle:
                Idle();
                break;
            case SlimeState.Attack:
                //Attack();
                break;
            case SlimeState.Hurt:
                // Hurt 상태 처리 로직 (생략)
                break;
            case SlimeState.Death:
                // Death 상태 처리 로직 (생략)
                break;
        }
    }

    private void LateUpdate()
    {
        // 발판의 끝에 도달하면 이동 방향을 반대로 변경
        if (DetectCliff() == true)
        {
            if (currentState == SlimeState.Idle)
            {
                TurnBack();
            }
            
            rb.velocity = new Vector2(0, rb.velocity.y);
        }

        if (DetectObstacle() == true)
        {
            if (currentState == SlimeState.Idle)
            {
                TurnBack();
            }
        }
    }

    private void TurnBack()
    {
        moveDirection = -moveDirection;
        sprite.flipX = (moveDirection.x < 0 ) ? true : false;
    }

    private bool DetectCliff()
    {
        // 발판의 끝을 감지
        RaycastHit2D hit = Physics2D.Raycast(rb.transform.position, moveDirection + 2* Vector2.down, CliffRaycastDistance, groundLayer);
        Debug.DrawRay(rb.transform.position, (moveDirection + Vector2.down) * CliffRaycastDistance, Color.red);

        if (hit.collider == null) return true;

        return false;
    }

    private bool DetectObstacle()
    {
        RaycastHit2D hit = Physics2D.Raycast(rb.transform.position, moveDirection, ObstacleRaycastDistance, groundLayer);
        Debug.DrawRay(rb.transform.position, (moveDirection) * ObstacleRaycastDistance, Color.blue);
        
        if (hit.collider != null) return true;

        return false;
    }
    
    private void Idle()
    {
        // 이동
        rb.transform.Translate( moveSpeed * Time.deltaTime * moveDirection);
    }

    private void ShootAttack()
    {
        if (playerTransform.position != null)
        {
            Vector2 attackDir = new Vector2(playerTransform.position.x - rb.position.x, playerTransform.position.y - rb.position.y).normalized;
            sprite.flipX = (attackDir.x < 0) ? true : false;

            GameObject bullet = Instantiate(blueSlimeBullet, transform.position+Vector3.up, Quaternion.identity);
            
            bullet.GetComponent<Rigidbody2D>().AddForce(  attackSpeed * attackDir , ForceMode2D.Impulse);
            
            SoundManager.instance.PlaySound("Slime_Jump", transform.position);
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (currentState == SlimeState.Death) return;
        
        // if (other.gameObject.layer == 10)
        // {
        //     GetHurt(2);
        // }
        
        if (other.gameObject.layer == 9)
        {
            TurnBack();
        }
        
        if (other.gameObject.layer == 6)
        {
            other.gameObject.GetComponent<PlayerController>().GetDamaged(AttackDamage, this.gameObject, Vector2.up * 10);
        }
    }
    
    public void IdleEvent()
    {
        currentState = SlimeState.Idle;
    }

    public void GetDamaged(float damage)
    {
        if (damage <= 0) return;
        if (currentState == SlimeState.Death) return;
        
        animator.SetTrigger("Hurt");
        currentState = SlimeState.Hurt;
        SoundManager.instance.PlaySound("Slime_Damaged", transform.position);
        
        this.hp -= damage;
        UIManager.instance.hitDamageInfo.PrintHitDamage(transform, damage);
        
        if (hp <= 0)
        {
            Die();
        }
    }

    public void Die()
    {
        currentState = SlimeState.Death;
        animator.SetTrigger("Death");
        
        SoundManager.instance.PlaySound("Slime_Destroyed", transform.position);
    }

    public void DestroyEvent()
    {
        Destroy(this.gameObject);
    }

    public void CheckPlayer()
    {
        if (currentState == SlimeState.Death) return;
        
        Collider2D collider = Physics2D.OverlapCircle(rb.position, recognizeRadius, playerLayer);
        if (collider == null && currentState == SlimeState.Attack)
        {
            animator.SetTrigger("Idle");
            currentState = SlimeState.Idle;
        }
        else if(collider != null && currentState == SlimeState.Idle)
        {
            playerTransform = collider.transform;
            animator.SetTrigger("Attack");
            currentState = SlimeState.Attack;
        }
        
        Invoke("CheckPlayer", 1f);
    }
}
