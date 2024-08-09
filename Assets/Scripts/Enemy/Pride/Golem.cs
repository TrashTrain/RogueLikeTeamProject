using UnityEngine;

public class Golem : MonoBehaviour, IDamageable
{
    protected GeneralMonsterDataStruct generalMonsterData;

    protected float startTime;
    
    private bool isTransition = false;
    protected FSMState P1_Idle;
    protected FSMState P1_AttackA1;
    protected FSMState P1_AttackA2;
    protected FSMState P1_Hit;
    protected FSMState P1_AtoB;
    protected FSMState P1_RunB;
    protected FSMState P1_AttackB;
    
    protected FSMState attackState;
    protected FSMState deathState;
    
    protected FSMState currentState;
    protected FSMState nextState;

    protected bool FindTarget = false;
    
    [Header("Ref")]
    public SpriteRenderer sprite;
    public Rigidbody2D rb;
    public Animator animator;
    [SerializeField] private GeneralMonsterData refData;
    [SerializeField] private GameObject sonicWavePrefab;
    [SerializeField] private GameObject subMonster;
    
    //Constant Variable
    private const int PlayerLayer = 1 << 6;
    private const int GroundLayer = 1 << 7;

    public Vector2 screenCenter;
    public float maxDistance = 12f;
    public float minDistance = 4f;
    
    
    protected void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        sprite = GetComponent<SpriteRenderer>();
        
        refData.SyncData();
        generalMonsterData = refData.data;
        
        //null check
        if(rb == null) {Debug.LogError($"{this.gameObject.name}(RigidBody2D) is null");}
        if(animator == null) {Debug.LogError($"{this.gameObject.name}(Animator) is null");}
        if(sprite == null) {Debug.LogError($"{this.gameObject.name}(Sprite) is null");}
        if(refData == null) {Debug.LogError($"{this.gameObject.name}(refData) is null");}
        if( generalMonsterData.targetLayer != PlayerLayer) {Debug.LogError($"{this.gameObject.name}(targetLayer is not playerLayer)");}
        
        generalMonsterData.patrolPos = transform.position;
        
