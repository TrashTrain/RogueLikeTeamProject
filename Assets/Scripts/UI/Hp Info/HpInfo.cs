using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.TextCore.Text;

public class HpInfo : MonoBehaviour
{
    public GameObject hpUpObject;
    public GameObject hpDownObject;
    
    [Header("Hp Up Font Set")]
    public TMP_FontAsset hpUpfont;
    public float hpUpFontSize;
    public Color hpUpFontColorTop;
    public Color hpUpFontColorBottom;

    
    [Header("Hp Down Font Set")]
    public TMP_FontAsset hpDownfont;
    public float hpDownFontSize;
    public Color hpDownFontColorTop;
    public Color hpDownFontColorBottom;
    
    private void Awake()
    {
        hpUpFontColorTop = Color.green;
        hpUpFontColorBottom = Color.gray;
        
        hpDownFontColorTop = Color.red;
        hpDownFontColorBottom = Color.black;
    }

    public void PrintHpUp(Transform charTrans, float plusHp)
    {
        var hpUp = Instantiate(hpUpObject, transform);
        
        hpUp.GetComponent<TextMeshProUGUI>().fontSize = hpUpFontSize;
        hpUp.GetComponent<TextMeshProUGUI>().colorGradient =
            new VertexGradient(hpUpFontColorTop, hpUpFontColorTop, hpUpFontColorBottom, hpUpFontColorBottom);
        hpUp.GetComponent<TextMeshProUGUI>().font = hpUpfont;
        
        // 월드 좌표를 화면 좌표로 변환
        Vector3 screenPosition = Camera.main.WorldToScreenPoint(charTrans.position);
        
        // hp 상승 텍스트 위치 설정
        hpUp.transform.position = screenPosition;
        hpUp.GetComponent<HpUpText>().SetHpUp(plusHp);
        hpUp.GetComponent<HpUpText>().SetCharTrans(charTrans);
    }

    public void PrintHpDown(Transform charTrans, float minusHp)
    {
        var hpDown = Instantiate(hpDownObject, this.transform);
        
        hpDown.GetComponent<TextMeshProUGUI>().fontSize = hpDownFontSize;
        hpDown.GetComponent<TextMeshProUGUI>().colorGradient =
            new VertexGradient(hpDownFontColorTop, hpDownFontColorTop, hpDownFontColorBottom, hpDownFontColorBottom);
        hpDown.GetComponent<TextMeshProUGUI>().font = hpDownfont;
        
        // 월드 좌표를 화면 좌표로 변환
        Vector3 screenPosition = Camera.main.WorldToScreenPoint(charTrans.position);
        
        // hp 상승 텍스트 위치 설정
        hpDown.transform.position = screenPosition;
        hpDown.GetComponent<HpDownText>().SetHpUp(minusHp);
        hpDown.GetComponent<HpDownText>().SetCharTrans(charTrans);
    }
}
