using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;
using UnityEngine.UI;
using Color = UnityEngine.Color;

public class GunSlot : MonoBehaviour
{
    public Image[] gunSlotBG;
    public Image[] gunSlot;
    public GameObject slotCenter;
    private PlayerController player;

    public static int selectGunNum = 0;
    public GameObject slot;
    public Sprite[] gunImages = new Sprite[8];

    Vector2 slotPos = new();
    int startAngle = 180;
    int endAngle;
    int check = 0;

    private void Update()
    {
        if (!PlayerController.IsControllable) return;
        OnButtonScreen();

        
    }
    private void GetFindPlayer()
    {
        if (player == null)
        {
            if (GameObject.Find("Player") == null)
            {
                return;
            }
            else
            {
                player = GameObject.Find("Player").GetComponent<PlayerController>();
            }
        }
        
    }
    void GunSlotCheck()
    {
        if (gunSlot == null) return;
        if (player.guns.Length <= selectGunNum) return;  
        for (int i = 1; i < gunSlot.Length; i++)
        {
            Color color = gunSlot[i].GetComponent<Image>().color;
            if (gunImages[i - 1] == null)
            {
                color.a = 0f;
            }
            else
            {
                color.a = 1f;
            }
            gunSlot[i].GetComponent<Image>().color = color;
            gunSlot[i].GetComponent<Image>().sprite = gunImages[i - 1];
        }
        Color zeroColor = gunSlot[0].GetComponent<Image>().color;
        zeroColor.a = 1f;
        gunSlot[0].GetComponent<Image>().color = zeroColor;
        gunSlot[0].GetComponent<Image>().sprite = gunImages[selectGunNum];
    }
    void OnButtonScreen()
    {
        
        Vector2 mousePos = Input.mousePosition;

        if (Input.GetKeyDown(KeyCode.F))
        {
            Pause.OnSlowMotion(true);
            GetFindPlayer();
            GetGunImage();
            GunSlotCheck();
            slotPos = mousePos;
            slotCenter.GetComponent<Image>().transform.position = slotPos;
            slot.SetActive(true);
        }
        if (Input.GetKey(KeyCode.F))
        {
            Vector2 direction = mousePos - slotPos;
            float angle = Vector2.Angle(Vector2.right, direction);
            if (mousePos.y < slotPos.y)
            {
                angle *= -1;
            }
            
            for (int i = 0; i < gunSlot.Length-1; i++)
            {
                endAngle = startAngle - 45;
                if (angle > endAngle && angle < startAngle)
                {
                    check = i;
                }
                startAngle = endAngle;
                if (i == check)
                {
                    gunSlotBG[i].transform.localScale = Vector3.one * 1.5f;
                }
                else
                {
                    gunSlotBG[i].transform.localScale = Vector3.one;
                }
            }
            startAngle = 180;
            
        }
        if (Input.GetKeyUp(KeyCode.F))
        {
            selectGunNum = check;
            Debug.Log(selectGunNum);
            player.SelectWeapon(selectGunNum);
            slot.SetActive(false);
            Pause.OnSlowMotion(false);
        }
    }
    public void GetGunImage()
    {
        for (int i = 0; i < player.guns.Length; i++)
        {
            gunImages[i] = player.guns[i].GetComponent<SpriteRenderer>().sprite;
        }
    }
    public static float GetAngle(Vector2 vStart, Vector2 vEnd)
    {
        Vector2 v = vEnd - vStart;

        return Mathf.Atan2(v.y, v.x) * Mathf.Rad2Deg;
    }
}
