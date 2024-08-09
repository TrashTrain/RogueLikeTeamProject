using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInfo : MonoBehaviour
{
    public PlayerHpBar playerHpBar;
    
    // player profile info
    public ProfileInfo profileInfo;


    public void SetHp(float currentHp)
    {
        playerHpBar.SetHp(currentHp);
    }

    public void InitPlayerUI(PlayerController playerController)
    {
        playerHpBar.InitPlayerHp(playerController.maxhp);
        playerHpBar.SetHp(playerController.hp);
    }

    // 플레이어 프로필 업데이트
    public void UpdateProfileUI(PlayerController playerController)
    {
        profileInfo.InitProfileInfo(playerController.atk, playerController.def, playerController.speed);
    }
}
