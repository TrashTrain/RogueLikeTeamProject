using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HPObstacle : MonoBehaviour
{
    public int dmg;

    public float bounce = 20f;
    private Color originalColor;            //장애물 원래 색
    public Color hitColor = Color.red;    //캐릭터가 장애물에 부딪혔을 때 바뀌는 색
    
    private float changeTime = 0.5f;        //장애물 색 유지시간
    private static bool isActive;
    
    private SpriteRenderer spriteRenderer;
    
    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        originalColor = spriteRenderer.color;
    }
    
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.layer == 6)
        {
            if (!isActive)
            {
                // 효과음
                SFXManager.Instance.PlaySound(SFXManager.Instance.hpAtk);
                
                PlayerController player = other.gameObject.GetComponent<PlayerController>();

                Vector2 bounceForce = new Vector2(bounce, bounce);
                
                player.GetDamaged(dmg, gameObject, bounceForce);
                
                StartCoroutine(ChangeColor(this));
            }
        }
    }
    
    // 플레이어와 함정 접촉 시 함정 색 변경
    IEnumerator ChangeColor(HPObstacle hpObstacle)
    {
        isActive = true;
        spriteRenderer.color = hitColor;
        
        hpObstacle.GetComponent<PolygonCollider2D>().isTrigger = true;  // 한번 hp 손상되었으면 지나갈 수 있도록
        yield return new WaitForSeconds(changeTime);
        
        hpObstacle.GetComponent<PolygonCollider2D>().isTrigger = false;
        
        spriteRenderer.color = originalColor;
        isActive = false;
    }
}
