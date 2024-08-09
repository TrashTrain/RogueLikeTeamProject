using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ATKObstacle : Obstacle
{
    public float ATK = 2f;
    public float originalATK = 10f;
    public float minusATKTime = 5f;

    private static bool isActive = false;
    private static float remainingTime = 0f;

    public BuffItemController BuffItemController;
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

            if (isActive)
            {
                remainingTime = minusATKTime;
            }
            else
            {
                StartCoroutine(DecreaseATK());
            }
            
            GetComponent<SpriteRenderer>().enabled = false;
            GetComponent<Collider2D>().enabled = false;
        }
    }

    IEnumerator DecreaseATK()
    {
        isActive = true;
        remainingTime = minusATKTime;

        playerController.atk -= ATK;

        while (remainingTime > 0)
        {
            yield return null;
            remainingTime -= Time.deltaTime;
        }

        playerController.atk = originalATK;
        isActive = false;
        
        Destroy(gameObject);
    }
}
