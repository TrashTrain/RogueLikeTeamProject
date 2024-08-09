using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class HPItem : Item
{
    public int hp = 2;

    protected override void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == 6)
        {
            PlayerController player = other.gameObject.GetComponent<PlayerController>();
            SFXManager.Instance.PlaySound(SFXManager.Instance.getItem);

            if (player.hp == 50f)
            {
                Destroy(gameObject);
                return;
            }
            else
            {
                float originalHp = player.hp;
                
                player.hp += hp;
                
                if (player.hp > 50)
                {
                    player.hp = 50;
                    float increase = player.hp - originalHp;
                    UIManager.instance.hpInfo.PrintHpUp(player.transform, increase); 
                }
                else
                {
                    UIManager.instance.hpInfo.PrintHpUp(player.transform, hp); 
                }
                
                UIManager.instance.playerInfo.SetHp(player.hp);
            }
            
            Destroy(gameObject);
        }
    }
}
