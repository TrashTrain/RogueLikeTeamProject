using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DEFObstacle : Obstacle
{
    public float DEF = 2f;
    public float originalDEF = 10f;
    public float minusDEFTime = 5f;

    private static bool isActive = false;
    private static float remainingTime = 0f;

    public BuffItemController BuffItemController;
    public Sprite icon;
    
    protected override void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == 6)
        {
            PlayerController player = other.gameObject.GetComponent<PlayerController>();

            if (isActive)
            {
                remainingTime = minusDEFTime;
            }
            else
            {
                StartCoroutine(DecreaseDEF(player));
            }
            
            GetComponent<SpriteRenderer>().enabled = false;
            GetComponent<Collider2D>().enabled = false;
        }
    }

    IEnumerator DecreaseDEF(PlayerController player)
    {
        isActive = true;
        remainingTime = minusDEFTime;

        player.def -= DEF;

        while (remainingTime > 0)
        {
            yield return null;
            remainingTime -= Time.deltaTime;
        }
        
        player.def = originalDEF;
        isActive = false;
        
        Destroy(gameObject);
    }
}
