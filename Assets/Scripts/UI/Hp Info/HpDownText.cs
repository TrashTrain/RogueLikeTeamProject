using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HpDownText : MonoBehaviour
{
    public float moveSpeed = 2f;
    private TextMeshProUGUI hpDownText;

    private RectTransform rectTransform;
    private Vector3 upTransform = Vector3.zero;
    private Transform charTrans;
    private Vector3 screenPosition;
    
    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        hpDownText = GetComponent<TextMeshProUGUI>();
    }
    
    private void Start()
    {
        Invoke("Destroy", 0.5f);
    }

    private void FixedUpdate()
    {
        upTransform += moveSpeed * Vector3.up;
        // 월드 좌표를 화면 좌표로 변환
        if (charTrans != null)
        {
            screenPosition = Camera.main.WorldToScreenPoint(charTrans.position + Vector3.up);
        }

        transform.position = screenPosition + upTransform;
    }

    private void Destroy()
    {
        Destroy(gameObject);
    }

    public void SetHpUp(float minusHp)
    {
        hpDownText.text = $" HP  - {minusHp}";
    }

    public void SetCharTrans(Transform transform)
    {
        charTrans = transform;
    }
}
