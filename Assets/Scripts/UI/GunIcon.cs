using Cinemachine.Utility;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GunIcon : MonoBehaviour
{
    // 추후에 스킬 받아와서 교체 및 처리 할 수 있게.
    public RollingShoot testSkill;
    private void Update()
    {
        if (UIManager.instance.gunSlot.gunImages[GunSlot.selectGunNum] == null) return;
        if (GunSlot.selectGunNum == 0)
            gameObject.transform.localPosition = new Vector3(38f, 60f);
        else
            gameObject.transform.localPosition = new Vector3(50f, 50f);
        gameObject.GetComponent<Image>().sprite = UIManager.instance.gunSlot.gunImages[GunSlot.selectGunNum];
    }

    private void FillSkillGauge()
    {

    }


}
