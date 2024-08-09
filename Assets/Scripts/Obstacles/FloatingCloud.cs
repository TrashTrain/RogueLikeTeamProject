using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatingCloud : MonoBehaviour
{
    public float amplitude = 0.2f;
    public float frequency = 1f;
    
    Vector2 startPos;

    void Start()
    {
        startPos = transform.position;
    }

    void Update()
    {
        // 위 아래로 진동
        float newX = startPos.x + Mathf.Sin(Time.time * frequency) * amplitude;
        transform.position = new Vector2(newX, transform.position.y);
    }
}
