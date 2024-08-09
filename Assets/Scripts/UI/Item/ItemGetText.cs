using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.TextCore.Text;
using UnityEngine.UI;

public class ItemGetText : MonoBehaviour
{
    public TextMeshProUGUI itemText;
    public float disPlayTime = 1f;
    private float timer = 0f;
    private bool isDisplaying = false;
    
    void Start()
    {
        itemText.text = "";
    }
    
    void Update()
    {
        if (isDisplaying)
        {
            timer += Time.deltaTime;
            if (timer > disPlayTime)
            {
                itemText.text = "";
                isDisplaying = false;
            }
        }
    }
    
    public void DisplayText(string text)
    {
        itemText.text = text;
        timer = 0f;
        isDisplaying = true;
    }
}
