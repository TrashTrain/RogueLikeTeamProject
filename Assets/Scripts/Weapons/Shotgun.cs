using UnityEngine;

public class Shotgun : GunController
{
    public GameObject bullet;
    public PlayerController player;
    public int bulletNum = 5;
    public float reboundForce = 50f;
    
    protected override void Fire()
    {
        
        if (Input.GetMouseButtonDown(0) && shootingRate > gunData.maxRate)
        {
            for (int i = 0; i < bulletNum; i++)
            {
                var tempBullet = Instantiate(bullet, muzzle.transform.position + 0.8f * muzzle.transform.right + 0.2f * Vector3.up, muzzle.transform.rotation * Quaternion.Euler( 10 * (bulletNum/2 - i) * transform.forward));
                tempBullet.GetComponent<BulletController>().Init(gunData.bulletSpeed, gunData.bulletDamage);
                tempBullet = null;
            }
            
            player.SetForce( reboundForce *  -transform.right);
            shootingRate = 0f;
            //for .NET Gabage Collector
            
        }
    }
}