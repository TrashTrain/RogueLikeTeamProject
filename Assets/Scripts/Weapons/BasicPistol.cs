using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicPistol : GunController
{
    public GameObject bullet;

    public int automaticBulletCnt = 0;
    public float bulletSize = 0.1f;

    protected override void Fire()
    {
        //muzzleFlash.Play();
        //bullet ����
        
        if (Input.GetMouseButtonDown(0) && shootingRate > gunData.maxRate)
        {
            Debug.Log(automaticBulletCnt);
            StartCoroutine(ShowCreateBullet());

        }
    }

    private IEnumerator ShowCreateBullet()
    {
        CreateBullet();
        for (int i = 0; i < automaticBulletCnt; i++)
        {
            yield return new WaitForSeconds(0.05f);
            CreateBullet();
        }

    }

    private void CreateBullet()
    {
        var tempBullet = Instantiate(bullet, muzzle.transform.position, muzzle.transform.rotation);
        tempBullet.GetComponent<BulletController>().Init(gunData.bulletSpeed, gunData.bulletDamage, bulletSize);

        shootingRate = 0f;

        //for .NET Gabage Collector
        tempBullet = null;
    }
}
