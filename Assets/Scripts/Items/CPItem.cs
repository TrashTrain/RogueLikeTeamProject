using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CPItem : Item
{
    public int CP = 2;
    public float originalCP = 5f;
    public float plusCPTime = 5f;

    private static bool isActive = false;
    private static float remainingTime = 0f;
    
    public BuffItemController buffItemController;
    public Sprite icon;

    private PlayerController playerController;
    
    protected override void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == 6)
        {
            playerController = other.gameObject.GetComponent<PlayerController>();
            if (playerController == null)
            {
                Debug.LogError($"{this.gameObject.name} : playerController is null");
                return;
            }
            
            SFXManager.Instance.PlaySound(SFXManager.Instance.getItem);

            //itemGetText.DisplayText("Attack Power Up!");
            UIManager.instance.itemGetText.DisplayText("Attack Power Up!");
            
            if (isActive)
            {
                remainingTime = plusCPTime;
            }
            else
            {
                StartCoroutine(IncreaseCP(playerController));
            }

            UIManager.instance.buffItemController.AddBuff("ATK Up Item", playerController.atk, plusCPTime, icon);
            //buffItemController.AddBuff("ATK Up Item", player.atk, plusCPTime, icon);
            
            GetComponent<SpriteRenderer>().enabled = false;
            GetComponent<Collider2D>().enabled = false;
        }
    }

    IEnumerator IncreaseCP(PlayerController player)
    {
        isActive = true;
        remainingTime = plusCPTime;
        
        playerController.atk += CP;

        // 플레이어 프로필 공격력 업데이트
        UIManager.instance.playerInfo.UpdateProfileUI(player);
        
        while (remainingTime > 0)
        {
            yield return null;
            remainingTime -= Time.deltaTime;
        }
        
        playerController.atk = originalCP;
        isActive = false;
        
        // 플레이어 프로필 공격력 업데이트
        UIManager.instance.playerInfo.UpdateProfileUI(player);
        
        Destroy(gameObject);
    }
}
