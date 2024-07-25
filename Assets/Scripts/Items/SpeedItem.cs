using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;

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
            // BGM.instance.OnSpeedItemCollected(plusSpeedTime);   // 배경음악 속도 빠르게
            SFXManager.Instance.PlaySound(SFXManager.Instance.getItem);
            PlayerController player = other.gameObject.GetComponent<PlayerController>();
            //itemGetText.DisplayText("Speed UP!");
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
            //buffItemController.AddBuff("Speed Up Item", player.speed, plusSpeedTime, icon);
            //UIManager.instance.Canvas.B
            UIManager.instance.buffItemController.AddBuff("Speed Up Item", player.speed, plusSpeedTime, icon);
            
            GetComponent<SpriteRenderer>().enabled = false;
            GetComponent<Collider2D>().enabled = false;
        }
    }

    IEnumerator IncreaseSpeed(PlayerController player)
    {
        isActive = true;    // 아이템 중복 확인용
        remainingTime = plusSpeedTime;
        
        player.speed += speed;
        if (player.speed > 5)
        {
            BGM.instance.OnSpeedItemCollected(plusSpeedTime);
        }
        
        while (remainingTime > 0)
        {
            yield return null;
            remainingTime -= Time.deltaTime;
        }

        player.speed = originalSpeed;
        isActive = false;   // 아이템 효과 끝

        Destroy(gameObject);
    }
}