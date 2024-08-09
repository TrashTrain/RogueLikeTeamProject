using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;


public class RollingShoot : MonoBehaviour
{
    public GameObject[] bullet;
    public GameObject[] guns;
    public GameObject player;

    private int bulletNum = 10;
    private Transform trans;
    private float delay = 4f;
    private float delayCheck = 4f;
    void Start()
    {
        if (player == null) return;
         trans = player.transform;
    }

    private void Update()
    {
        delayCheck += Time.deltaTime;
        if (Input.GetKeyDown(KeyCode.R) && delayCheck >= delay)
        {
            StartCoroutine(S_RollingShot());
            delayCheck = 0f;
        }
    }

    private IEnumerator S_RollingShot()
    {
        for (int i = 0; i < bulletNum; i++)
        {
            var gunData = guns[GunSlot.selectGunNum].GetComponent<GunController>();
            var roateAngle = trans.rotation * Quaternion.Euler(36 * (bulletNum / 2 - i) * trans.forward);
            if(GunSlot.selectGunNum == 1)
            {
                StartCoroutine(gunData.GetComponent<RazorGun>().FireLaserContinuously());
                break;
                //yield return new WaitForSeconds(0.02f);
            }
            else
            {
                var tempBullet = Instantiate(bullet[GunSlot.selectGunNum], trans.position, roateAngle);
                tempBullet.GetComponent<BulletController>().Init(gunData.GunData.bulletSpeed, gunData.GunData.bulletDamage);
                yield return new WaitForSeconds(0.02f);
                tempBullet = null;
            }

            
            
        }
    }
}
