using System;
using UnityEngine;

public class BlueSlimeBullet : MonoBehaviour
{
    public SpriteRenderer sprite;
    public Rigidbody2D rb;
    public Animator animator;
    
    private float attackDamage = 3f;
    public float AttackDamage => attackDamage;
    
    
    private SlimeState currentState = SlimeState.Attack;

    private void OnEnable()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        sprite = GetComponent<SpriteRenderer>();

        sprite.flipX = (rb.velocity.x > 0) ? false : true;
        
        Invoke("DestroyEvent", 4f);
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (currentState == SlimeState.Death) return;
        if (other.gameObject.layer == 9) return;
        
        if (other.gameObject.layer == 6)
        {
            other.gameObject.GetComponent<PlayerController>().GetDamaged(AttackDamage, this.gameObject, Vector2.zero);
        }
        
        Die();
    }
    
    public void Die()
    {
        currentState = SlimeState.Death;
        animator.SetTrigger("Death");
        SoundManager.instance.PlaySound("Slime_Damaged", transform.position);
    }

    public void DestroyEvent()
    {
        Destroy(this.gameObject);
    }
}
