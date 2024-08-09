using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class GunController : MonoBehaviour
{
    #region GunData
    
    protected GunDataStruct gunData;
    public GunDataStruct GunData => gunData;
    [Header("Ref")] 
    [SerializeField] protected GunData refData;
    public GunData RefData => refData;

    #endregion

    protected PassiveSkillData passiveData;
    public Transform muzzle;
    protected float angle;

    protected Vector2 mousePos;
    
    protected Vector2 target;
    private SpriteRenderer sr;
    
    protected float shootingRate = 0f;

    //public float maxRate = 0.4f;
    //public float bulletSpeed = 0f;
    //public float bulletDmg = 2f;

    //public int automaticBulletCnt = 0;

    private void Awake()
    {
        if (refData == null)
        {
            Debug.LogError($"{this.gameObject} : refData is null");
        }
        else
        {
            refData.SyncData();
            gunData = refData.data;
        }
    }

    protected virtual void Start()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        if (!PlayerController.IsControllable) return;
        //if (!Pause.isPause) return;
        if(Time.timeScale == 0) return;

        //Debug.Log("test");
        mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        target = mousePos - (Vector2)transform.position;
        shootingRate += Time.deltaTime;

        transform.right = (Vector2)target.normalized;
        if (target.x < 0)
        {
            //if (sr.flipY != true) SetMuzzlePos();
            sr.flipY = true;
        }
        else
        {
            //if (sr.flipY != false) SetMuzzlePos();
            sr.flipY= false;
        }

        //���콺 �Է� �Ѿ� �߻�
        // if (Input.GetMouseButtonDown(0) && shootingRate > gunData.maxRate)
        // {
        //     Fire();
        //     shootingRate = 0f;
        // }
        Fire();

    }

    void SetMuzzlePos()
    {
        if (muzzle == null) return;
        muzzle.position = new Vector3(muzzle.position.x, muzzle.position.y, muzzle.position.z);
    }


    //�߻�ȭ �� �� �ڽ� ��ü���� ����� �����ض�.
    protected abstract void Fire();


    #region SetGunData

    //For Changing GunData Temporary 
    public void SetBulletDamageT(float damage)
    {
        gunData.bulletDamage = damage;
    }
    
    public void SetBulletSpeedT(float speed)
    {
        gunData.bulletSpeed = speed;
    }
    
    public void SetBulletMaxRateT(float maxRate)
    {
        gunData.maxRate = maxRate;
    }
    
    public void SetBulletReloadTimeT(float reloadTime)
    {
        gunData.reloadTime = reloadTime;
    }
    
    public void SetMaxAmmoT(int maxAmmo)
    {
        gunData.maxAmmo = maxAmmo;
    }

    #endregion

    #region SetRefData

    //For Changing refData Eternally
    public void SetBulletDamageE(float damage)
    {
        refData.bulletDamage = damage;
    }
    
    public void SetBulletSpeedE(float speed)
    {
        refData.bulletSpeed = speed;
    }
    
    public void SetBulletMaxRateE(float maxRate)
    {
        refData.maxRate = maxRate;
    }
    
    public void SetBulletReloadTimeE(float reloadTime)
    {
        refData.reloadTime = reloadTime;
    }
    
    public void SetMaxAmmo(int maxAmmo)
    {
        refData.maxAmmo = maxAmmo;
    }
    #endregion

}
