using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    public float maxhp = 50;
    //player Status
    [Header("Player Status")]
    public float hp = 50;
    public float atk = 0;
    public float def = 10;
    public float speed = 5f;

    bool _isTurn = true;

    private SpriteRenderer spriteRenderer;

    private Transform tf;
    private Collider2D col;
    
    [Header("Animation")]
    public Animator ani;

    private Rigidbody2D rigid;

    Vector3 movement;
    bool isJumping = false;
    bool isUnBeatTime = false;

    public GameObject[] guns;
    private GameObject curGun;
    public static bool IsControllable = true;
    
    [Header("Jump & Climb")]
    public float jumpPower = 1f;
    public int maxJump = 1;
    public int jumpCount = 1;
    public float groundCheckDistance = 0.1f; // 박스캐스트 길이
    public LayerMask groundLayer; // 땅으로 인식할 레이어
    public float wallCheckDistance = 0.5f; // 레이캐스트 길이
    public LayerMask wallLayer; // 벽으로 인식할 레이어
    public float wallSlideSpeed = 2f; // 벽에 붙었을 때 낙하 속도
    public bool isWallSliding = false;
    public bool isWallJump = false;
    public Vector2 groundCheckSize = new Vector2(0.4f, 0.1f); // 박스캐스트 크기

    
    //Hit Effect
    public CinemachineImpulseSource impurse;
    public Animator hitAnimator;
    
    //---------------------------------------------------[Override Function]
    //Initialization
    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        rigid = gameObject.GetComponent<Rigidbody2D>();
        col = GetComponent<Collider2D>();
        tf = transform;
        curGun = guns[0];
        curGun.SetActive(true);
        

        jumpCount = maxJump;
        //Cinemachine Impurse
        impurse = transform.GetComponent<CinemachineImpulseSource>();
    }

    private void Start()
    {
        UIManager.instance.playerInfo.InitPlayerUI(this);
        // 플레이어 프로필 업데이트
        UIManager.instance.playerInfo.UpdateProfileUI(this);
    }

    //Graphic & Input Updates	
    void Update()
    {
        if (!IsControllable) return;
        UIManager.instance.playerInfo.UpdateProfileUI(this);
        //Debug.Log("playertest");
        WallCheck();
        Jump();
        

        // // fall Charactor
        // if(transform.position.y <= -7)
        // {
        //     Destroy(gameObject);
        //     Debug.Log("GameOver");
        // }
    }
    private void FixedUpdate()
    {
        Move();
    }

    private void LateUpdate()
    {
        if (isWallSliding)
        {
            WallSlide();
        }
    }


    void Move()
    {
        if (!IsControllable) return;
        if (isWallSliding || isWallJump) return;
        
        Vector2 moveVelocity = Vector2.zero;
        Vector2 mousePos = Input.mousePosition;

        Vector2 target = Camera.main.ScreenToWorldPoint(mousePos);
        if (Input.GetAxisRaw("Horizontal") < 0)
        {
            ani.SetBool("IsRunning", true);
            moveVelocity = Vector3.left;

            if (rigid.velocity.x > 0 && !isWallSliding)
            {
                rigid.velocity = new Vector2(0, rigid.velocity.y);
            }

        }

        else if (Input.GetAxisRaw("Horizontal") > 0)
        {
            ani.SetBool("IsRunning", true);
            moveVelocity = Vector3.right;
            
            if (rigid.velocity.x < 0 && !isWallSliding)
            {
                rigid.velocity = new Vector2(0, rigid.velocity.y);
            }
        }
        else
        {
            ani.SetBool("IsRunning", false);
        }

        if (target.x < tf.position.x && _isTurn)
        {
            _isTurn = false;
            tf.Rotate(0f, 180f, 0f);
            
        }
        if (target.x > tf.position.x && !_isTurn)
        {
            _isTurn = true;
            tf.Rotate(0f, 180f, 0f);
            
        }
        
        rigid.position += moveVelocity * speed * Time.deltaTime;
    }

    void Jump()
    {
        if (!IsControllable) return;
        if (Input.GetButtonDown("Jump"))
        {
            GroundCheck();
            if(jumpCount <= 0) return;
            if(isWallSliding) return;
            
            //Prevent Velocity amplification.
            rigid.velocity = Vector2.zero;
            
            Vector2 jumpVelocity = new Vector2(0, jumpPower);

            rigid.AddForce(jumpVelocity, ForceMode2D.Impulse);
            jumpCount--;
        }
        
        //isWallSliding = false;
    }

    public void SetForce(Vector2 force)
    {
        rigid.velocity = Vector2.zero;
        rigid.AddForce(force, ForceMode2D.Impulse);
    }
    
    public void AddForce(Vector2 force)
    {
        rigid.AddForce(force, ForceMode2D.Force);
    }
    
    public void GetDamaged(float dmg, GameObject enemy, Vector2 attackPower)
    {
        if (!isUnBeatTime && attackPower != null)
        {
            rigid.AddForce(attackPower, ForceMode2D.Impulse);
        }
        if (!isUnBeatTime)
        {
            hp -= dmg;
            UIManager.instance.playerInfo.SetHp(hp);
            
            //hp minus text
            UIManager.instance.hpInfo.PrintHpDown(transform, dmg);
            
            isUnBeatTime = true;
            StartCoroutine("UnBeatTime");

            ShakeCamera();
            if (hitAnimator != null)
            {
                hitAnimator.SetTrigger("Hit");
            }
            
            if (hp <= 0)
            {
                Debug.Log("GameOver");
                Destroy(gameObject);
                GameOver();
            }
        }
    }
    
    //private void OnCollisionEnter2D(Collision2D collision)
    //{
    //    if(collision.gameObject.layer == 7)
    //    {
    //        jumpCount = 0;
    //        isJumping = true;
    //    }
        
    //}
    // private void OnTriggerEnter2D(Collider2D collision)
    // {
    //     if (collision.gameObject.layer == 7)
    //     {
    //         jumpCount = 0;
    //         isJumping = true;
    //     }
    // }
    
    private void GroundCheck()
    {
        // 박스캐스트로 땅에 닿았는지 확인
        //RaycastHit2D hit = Physics2D.BoxCast(col.bounds.center, col.bounds.size, 0f, Vector2.down, groundCheckDistance, groundLayer);
        Vector2 boxCenter = new Vector2(col.bounds.center.x, col.bounds.min.y);
        RaycastHit2D hit = Physics2D.BoxCast(boxCenter, groundCheckSize, 0f, Vector2.down, groundCheckDistance, groundLayer);

        
        if (hit.collider != null)
        {
            jumpCount = maxJump; // 땅에 닿으면 점프 가능 횟수 초기화
            //isWallSliding = false; // 땅에 닿았을 때 벽 슬라이딩 상태 초기화
        }
    }

    private void WallCheck()
    {
        if (isUnBeatTime)
        {
            isWallSliding = false;
            isWallJump = false;
            return;
        }
        
        // 좌우로 레이캐스트를 쏘아서 벽에 닿았는지 확인
        RaycastHit2D hitLeft = Physics2D.Raycast(transform.position + 0.3f * Vector3.up, Vector2.left, wallCheckDistance, wallLayer);
        RaycastHit2D hitRight = Physics2D.Raycast(transform.position + 0.3f * Vector3.up, Vector2.right, wallCheckDistance, wallLayer);

        if (hitLeft.collider != null && Input.GetAxisRaw("Horizontal") <= 0 || hitRight.collider != null && Input.GetAxisRaw("Horizontal") >= 0)
        {
            isWallSliding = true; // 벽에 닿았으면 벽 슬라이딩 상태로 설정
        }
        else
        {
            isWallSliding = false; // 벽에 닿지 않았으면 벽 슬라이딩 상태 해제
        }
    }
    
    private void WallSlide()
    {
        //isWallJump = false;
        
        if (rigid.velocity.y < -wallSlideSpeed)
        {
            rigid.velocity = new Vector2(rigid.velocity.x, -wallSlideSpeed);
        }

        // if (Input.GetAxisRaw("Horizontal") > 0 && Input.GetButtonDown("Horizontal"))
        // {
        //     RaycastHit2D hitLeft = Physics2D.Raycast(transform.position, Vector2.left, wallCheckDistance, wallLayer);
        //     if (hitLeft.collider != null)
        //     {
        //         rigid.AddForce(new Vector2(jumpPower / 2, 0), ForceMode2D.Impulse);
        //         isWallSliding = false;
        //         isWallJump = false;
        //         return;
        //     }
        // }
        //
        // if (Input.GetAxisRaw("Horizontal") < 0 && Input.GetButtonDown("Horizontal"))
        // {
        //     RaycastHit2D hitRight = Physics2D.Raycast(transform.position, Vector2.right, wallCheckDistance, wallLayer);
        //     if (hitRight.collider != null)
        //     {
        //         rigid.AddForce(new Vector2(-jumpPower / 2, 0), ForceMode2D.Impulse);
        //         isWallSliding = false;
        //         isWallJump = false;
        //         return;
        //     }
        // }
        
        if (Input.GetButtonDown("Jump"))
        {
            RaycastHit2D hitLeft = Physics2D.Raycast(transform.position, Vector2.left, wallCheckDistance, wallLayer);
            if (hitLeft.collider != null)
            {
                rigid.AddForce(new Vector2(jumpPower/2, 0), ForceMode2D.Impulse);
            }
            else if (hitLeft.collider == null)
            {
                rigid.AddForce(new Vector2(-jumpPower/2, 0), ForceMode2D.Impulse);
            }
            
            rigid.AddForce(new Vector2(0, jumpPower), ForceMode2D.Impulse);
            isWallJump = true;
            
            Invoke("SetWallJumpFalse", 0.3f);
        }
    }

    private void SetWallJumpFalse()
    {
        isWallJump = false;
    }
    private void OnDrawGizmosSelected()
    {
        if (col == null) return;

        Gizmos.color = Color.red;
        //Gizmos.DrawWireCube(col.bounds.center - new Vector3(0, groundCheckDistance, 0), col.bounds.size);
        //Gizmos.DrawWireCube(col.bounds.center - new Vector3(0, groundCheckDistance, 0), new Vector3(groundCheckSize.x, groundCheckSize.y, 1));
        Vector3 boxCenter = new Vector3(col.bounds.center.x, col.bounds.min.y, 0);
        Gizmos.DrawWireCube(boxCenter - new Vector3(0, groundCheckDistance, 0), new Vector3(groundCheckSize.x, groundCheckSize.y, 1));

        
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(transform.position + 0.3f * Vector3.up, transform.position + 0.3f * Vector3.up + Vector3.left * wallCheckDistance);
        Gizmos.DrawLine(transform.position + 0.3f * Vector3.up, transform.position + 0.3f * Vector3.up + Vector3.right * wallCheckDistance);
    }

    IEnumerator UnBeatTime()
    {
        int countTime = 0;
        while (countTime < 10)
        {
            if (countTime % 2 == 0)
            {
                spriteRenderer.color = new Color32(255, 255, 255, 90);
            }
            else
            {
                spriteRenderer.color = new Color32(255, 255, 255, 180);
            }
            yield return new WaitForSeconds(0.2f);

            countTime++;
        }

        spriteRenderer.color = new Color32(255, 255, 255, 255);

        isUnBeatTime = false;

        yield return null;
    }

    private void GameOver()
    {
        BGM.instance.PlayBGM("GameOver");
        SceneManager.LoadScene("GameOver");
    }
    public void SelectWeapon(int idx)
    {
        if (guns.Length <= GunSlot.selectGunNum) return;
        curGun.SetActive(false);


        guns[idx].gameObject.SetActive(true);

        curGun = guns[idx];
    }

    private void ShakeCamera()
    {
        impurse.GenerateImpulse(0.25f);
    }


}
