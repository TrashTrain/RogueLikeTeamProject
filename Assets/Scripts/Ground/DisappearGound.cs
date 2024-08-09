using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisappearGound : MonoBehaviour
{
    private SpriteRenderer sprite;

    private void Awake()
    {
        sprite = GetComponent<SpriteRenderer>();
    }

    public void EnableEvent()
    {
        gameObject.SetActive(true);
    }
    
    public void DisableEvent()
    {
        gameObject.SetActive(false);
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.layer == 6)
        {
            Invoke("DisableEvent", 2f);
        }

        if (other.gameObject.layer == 9)
        {
            gameObject.SetActive(false);
        }
    }

    private void OnDisable()
    {
        Invoke("EnableEvent", 6f);
    }
}
