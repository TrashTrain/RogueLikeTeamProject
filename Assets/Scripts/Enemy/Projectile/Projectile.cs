using System;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public SpriteRenderer sprite;
    public Rigidbody2D rb;
    public Animator animator;

    [SerializeField] private float knockBackPower = 50f;
    [SerializeField] private float attackDamage = 2f;
    public float AttackDamage => attackDamage;


    private void FixedUpdate()
    {
        transform.localScale += 0.5f * (Time.deltaTime) * transform.localScale;
    }

    private void OnEnable()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        sprite = GetComponent<SpriteRenderer>();
        
        Invoke("DestroyEvent", 3f);
    }

    protected virtual void OnTriggerEnter2D(Collider2D other)
    {
        
        if (other.gameObject.layer == 6)
        {
            
            other.gameObject.GetComponent<PlayerController>().GetDamaged(AttackDamage, this.gameObject,
                (((other.transform.position.x > transform.position.x) ? Vector2.right : Vector2.left) + 0.5f * Vector2.up).normalized * knockBackPower);
        }
        else
        {
            return;
        }
        
        DestroyEvent();
    }
    

    public void DestroyEvent()
    {
        Destroy(this.gameObject);
    }
}

