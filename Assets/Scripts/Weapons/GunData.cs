using UnityEngine;

[CreateAssetMenu(fileName = "GunData", menuName = "GunData")]
public class GunData : ScriptableObject
{
    public GunDataStruct data;
    
    [Header("Common Data")]
    public float bulletSpeed;
    public float bulletDamage;
    public float maxRate;
    public float reloadTime;
    public int maxAmmo;

    //Sync SO class variable to struct data (for deep copy)
    public void SyncData()
    {
        data.bulletSpeed = bulletSpeed;
        data.bulletDamage = bulletDamage;
        data.maxRate = maxRate;
        data.reloadTime = reloadTime;
        data.maxAmmo = maxAmmo;
    }
}
