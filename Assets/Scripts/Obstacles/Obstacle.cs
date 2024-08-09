using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Obstacle : MonoBehaviour
{
    protected PlayerController player;

    protected abstract void OnTriggerEnter2D(Collider2D other);
}
