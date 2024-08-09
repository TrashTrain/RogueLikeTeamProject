using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cannon : GunController
{
    public GameObject bullet;
    
    
    protected override void Fire()
    {
        
        if (Input.GetMouseButtonDown(0) && shootingRate > gunData.maxRate)
        {
            var tempBullet = Instantiate(bullet, muzzle.transform.position + (Vector3.up * 0.2f), muzzle.transform.rotation);
            tempBullet.GetComponent<BulletController>().Init(gunData.bulletSpeed, gunData.bulletDamage);
            
            shootingRate = 0f;
            
            //for .NET Gabage Collector
            tempBullet = null;
        }
    }
}
