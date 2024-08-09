using System;
using UnityEngine;

public class FlameGolemAttack : MonoBehaviour
{
    public GeneralMonsterTest flameGolem;

    private void Awake()
    {
        if (flameGolem == null)
        {
            transform.parent.gameObject.GetComponent<GeneralMonsterTest>();
        }
    }

    private void OnParticleTrigger()
    {
        Debug.Log("Flame");
        if (flameGolem != null)
        {
            float damage = flameGolem.GeneralMonsterData.attackDamage;
            float knockBackPower = flameGolem.GeneralMonsterData.knockBackPower;
            Transform player = flameGolem.GeneralMonsterData.targetTransform;
            
            player.gameObject.GetComponent<PlayerController>().GetDamaged(damage, this.transform.parent.gameObject, 
                (((this.transform.parent.position.x > transform.position.x) ? Vector2.left : Vector2.right) + 0.5f * Vector2.up).normalized *  knockBackPower);
        }
    }
}