// using UnityEngine;
//
// public class GeneralMonster : MonoBehaviour, IDamageable
// {
//     private GeneralMonsterDataStruct generalMonsterData;
//     
//     [Header("Ref")]
//     public SpriteRenderer sprite;
//     public Rigidbody2D rb;
//     public Animator animator;
//     [SerializeField] private GeneralMonsterData refData;
//     
//     //Constant Variable
//     private const int PlayerLayer = 1 << 6;
//     private const int GroundLayer = 1 << 7;
//     
//     protected void Awake()
//     {
//         rb = GetComponent<Rigidbody2D>();
//         animator = GetComponent<Animator>();
//         sprite = GetComponent<SpriteRenderer>();
//         
//         //null check
//         if(rb == null) {Debug.LogError($"{this.gameObject.name}(RigidBody2D) is null");}
//         if(animator == null) {Debug.LogError($"{this.gameObject.name}(Animator) is null");}
//         if(sprite == null) {Debug.LogError($"{this.gameObject.name}(Sprite) is null");}
//         if(refData == null) {Debug.LogError($"{this.gameObject.name}(refData) is null");}
//         if( generalMonsterData.targetLayer != PlayerLayer) {Debug.LogError($"{this.gameObject.name}(targetLayer is not playerLayer)");}
//         
//         refData.SyncData();
//         generalMonsterData = refData.data;
//         
//         generalMonsterData.patrolPos = transform.position;
//     }
//
//     protected void Start()
//     {
//         Invoke("CheckTarget", 1f);
//     }
//
//     protected void FixedUpdate()
//     {
//         if ( generalMonsterData.currentState == GeneralMonsterState.Idle)
//         {
//             Patrol();
//         }
//
//         if ( generalMonsterData.currentState == GeneralMonsterState.Attack)
//         {
//             sprite.flipX = ( generalMonsterData.targetTransform.position.x < transform.position.x);
//         }
//     }
//
//     protected void TurnBack()
//     {
//         generalMonsterData.moveDirection = - generalMonsterData.moveDirection;
//         sprite.flipX = ( generalMonsterData.moveDirection.x < 0 );
//     }
//
//     protected void Patrol()
//     {
//         if (DetectObstacle())
//         {
//             TurnBack();
//         }
//         
//         switch (generalMonsterData.moveDirection.x)
//         {
//             case > 0 when Vector2.Distance(transform.position,
//                 generalMonsterData.patrolPos + Vector2.right * generalMonsterData.patrolDistance / 2) < 0.1f:
//             case < 0 when Vector2.Distance(transform.position, 
//                 generalMonsterData.patrolPos + Vector2.left * generalMonsterData.patrolDistance / 2) < 0.1f:
//                 TurnBack();
//                 break;
//         }
//         
//         rb.transform.Translate( generalMonsterData.moveSpeed * Time.deltaTime *  generalMonsterData.moveDirection);
//     }
//
//     protected void CheckTarget()
//     {
//         if ( generalMonsterData.currentState != GeneralMonsterState.Idle) return;
//
//         Collider2D target = Physics2D.OverlapCircle(rb.position,  generalMonsterData.recognizeRadius,  generalMonsterData.targetLayer);
//         if (target != null)
//         {
//             Attack();
//             generalMonsterData.targetTransform = target.transform;
//             animator.SetTrigger("Attack");
//             generalMonsterData.currentState = GeneralMonsterState.Attack;
//         }
//         
//         Invoke("CheckTarget", 1f);
//     }
//     
//     protected bool DetectObstacle()
//     {
//         RaycastHit2D hit = Physics2D.Raycast(rb.transform.position,  generalMonsterData.moveDirection, generalMonsterData.obstacleRaycastDistance, GroundLayer);
//         Debug.DrawRay(rb.transform.position, ( generalMonsterData.moveDirection) * generalMonsterData.obstacleRaycastDistance, Color.blue);
//         
//         if (hit.collider != null) return true;
//
//         return false;
//     }
//     
//     protected void OnCollisionEnter2D(Collision2D other)
//     {
//         if ( generalMonsterData.currentState == GeneralMonsterState.Death) return;
//         
//         if (other.gameObject.layer == 9)
//         {
//             TurnBack();
//         }
//
//         if (other.gameObject.layer == 6)
//         {
//             other.gameObject.GetComponent<PlayerController>().GetDamaged( generalMonsterData.attackDamage, this.gameObject,
//                 (((other.transform.position.x > transform.position.x) ? Vector2.right : Vector2.left) + 0.5f * Vector2.up).normalized *  generalMonsterData.knockBackPower);
//         }
//     }
//
//     protected virtual void Attack()
//     {
//         Debug.Log("Attack!GM");
//     }
//     
//     public void GetDamaged(float damage)
//     {
//         if(damage <= 0) return;
//         if( generalMonsterData.currentState == GeneralMonsterState.Death) return;
//
//         generalMonsterData.hp -= damage;
//         UIManager.instance.hitDamageInfo.PrintHitDamage(transform, damage);
//
//         if ( generalMonsterData.hp < 0)
//         {
//             Destroy(this.gameObject);
//         }
//     }
// }
