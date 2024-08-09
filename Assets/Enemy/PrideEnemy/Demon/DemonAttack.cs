using System;
using System.Collections;
using System.Collections.Generic;
using Assets.PixelFantasy.PixelHeroes.Common.Scripts.CollectionScripts;
using Unity.VisualScripting;
using UnityEngine;

public class DemonAttack : MonoBehaviour
{
    public GeneralMonsterTest demon;
    public float attackDmg;
    
    private void Awake()
    {
        if (demon == null)
        {
            transform.parent.gameObject.GetComponent<GeneralMonsterTest>();
        }
    }
    
    private void OnParticleCollision(GameObject other)
    {
        Debug.Log("Particle Collision Detected");

        if (other.layer == 6)
        {
            if (demon != null)
            {
                // float damage = demon.GeneralMonsterData.attackDamage;
                float knockBackPower = demon.GeneralMonsterData.knockBackPower;
                Transform player = other.transform;
                
                // 데미지와 넉백을 처리
                player.gameObject.GetComponent<PlayerController>().GetDamaged(
                    attackDmg, 
                    this.transform.parent.gameObject,
                    (((this.transform.parent.position.x > transform.position.x) ? Vector2.left : Vector2.right) + 0.5f * Vector2.up).normalized * knockBackPower
                );
            }
        }
    }
    // private void OnParticleTrigger()
    // {
    //     Debug.Log("Explosion");
    //     if (demon != null)
    //     {
    //         float damage = demon.GeneralMonsterData.attackDamage;
    //         float knockBackPower = demon.GeneralMonsterData.knockBackPower;
    //         Transform player = demon.GeneralMonsterData.targetTransform;
    //         
    //         player.gameObject.GetComponent<PlayerController>().GetDamaged(damage, this.transform.parent.gameObject,
    //             (((this.transform.parent.position.x>transform.position.x)?Vector2.left : Vector2.right) + 0.5f * Vector2.up).normalized*knockBackPower);
    //     }
    // }
}