        StateInit();
    }

    protected void Start()
    {
        P1_IdleEnter();
    }

    protected void FixedUpdate()
    {
        if (isTransition && currentState != nextState)
        {
            currentState = nextState;
            currentState.OnEnter?.Invoke();
            isTransition = false;
        }
        
        currentState.OnUpdate?.Invoke();
        isTransition = TransitionCheck();
        
        if(isTransition && currentState != nextState) currentState.OnExit?.Invoke();
    }
    
    protected virtual void StateInit()
    {
        deathState = new FSMState(null, null, null);

        P1_Idle = new FSMState(P1_IdleEnter, P1_IdleUpdate, P1_IdleExit);
        P1_AttackA1 = new FSMState(P1_AttackA1Enter, P1_AttackA1Update, P1_AttackA1Exit);
        P1_AttackA2 = new FSMState(P1_AttackA2Enter, P1_AttackA2Update, P1_AttackA2Exit);
        P1_Hit = new FSMState(null, null, null);
        P1_AtoB = new FSMState(P1_AtoBEnter, P1_AtoBUpdate, P1_AtoBExit);
        P1_RunB = new FSMState(P1_RunBEnter, P1_RunBUpdate, P1_RunBExit);
        P1_AttackB = new FSMState(P1_AttackBEnter, P1_AttackBUpdate, P1_AttackBExit);
        
        
        currentState = P1_Idle;
        nextState = P1_Idle;
    }

    protected virtual bool TransitionCheck()
    {
        //AnyState -> ?
        if (currentState != nextState)
        {
            
        }
        
        //P1_Idle -> P1_AttackA1, AttackA2, AtoB
        if (currentState == P1_Idle)
        {
            if (FindTarget && (nextState == P1_AttackA1 || nextState == P1_AttackA2 || nextState == P1_AtoB))
            {
                FindTarget = false;
                return true;
            }
        }
        
        //P1_AtoB -> P1_RunB
        if (currentState == P1_AtoB)
        {
            if (nextState == P1_RunB)
            {
                return true;
            }
        }
        
        //P1_RunB -> P1_AttackB
        if (currentState == P1_RunB)
        {
            if (nextState == P1_AttackB)
            {
                return true;
            }
        }
        
        //P1_AttackA1, AttackA2, AttackB -> P1_Idle
        if (currentState == P1_AttackA1 || currentState == P1_AttackA2 || currentState == P1_AttackB)
        {
            if (nextState == P1_Idle)
            {
                return true;
            }
        }
        
        return false;
    }

    #region P1_Idle

    protected virtual void SetP1_Idle()
    {
        nextState = P1_Idle;
        //isTransition = true;
    }
    protected virtual void P1_IdleEnter()
    {
        Debug.Log("P1_Idle");
        animator.SetBool("P1_Idle", true);
        Invoke("CheckTarget", 1f);
        rb.velocity = Vector2.zero;
        
        screenCenter = new Vector2(Screen.width / 2, Screen.height / 2);
        SetRandomDirection();
    }
    
    protected virtual void P1_IdleUpdate()
    {
        Patrol();
    }

    protected virtual void P1_IdleExit()
    {
        animator.SetBool("P1_Idle", false);
    }
    #endregion

    #region P1_AttackA1

    protected virtual void P1_AttackA1Enter()
    {
        Debug.Log("P1_AttackA1");
        animator.SetTrigger("P1_AttackA1");
        Attack();
        startTime = Time.time;
        
        Invoke("P1_AttackA1Attack", 0.5f);
    }
    
    protected virtual void P1_AttackA1Update()
    {
        sprite.flipX = ( generalMonsterData.targetTransform.position.x < transform.position.x);
        
        if(Time.time - startTime < 0.5f)
        {
            
        }
        else if(Time.time - startTime < 5f)
        {
            Patrol();
        }
        else
        {
            nextState = P1_Idle;
        }
    }
    
    protected virtual void P1_AttackA1Attack()
    {
        if(currentState != P1_AttackA1) return;
        
        GameObject subMon1 = Instantiate(this.subMonster, transform.position + Vector3.up * 2 + Vector3.left * 5, Quaternion.identity);
        GameObject subMon2 = Instantiate(this.subMonster, transform.position + Vector3.up * 2 + Vector3.right * 5, Quaternion.identity);
        
        subMon1.SetActive(true);
        subMon2.SetActive(true);
        
        subMon1.transform.localScale = 5 * Vector3.one;
        subMon2.transform.localScale = 5 * Vector3.one;
    }

    protected virtual void P1_AttackA1Exit()
    {
    }
    #endregion
    
    #region P1_AttackA2

    protected virtual void P1_AttackA2Enter()
    {
        Debug.Log("P1_AttackA2");
        animator.SetTrigger("P1_AttackA2");
        Attack();
        startTime = Time.time;
    }
    
    protected virtual void P1_AttackA2Update()
    {
        if (Time.time - startTime < 1f)
        {
            sprite.flipX = (generalMonsterData.targetTransform.position.x < transform.position.x);

            if (Time.time - startTime + 0.05f > 1f)
            {
                P1_AttackA2Attack();
            }
        }

        else if(Time.time - startTime < 1.5f)
        {
            if (Time.time - startTime + 0.05f > 1.5f)
            {
                P1_AttackA2Attack();
            }
        }

        
        
        if (Time.time - startTime > 2f)
        {
            nextState = P1_Idle;
        }
    }
    
    protected virtual void P1_AttackA2Attack()
    {
        if(currentState != P1_AttackA2) return;
        
        
        //rb.gravityScale = 0f;
        sprite.flipX = (generalMonsterData.targetTransform.position.x < transform.position.x);
        rb.velocity = Vector2.zero;
        
        //var attackDir = sprite.flipX ? Vector2.left : Vector2.right;
        var attackDir = generalMonsterData.targetTransform.position - transform.position;
        rb.AddForce( 600f * attackDir.normalized, ForceMode2D.Impulse);
    }

    protected virtual void P1_AttackA2Exit()
    {
        //rb.gravityScale = 1f;
        rb.velocity = Vector2.zero;
    }
    #endregion
    
    #region P1_AtoB

    protected virtual void SetP1_AtoB()
    {
        nextState = P1_AtoB;
        //isTransition = true;
    }
    protected virtual void P1_AtoBEnter()
    {
        Debug.Log("P1_AtoB");
        animator.SetTrigger("P1_AtoB");
        rb.gravityScale = 0f;
        startTime = Time.time;
    }
    
    protected virtual void P1_AtoBUpdate()
    {
        rb.transform.Translate(4 * Time.deltaTime * Vector2.up);
        if (Time.time - startTime > 2f)
        {
            nextState = P1_RunB;
        }
    }

    protected virtual void P1_AtoBExit()
    {
    }
    #endregion

    #region P1_RunB

    protected virtual void P1_RunBEnter()
    {
        Debug.Log("P1_RunB");
        animator.SetTrigger("P1_RunB");
        startTime = Time.time;
        
        P1_RunBAttack();
    }
    
    protected virtual void P1_RunBUpdate()
    {
        Patrol();
        
        if (Time.time - startTime > 5f)
        {
            nextState = P1_AttackB;
        }
    }

    protected virtual void P1_RunBAttack()
    {
        if(currentState != P1_RunB) return;
        
        var targetPos = generalMonsterData.targetTransform.position;
        Vector2 attackDir = new Vector2(targetPos.x - transform.position.x, targetPos.y - transform.position.y).normalized;
        float angle = Mathf.Atan2(attackDir.y, attackDir.x) * Mathf.Rad2Deg; // 각도 계산
            
        GameObject bullet = Instantiate(sonicWavePrefab, transform.position + transform.forward, Quaternion.Euler(new Vector3(0, 0, angle)));
        bullet.SetActive(true);
        bullet.transform.localScale = Vector3.one;
        bullet.GetComponent<Rigidbody2D>().AddForce(  generalMonsterData.attackSpeed * attackDir , ForceMode2D.Impulse);
        
        Invoke("P1_RunBAttack", 1f);
    }

    protected virtual void P1_RunBExit()
    {
    }

    #endregion
    
    #region P1_AttackB

    protected virtual void SetP1_AttackB()
    {
        nextState = P1_AttackB;
        //isTransition = true;
    }
    
    protected virtual void P1_AttackBEnter()
    {
        Debug.Log("P1_AttackB");
        animator.SetTrigger("P1_AttackB");
        Attack();
        startTime = Time.time;
    }
    
    protected virtual void P1_AttackBUpdate()
    {
        sprite.flipX = ( generalMonsterData.targetTransform.position.x < transform.position.x);
        if (Time.time - startTime >= 0.5f)
        {
            nextState = P1_Idle;
        }
    }

    protected virtual void P1_AttackBExit()
    {
        //rb.gravityScale = 1f;
        //rb.AddForce(200f * Vector2.down, ForceMode2D.Impulse);
    }
    #endregion
    
    
    protected void TurnBack()
    {
        generalMonsterData.moveDirection = - generalMonsterData.moveDirection;
        sprite.flipX = ( generalMonsterData.moveDirection.x < 0 );
    }

    protected void Patrol()
    {
        if (ShouldChangeDirection())
        {
            SetRandomDirection();
        }
        
        rb.transform.Translate( generalMonsterData.moveSpeed * Time.deltaTime *  generalMonsterData.moveDirection);
    }
    
    private bool ShouldChangeDirection()
    {
        if (generalMonsterData.targetTransform == null)
        {
            generalMonsterData.targetTransform = FindObjectOfType<PlayerController>().transform;
        }
            
        // Change direction if the monster is too close or too far from the player
        float distanceToPlayer = Vector2.Distance(transform.position, generalMonsterData.targetTransform.position);
        return generalMonsterData.targetTransform.position.y - 4 >= transform.position.y || distanceToPlayer > maxDistance;
    }
    
    private bool ReachedBoundary()
    {
        // 화면 경계 체크
        // return transform.position.x > generalMonsterData.patrolPos.x + generalMonsterData.patrolDistance / 2 ||
        //        transform.position.x < generalMonsterData.patrolPos.x - generalMonsterData.patrolDistance / 2 ||
        //        transform.position.y > generalMonsterData.patrolPos.y + generalMonsterData.patrolDistance / 2 ||
        //        transform.position.y < generalMonsterData.patrolPos.y - generalMonsterData.patrolDistance / 2;
        return false;
    }

    
    private void SetRandomDirection()
    {
        if (generalMonsterData.targetTransform == null)
        {
            generalMonsterData.targetTransform = FindObjectOfType<PlayerController>().transform;
        }
        
        Vector2 directionToPlayer = (generalMonsterData.targetTransform.position - transform.position).normalized;
        float distanceToPlayer = Vector2.Distance(transform.position, generalMonsterData.targetTransform.position);
        
        if (distanceToPlayer >= maxDistance)
        {
            generalMonsterData.moveDirection = directionToPlayer;
            return;
        }

        if (generalMonsterData.targetTransform.position.y - 4 >= transform.position.y)
        {
            generalMonsterData.moveDirection =
                new Vector2(generalMonsterData.moveDirection.x, generalMonsterData.moveDirection.y >= 0 ? generalMonsterData.moveDirection.y : -generalMonsterData.moveDirection.y);
            
            return;
        }
        
        //Otherwise, calculate a new random direction
        float angle = Random.Range(0f, 360f);
        generalMonsterData.moveDirection = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)).normalized;
    }
    

    protected void CheckTarget()
    {
        if (currentState != P1_Idle) return;
        
        Collider2D target = Physics2D.OverlapCircle(rb.position,  generalMonsterData.recognizeRadius,  generalMonsterData.targetLayer);
        if (target != null)
        {
            generalMonsterData.targetTransform = target.transform;

            var attackPattern = Random.Range(1, 4);
            //Debug.Log(attackPattern);
            //var attackPattern = 3;
            
            if (attackPattern == 1)
            {
                nextState = P1_AttackA1;
            }
            else if(attackPattern == 2)
            {
                nextState = P1_AttackA2;
            }
            else
            {
                nextState = P1_AtoB;
            }
            
            FindTarget = true;
        }
        else
        {
            nextState = P1_Idle;
            FindTarget = false;
        }
        
        Invoke("CheckTarget", 1f);
    }
    
    protected bool DetectObstacle()
    {
        RaycastHit2D hit = Physics2D.Raycast(rb.transform.position,  generalMonsterData.moveDirection, generalMonsterData.obstacleRaycastDistance, GroundLayer);
        Debug.DrawRay(rb.transform.position, ( generalMonsterData.moveDirection) * generalMonsterData.obstacleRaycastDistance, Color.blue);
        
        if (hit.collider != null) return true;

        return false;
    }
    
    protected void OnCollisionEnter2D(Collision2D other)
    {
        if ( currentState == deathState) return;
        

        if (other.gameObject.layer == 6)
        {
            other.gameObject.GetComponent<PlayerController>().GetDamaged( generalMonsterData.attackDamage, this.gameObject,
                (((other.transform.position.x > transform.position.x) ? Vector2.right : Vector2.left) + 0.5f * Vector2.up).normalized *  generalMonsterData.knockBackPower);
        }
    }

    protected virtual void Attack()
    {
        Debug.Log("Attack!");
    }
    
    
    public void GetDamaged(float damage)
    {
        if(damage <= 0) return;
        if( currentState == deathState) return;

        generalMonsterData.hp -= damage;
        UIManager.instance.hitDamageInfo.PrintHitDamage(transform, damage);

        //if(generalMonsterData.targetTransform != null) SetP1_AtoB();
        
        if ( generalMonsterData.hp < 0)
        {
            Destroy(this.gameObject);
        }
    }
}
