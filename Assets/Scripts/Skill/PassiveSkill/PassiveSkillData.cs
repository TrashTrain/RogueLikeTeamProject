using UnityEngine;

[System.Serializable]
public class PassiveSkillData
{
    public int automaticBulletCnt = 0;
    public float bulletSize = 1.0f;

    public PassiveSkillData(int automaticBulletCnt, float bulletSize)
    {
        Debug.Log("autoBullet : " + automaticBulletCnt + "bulletsize" + bulletSize);
        this.automaticBulletCnt = automaticBulletCnt;
        this.bulletSize = bulletSize;
    }
}
