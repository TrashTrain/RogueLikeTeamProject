using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedObstacle : Obstacle
{
    public float speed = 2f;
    public float originalSpeed = 5f;
    public float minusSpeedTime = 5f;
    
    public ItemGetText itemGetText;
    
    private static bool isActive = false;
    private static float remainingTime = 0f;

    public BuffItemController buffItemController;
    public Sprite icon;

    protected override void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == 6)
        {
            player = other.gameObject.GetComponent<PlayerController>();
            UIManager.instance.itemGetText.DisplayText("Slow Down!");
            
            if (isActive)
            {
                // 아이템 중복으로 획득하면 타이머만 갱신
                remainingTime = minusSpeedTime;
            }
            else
            {
                StartCoroutine(DecreaseSpeed(player));
            }
            
            GetComponent<SpriteRenderer>().enabled = false;
            GetComponent<Collider2D>().enabled = false;
        }
    }

    IEnumerator DecreaseSpeed(PlayerController player)
    {
        isActive = true;
        remainingTime = minusSpeedTime;

        // float currentSpeed = player.speed;
        player.speed -= speed;
        
        // 플레이어 프로필 스피드 업데이트
        UIManager.instance.playerInfo.UpdateProfileUI(player);
        
        if (player.speed < originalSpeed)
        {
            BGM.instance.OnSpeedObstacleCollected(minusSpeedTime);
        }
        else if (player.speed == originalSpeed)
        {
            BGM.instance.SetOriginalPitch();
        }
        
        while (remainingTime > 0)
        {
            yield return null;
            remainingTime -= Time.deltaTime;
        }
        
        player.speed = originalSpeed;
        isActive = false;
        
        // 플레이어 프로필 스피드 업데이트
        UIManager.instance.playerInfo.UpdateProfileUI(player);
        
        Destroy(gameObject);
    }
}
