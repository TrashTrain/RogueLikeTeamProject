using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Poison : MonoBehaviour
{
    public float dmg = 2f;
    public float dmgTime = 3f;

    private SpriteRenderer spriteRenderer;
    private Color originalColor;
    
    private static bool isActive = false;
    private static float duration = 0f;
    
    public BuffItemController buffItemController;
    public Sprite icon;
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == 6)
        {
            PlayerController player = other.gameObject.GetComponent<PlayerController>();
            spriteRenderer = player.GetComponent<SpriteRenderer>();
            originalColor = spriteRenderer.color;
            
            // sound
            SFXManager.Instance.PlaySound(SFXManager.Instance.getItem);
            
            if (isActive)
            {
                // 아이템 중복으로 획득하면 타이머만 갱신
                duration = 0f;
            }
            else
            {
                StartCoroutine(DecreaseHp(player));
            }
            
            UIManager.instance.buffItemController.AddBuff("Poisonous Obstacle", -1*dmg*dmgTime, dmgTime, icon);
            
            
            GetComponent<SpriteRenderer>().enabled = false;
            GetComponent<Collider2D>().enabled = false;

            if (player.hp <= 0)
            {
                Debug.Log("GameOver");
                Destroy(player);
            }
        }
    }

    IEnumerator DecreaseHp(PlayerController player)
    {
        isActive = true;

        while (duration < dmgTime)
        {
            yield return new WaitForSeconds(1f);
            if (duration % 2 == 0)
            {
                player.GetComponent<SpriteRenderer>().color = new Color32(255, 255, 255, 90);
            }
            else
            {
                player.GetComponent<SpriteRenderer>().color = new Color32(255, 255, 255, 180);
            }
            
            // 효과음
            SFXManager.Instance.PlaySound(SFXManager.Instance.hpAtk);
            
            //hp minus text
            UIManager.instance.hpInfo.PrintHpDown(player.transform, dmg);

            player.hp -= dmg;
            
            UIManager.instance.playerInfo.SetHp(player.hp);
            duration += 1f;
        }
        
        spriteRenderer.color = originalColor;
        isActive = false;
        Destroy(gameObject);
    }
}
