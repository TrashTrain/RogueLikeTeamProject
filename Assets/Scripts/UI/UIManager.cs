using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;
    public Canvas Canvas;

    [Header("PlayerHP")]
    public PlayerInfo playerInfo;
    public HitDamageInfo hitDamageInfo;
    public HpInfo hpInfo;   // hp 텍스트

    [Header("Item&Buff")]
    public GameObject buffPanel;
    public ItemGetText itemGetText;
    public BuffItemController buffItemController;

    [Header("SlotMachine&GunSlot")]
    public SlotController slotController;
    public GunSlot gunSlot;

    [Header("DialogSystem")]
    public DialogSystem dialogSystem;

    [Header("ShowProfileSkill")]
    // profile skill
    public SkillController skillController;
    
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
