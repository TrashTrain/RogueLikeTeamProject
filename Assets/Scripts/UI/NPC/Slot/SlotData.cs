using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public enum ItemType
{
    Status, PassiveSkill, ActiveSkill
}
public enum StatusType
{
    NULL, HP, ATK, DEF, SPD
}
public enum PassiveType
{
    NULL, BulletCnt, RopeAtk, PenetrateCnt, BulletSize
}

[CreateAssetMenu(fileName = " New SlotData", menuName ="CustomData/Create SlotData")]
public class SlotData : ScriptableObject
{
    public Sprite itemImage;
    public string itemName;
    public int value;
    public TMP_FontAsset font;
    
    [TextArea]
    public string itemDescription;
    public ItemType type;
    public StatusType status;
    public PassiveType passive;
}
