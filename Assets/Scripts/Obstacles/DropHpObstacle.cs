using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropHpObstacle : MonoBehaviour
{
    public PlayerController player;

    public float dropDistance = 2f;

    private Rigidbody2D rb;

    public float gravityScale;
    public float dmg;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    
    void Update()
    {
        if (player != null)
        {
            float distance = Vector2.Distance(transform.position, player.transform.position);

            if (distance <= dropDistance)
            {
                rb.gravityScale = gravityScale;
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.layer == 7)
        {
            Destroy(gameObject);
        }
        else if (other.gameObject.layer == 6)
        {
            other.gameObject.GetComponent<PlayerController>().GetDamaged(dmg, gameObject, new Vector2(0f,0f));
            Destroy(gameObject);
        }
    }
}
