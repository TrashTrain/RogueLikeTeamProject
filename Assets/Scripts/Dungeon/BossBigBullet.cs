using System;
using UnityEngine;

public class BossBigBullet : MonoBehaviour
{
    public Rigidbody2D rb;
    public float disableTime = 20f;

    [SerializeField] private float knockBackPower = 80f;
    [SerializeField] private float attackDamage = 2f;
    public float AttackDamage => attackDamage;

    public void SetDisableTime(float time)
    {
        disableTime = time;
    }
    
    private void OnEnable()
    {
        rb = GetComponent<Rigidbody2D>();
        
        Invoke("DestroyEvent", disableTime);
    }

    protected virtual void OnTriggerEnter2D(Collider2D other)
    {
        
        if (other.gameObject.layer == 6)
        {
            
            other.gameObject.GetComponent<PlayerController>().GetDamaged(AttackDamage, this.gameObject,
                (((other.transform.position.x > transform.position.x) ? Vector2.right : Vector2.left) + 0.5f * Vector2.up).normalized * knockBackPower);
        }
        
        DestroyEvent();
    }
    

    public void DestroyEvent()
    {
        Destroy(this.gameObject);
    }
}