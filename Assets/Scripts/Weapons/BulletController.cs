using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    private float bulletSpeed = 2f;
    private float bulletDamage;

    private bool isInit = false;

    private float timeCounter = 0f;

    private const float maxTime = 3f;
    
    public int penetrateCount = 0;
    public int maxPenetrateCount = 1;

    public void Init(float speed, float dmg)
    {
        bulletSpeed = speed;
        bulletDamage = dmg;
        maxPenetrateCount = 1;
    }

    public void Init(float speed, float dmg, float bulletSize)
    {
        bulletSpeed = speed;
        bulletDamage = dmg;
        gameObject.transform.localScale = Vector3.one * bulletSize;
        maxPenetrateCount = 1;
    }

    public void Init(float speed, float dmg, int maxPenetrateCount)
    {
        bulletSpeed = speed;
        bulletDamage = dmg;
        this.maxPenetrateCount = maxPenetrateCount;
    }
    
    private void Start()
    {        
        transform.Rotate(0, 0, -90);
        if (SoundManager.instance == null) return;
        SoundManager.instance.PlaySound("Shoot", transform.position);
    }
    
    void Update()
    {
        
        transform.Translate(0f, bulletSpeed * Time.deltaTime, 0f, Space.Self);
        timeCounter += Time.deltaTime;
        if(timeCounter > maxTime)
        {
            Destroy(gameObject);
        }
    }
    
    private void OnCollisionEnter2D(Collision2D collision)
    {
        penetrateCount = 0;
        
        if (collision.gameObject.layer == 9)
        {
            IDamageable enemy = collision.gameObject.GetComponent<IDamageable>();
            if (enemy != null)
            {
                enemy.GetDamaged(bulletDamage);
            }
            
            penetrateCount += 1;
            
            if (penetrateCount >= maxPenetrateCount)
            {
                Destroy(gameObject);
            }
        }

        if (collision.gameObject.layer == 7)
        {
            Destroy(gameObject);
        }
    }

    private IEnumerator EffectDelayedDestroy(ParticleSystem vfx)
    {
        yield return null;
        // �Ѿ� Ÿ�ݽ� ����Ʈ
    }
}

