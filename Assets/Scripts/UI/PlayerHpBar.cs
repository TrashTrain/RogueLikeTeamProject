using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerHpBar : MonoBehaviour
{
    [SerializeField] private RectTransform playerHpSlide;
    [SerializeField] private TextMeshProUGUI playerHpText;

    private float playerMaxHp = 77;
    private float playerCurrentHp;
    
    //max width value of playerHpSlide RectTransform
    private const float max = 155f;

    public void Awake()
    {
        var player = FindObjectOfType<PlayerController>();
        if (player != null)
        {
            playerMaxHp = player.maxhp;
            playerCurrentHp = player.hp;
        }
    }

    public void InitPlayerHp(float MaxHp)
    {
        playerMaxHp = MaxHp;
        playerCurrentHp = playerMaxHp;
        SetHp(playerCurrentHp);
    }
    
    public void SetHp(float CurrentHp)
    {
        playerCurrentHp = CurrentHp;
        Debug.Log(playerCurrentHp);
        
        var percent = playerCurrentHp / playerMaxHp;
        
        playerHpSlide.sizeDelta = new Vector2(percent * max, playerHpSlide.sizeDelta.y);
        playerHpText.text = $"{playerCurrentHp} / {playerMaxHp}";
    }
}
