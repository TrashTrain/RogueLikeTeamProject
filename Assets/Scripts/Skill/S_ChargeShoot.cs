using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class S_ChargeShoot : MonoBehaviour
{
    // 파티클 프리팹을 할당할 변수
    public GameObject chargeParticlePrefab;

    // 파티클 프리팹 인스턴스
    private GameObject chargeParticleInstance;
    
    public GameObject bullet;
    public Transform shootTrans;
    private float chargeTime = 2f;
    
    private float bulletDmg = 1f;
    private float bulletSpeed = 10f;
    private int maxBulletCount = 5;
    private float fireRate = 0.1f;

    private float startTime;
    private bool isCharged = false;

    #region ParticleTest

    private ParticleSystem ps;
    private ParticleSystem.MainModule mainModule;
    private ParticleSystem.ColorOverLifetimeModule colorOverLifetime;
    private ParticleSystem.SizeOverLifetimeModule sizeOverLifetime;
    private float elapsedTime = 0f;
    public float changeTime = 2f; // 2초 후에 변화

    private bool hasColorChanged = false;
    private bool hasSizeChanged = false;

    #endregion
    
    private void Update()
    {
        //차징 초기시간 초기화
        if (Input.GetKeyDown(KeyCode.E))
        {
            startTime = Time.time;
            isCharged = false;
            
            // 파티클 프리팹 인스턴스 생성
            chargeParticleInstance = Instantiate(chargeParticlePrefab, shootTrans.position, shootTrans.rotation);
            chargeParticleInstance.transform.localScale *= 0.5f;
            // 파티클 인스턴스를 캐릭터에 자식으로 설정
            chargeParticleInstance.transform.SetParent(shootTrans);
            
            InitPS(chargeParticleInstance);
            SoundManager.instance.PlaySound("Charge", transform);
        }

        if (Input.GetKey(KeyCode.E))
        {
            if (!isCharged && Time.time - startTime >= chargeTime)
            {
                isCharged = true;
                ChangeSize();
                ChangeColor();
            }
        }

        if (Input.GetKeyUp(KeyCode.E))
        {
            // 파티클 인스턴스 제거
            if (chargeParticleInstance != null)
            {
                Destroy(chargeParticleInstance);
            }
            
            //차징 시간보다 오랫동안 누르고 있을 경우
            if (isCharged)
            {
                //스킬 발사
                StartCoroutine(ChargeShoot());
            }
        }
    }
    
    
    
    // Bullet을 생성하는 코루틴
    IEnumerator ChargeShoot()
    {
        int bulletCount = 0;

        while (bulletCount < maxBulletCount)
        {
            var tempBullet = Instantiate(bullet, shootTrans.position, shootTrans.rotation);
            tempBullet.GetComponent<BulletController>().Init(bulletSpeed, bulletDmg);
            bulletCount++;

            // 0.1초 대기
            yield return new WaitForSeconds(fireRate);
        }
    }

    void InitPS(GameObject particleSystem)
    {
        ps = particleSystem.GetComponent<ParticleSystem>();
        mainModule = ps.main;
        colorOverLifetime = ps.colorOverLifetime;
        sizeOverLifetime = ps.sizeOverLifetime;

        // ColorOverLifetime 초기화
        colorOverLifetime.enabled = true;
        Gradient gradient = new Gradient();
        gradient.SetKeys(
            new GradientColorKey[] { new GradientColorKey(Color.white, 0f) },
            new GradientAlphaKey[] { new GradientAlphaKey(1f, 0f) }
        );
        colorOverLifetime.color = new ParticleSystem.MinMaxGradient(gradient);

        // SizeOverLifetime 초기화
        sizeOverLifetime.enabled = true;
        AnimationCurve curve = new AnimationCurve();
        curve.AddKey(0f, 1f);
        sizeOverLifetime.size = new ParticleSystem.MinMaxCurve(1f, curve);
    }
    
    void ChangeColor()
    {
        Gradient gradient = new Gradient();
        gradient.SetKeys(
            new GradientColorKey[] {
                new GradientColorKey(Color.red, 0f),
                new GradientColorKey(Color.red, 1f)
            },
            new GradientAlphaKey[] {
                new GradientAlphaKey(1f, 0f),
                new GradientAlphaKey(1f, 1f)
            }
        );
        colorOverLifetime.color = new ParticleSystem.MinMaxGradient(gradient);
    }

    void ChangeSize()
    {
        AnimationCurve curve = new AnimationCurve();
        curve.AddKey(0f, 3f); // 크기를 3배로 설정
        sizeOverLifetime.size = new ParticleSystem.MinMaxCurve(1f, curve);
    }
}
