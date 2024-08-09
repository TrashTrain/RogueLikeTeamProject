using UnityEngine;

public class Sniper : GunController
{
    public GameObject bullet;
    public float mouseDownTime;
    public bool isMouseHeld;

    public LineRenderer gaugeLineRenderer; // Line Renderer 참조
    public float maxGaugeTime = 2f; // 게이지의 최대 시간
    public int segments = 60; // 원을 구성하는 점의 수
    
    private float criticalThresholdLow = 0.4f; // 크리티컬 하한
    private float criticalThresholdHigh = 0.6f; // 크리티컬 상한
    
    protected override void Fire()
    {
        if (shootingRate < gunData.maxRate) return;
        
        if (Input.GetMouseButtonDown(0))
        {
            mouseDownTime = Time.time;
            isMouseHeld = true;

            
            gaugeLineRenderer.enabled = true;
        }

        if (Input.GetMouseButton(0))
        {
            // 게이지 업데이트
            float fillAmount = Mathf.Clamp((Time.time - mouseDownTime) / maxGaugeTime, 0, 1);
            UpdateGauge(fillAmount);
        }
        
        if (Input.GetMouseButtonUp(0) && isMouseHeld)
        {
            float mouseHeldDuration = Time.time - mouseDownTime;
            isMouseHeld = false;

            float bulletSpeed = gunData.bulletSpeed;
            float bulletDamage = gunData.bulletDamage;

            // 마우스를 0.8초에서 1.2초 사이로 눌렀다면, 총알 속도를 5배로 설정
            if (mouseHeldDuration >= 0.8f && mouseHeldDuration <= 1.2f)
            {
                bulletSpeed *= 5f;
                bulletDamage *= 10f;
            }
            
            var tempBullet = Instantiate(bullet, muzzle.transform.position, muzzle.transform.rotation);
            tempBullet.GetComponent<BulletController>().Init(bulletSpeed, bulletDamage, 2);

            shootingRate = 0f;

            // .NET Garbage Collector를 위해 null로 설정
            tempBullet = null;

            // 게이지 초기화
            UpdateGauge(0);
            gaugeLineRenderer.enabled = false;
        }
    }

    void UpdateGauge(float fillAmount)
    {
        float angle = 180f * fillAmount;
        int pointCount = Mathf.CeilToInt(segments * fillAmount);

        gaugeLineRenderer.positionCount = pointCount + 1;
        gaugeLineRenderer.SetPosition(0, transform.position + 2 * Vector3.up + Vector3.zero);
        
        if (fillAmount >= criticalThresholdLow && fillAmount <= criticalThresholdHigh)
        {
            SetLineRendererColor(Color.red);
        }
        else
        {
            SetLineRendererColor(Color.yellow);
        }
        
        for (int i = 1; i <= pointCount; i++)
        {
            float rad = Mathf.Deg2Rad * (i * (angle / pointCount));
            float x = Mathf.Cos(rad);
            float y = Mathf.Sin(rad);
            
            gaugeLineRenderer.SetPosition(i,  transform.position + 2 * Vector3.up + new Vector3(x, y, 0));
        }
    }
    
    void SetLineRendererColor(Color color)
    {
        Color[] colors = new Color[2];
        colors[0] = color;
        colors[1] = color;
        Gradient gradient = new Gradient();
        gradient.SetKeys(
            new GradientColorKey[] { new GradientColorKey(colors[0], 0f), new GradientColorKey(colors[1], 1f) },
            new GradientAlphaKey[] { new GradientAlphaKey(1f, 0f), new GradientAlphaKey(1f, 1f) }
        );
        gaugeLineRenderer.colorGradient = gradient;
    }

}
