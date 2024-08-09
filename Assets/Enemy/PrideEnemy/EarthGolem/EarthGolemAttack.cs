using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EarthGolemAttack : MonoBehaviour
{
    
    public GeneralMonsterTest earthGolem;

    private void Awake()
    {
        if (earthGolem == null)
        {
            transform.parent.gameObject.GetComponent<GeneralMonsterTest>();
        }
    }
    private void OnParticleTrigger()
    {
        Debug.Log("Earth");
        if (earthGolem != null)
        {
            float damage = earthGolem.GeneralMonsterData.attackDamage;
            float knockBackPower = earthGolem.GeneralMonsterData.knockBackPower;
            
            Transform player = earthGolem.GeneralMonsterData.targetTransform;
            
            player.gameObject.GetComponent<PlayerController>().GetDamaged(damage, this.gameObject, 
                (((player.position.x > this.transform.position.x) ? Vector2.right : Vector2.left) + 0.5f * Vector2.up).normalized *  knockBackPower);
        }
    }
}
