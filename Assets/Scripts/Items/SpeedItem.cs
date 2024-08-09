using System.Collections;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class SpeedItem : Item
{
    public float speed = 2f;
    public float originalSpeed = 5f;
    public float plusSpeedTime = 5f;

    private static bool isActive = false;
    private static float remainingTime = 0f;
    
    public BuffItemController buffItemController;
    public Sprite icon;
    
    protected override void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == 6)
        {
            SFXManager.Instance.PlaySound(SFXManager.Instance.getItem);
            PlayerController player = other.gameObject.GetComponent<PlayerController>();

            UIManager.instance.itemGetText.DisplayText("Speed UP!");

            if (isActive)
            {
                // 아이템 중복으로 획득하면 타이머만 갱신
                remainingTime = plusSpeedTime;
            }
            else
            {
                StartCoroutine(IncreaseSpeed(player));
            }
            //아이템 버퍼창에 띄우기
            UIManager.instance.buffItemController.AddBuff("Speed Up Item", player.speed, plusSpeedTime, icon);
            
            // skill test
            // UIManager.instance.skillController.AddSkill(icon, "new skill");
            // test end;
            
            GetComponent<SpriteRenderer>().enabled = false;
            GetComponent<Collider2D>().enabled = false;
        }
    }

    IEnumerator IncreaseSpeed(PlayerController player)
    {
        isActive = true;    // 아이템 중복 확인용
        remainingTime = plusSpeedTime;

        // float currentSpeed = player.speed;
        player.speed += speed;
        
        // 플레이어 프로필 스피드 업데이트
        UIManager.instance.playerInfo.UpdateProfileUI(player);
        
        if (player.speed > originalSpeed)
        {
            BGM.instance.OnSpeedItemCollected(plusSpeedTime);
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
        isActive = false;   // 아이템 효과 끝
        
        // 플레이어 프로필 스피드 업데이트
        UIManager.instance.playerInfo.UpdateProfileUI(player);

        Destroy(gameObject);
    }
}