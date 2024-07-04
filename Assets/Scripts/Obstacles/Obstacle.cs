using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Obstacle : MonoBehaviour
{
    public PlayerController player;
    //protected abstract void faceObstacle();

    protected abstract void OnTriggerEnter2D(Collider2D other);
    // {
    //     if (other.gameObject.layer == 6)
    //     {
    //         faceObstacle();
    //     }
    // }
}