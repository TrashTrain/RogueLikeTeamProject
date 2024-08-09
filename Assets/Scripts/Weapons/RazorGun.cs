using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RazorGun : GunController
{
    public float chargeTime = 3f;
    public float currentTime = 0f;
    public bool isCharge = false;

    public LineRenderer lineRenderer;
    public float laserDistance = 50f;
    public LayerMask hitLayers;
    
    public Color chargingColor = Color.yellow;
    public Color chargedColor = Color.red;
    public float chargingWidth = 0.05f;
    public float chargedWidth = 0.1f;
    public float firingWidth = 0.5f;
    public float minFiringWidth = 0.1f;
    public float widthDecreaseRate = 0.1f; // 레이저 두께 증가율
    
    public LineRenderer chargeGaze;
    public float chargeGazef;
    
    protected override void Start()
    {
        base.Start();
        // LineRenderer 초기화
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.positionCount = 2;
        lineRenderer.enabled = false;
        
        lineRenderer.startColor = chargingColor;
        lineRenderer.endColor = chargingColor;
        lineRenderer.startWidth = chargingWidth;
        lineRenderer.endWidth = chargingWidth;
        lineRenderer.enabled = false;

        chargeGaze.positionCount = 2;
        chargeGaze.enabled = true;
        chargeGaze.startWidth = 0.35f;
        chargeGaze.endWidth = 0.35f;
    }
    
    protected override void Fire()
    {
        
        chargeGaze.SetPosition(0, transform.position - transform.right * 0.4f);
        chargeGaze.SetPosition(1, transform.position - transform.right * 0.4f + 0.8f  * Mathf.Min(currentTime, chargeTime)/chargeTime * transform.right);
        
        if (Input.GetMouseButtonDown(0))
        {
            isCharge = false;
            currentTime = 0;
            SetRazor(chargingColor, chargingWidth);
            UpdateRazor();
            lineRenderer.enabled = true;
        }
        else if(Input.GetMouseButton(0))
        {
            UpdateRazor();
            if (!isCharge)
            {
                currentTime += Time.deltaTime;
                if (currentTime >= chargeTime)
                {
                    isCharge = true;
                    SetRazor(chargedColor, chargedWidth);
                }
            }
        }
        else if(Input.GetMouseButtonUp(0))
        {
            if (isCharge)
            {
                //shoot
                StartCoroutine(FireLaserContinuously());
            }

            currentTime = 0;
            isCharge = false;
            lineRenderer.enabled = false;
        }
    }
    
    private void ShootLaser()
    {
        // 레이저 시작 위치 및 방향 설정
        Vector3 startPos = transform.position + transform.right * 0.5f;
        Vector3 direction = transform.right; 

        UpdateRazor();
        SetRazor(chargedColor, firingWidth);

        // 레이저의 끝 위치 계산 및 관통 레이저 구현
        RaycastHit2D[] hits = Physics2D.RaycastAll(startPos, direction, laserDistance, hitLayers);

        foreach (var hit in hits)
        {
        
            // 충돌한 객체에 피해를 주는 로직
            var enemy = hit.collider.GetComponent<IDamageable>();
            if (enemy != null)
            {
                enemy.GetDamaged(gunData.bulletDamage);
            }
        }
        lineRenderer.enabled = true;
    }
    
    public IEnumerator FireLaserContinuously()
    {
        firingWidth = 0.6f;

        for(int i =0 ; i<5 ; i++)
        {
            ShootLaser();

            // 레이저 두께 증가
            firingWidth = Mathf.Max(firingWidth - widthDecreaseRate, minFiringWidth);

            yield return new WaitForSeconds(0.1f);
        }
        
        lineRenderer.enabled = false;
    }

    private void SetRazor(Color color, float width)
    {
        lineRenderer.startColor = color;
        lineRenderer.endColor = color;
        lineRenderer.startWidth = width;
        lineRenderer.endWidth = width;
    }

    private void UpdateRazor()
    {
        // 레이저 시작 위치 및 방향 설정
        Vector3 startPos = transform.position + transform.right * 0.5f;
        Vector3 direction = transform.right;
        // 레이저의 끝 위치 계산
        Vector3 endPos = startPos + direction * laserDistance;
        
        lineRenderer.SetPosition(0, startPos);
        lineRenderer.SetPosition(1, endPos);
    }
}
