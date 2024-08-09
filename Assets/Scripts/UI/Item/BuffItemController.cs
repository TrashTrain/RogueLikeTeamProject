using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting;
using UnityEngine.EventSystems;

public class BuffItemController : MonoBehaviour
{
    public GameObject buffItemPrefab;
    public Transform buffPanel;
    public GameObject tooltipPanel;
    public TextMeshProUGUI tooltipText;
    public Image tooltipImg;
    
    public List<Buff> activeBuffs = new List<Buff>();
    private Buff currentBuff;

    void Update()
    {
        // 획득한 아이템 갱신 
        for (int i = activeBuffs.Count - 1; i >= 0; i--)
        {
            // 아이템 지속시간 체크
            activeBuffs[i].duration -= Time.deltaTime;
            if (activeBuffs[i].duration <= 0)
            {
                if (currentBuff == activeBuffs[i])
                {
                    tooltipPanel.SetActive(false);
                }
                Destroy(activeBuffs[i].buffObject);
                activeBuffs.RemoveAt(i);
            }
        }
        
        // 아이템 정보 갱신
        if (currentBuff != null)
        {
            tooltipImg.sprite = currentBuff.image;
            tooltipText.text = $"{currentBuff.name} \n current status/damage : {currentBuff.current} \n duration : {currentBuff.duration:F1}s";
        }
    }
    public void RemoveBuff()
    {
        
    }
    
    public void AddBuff(string buffName, float current, float duration, Sprite icon)
    {
        var duplicate = activeBuffs.Find(x => x.name.Equals(buffName));

        if (duplicate != null)
        {
            //duplicate.duration += duration;
            //duplicate.duration = 5f;
            duplicate.duration = duration;
            return;
        }
        
        GameObject newBuffItem = Instantiate(buffItemPrefab, buffPanel);
        newBuffItem.GetComponentInChildren<Image>().sprite = icon;
        Buff buff = new Buff { name = buffName, current = current, duration = duration, image = icon, buffObject = newBuffItem };

        EventTrigger trigger = newBuffItem.AddComponent<EventTrigger>();

        // 아이콘에 마우스 올리면 툴팁 보이게
        EventTrigger.Entry entryEnter = new EventTrigger.Entry();
        entryEnter.eventID = EventTriggerType.PointerEnter;
        entryEnter.callback.AddListener((data) => { OnMouseEnter(buff); });
        trigger.triggers.Add(entryEnter);
       
        // 아이콘에 마우스 내리면 툴팁 안 보이게
        EventTrigger.Entry entryExit = new EventTrigger.Entry();
        entryExit.eventID = EventTriggerType.PointerExit;
        entryExit.callback.AddListener((data) => { OnMouseExit(buff);});
        trigger.triggers.Add(entryExit);
        
        activeBuffs.Add(buff);
    }
    
    private void OnMouseEnter(Buff buff)
    {
        currentBuff = buff;
        tooltipPanel.SetActive(true);
    }

    private void OnMouseExit(Buff buff)
    {
        if (currentBuff == buff)
        {
            currentBuff = null;
            tooltipPanel.SetActive(false);
        }
    }

    public class Buff
    {
        public string name;
        public float current;
        public float duration;
        public Sprite image;
        public GameObject buffObject;
    }
}
