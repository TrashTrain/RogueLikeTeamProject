using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootingRateItem : Item
{
    public float shootingRate = 0.2f;
    public float plusShootingRateTime = 5f;
    
    public GunController gun;

    public BuffItemController buffItemController;
    public Sprite icon;
    
    protected override void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == 6)
        {
            SFXManager.Instance.PlaySound(SFXManager.Instance.getItem);
            //itemGetText.DisplayText("Shooting Rate Up!");
            UIManager.instance.itemGetText.DisplayText("Shooting Rate Up!");
            
            StartCoroutine(IncreaseShootingRate(gun));
            
            //buffItemController.AddBuff("Shooting Rate Up Item", gun.maxRate, plusShootingRateTime, icon);
            UIManager.instance.buffItemController.AddBuff("Shooting Rate Up Item", gun.GunData.maxRate, plusShootingRateTime, icon);

            GetComponent<SpriteRenderer>().enabled = false;
            GetComponent<Collider2D>().enabled = false;
        }
    }

    IEnumerator IncreaseShootingRate(GunController gun)
    {
        float playerShootingRate = gun.GunData.maxRate;
        gun.SetBulletMaxRateT(playerShootingRate + shootingRate);
        
        yield return new WaitForSeconds(plusShootingRateTime);

        gun.SetBulletMaxRateT(playerShootingRate);
        Destroy(gameObject);
    }
}
