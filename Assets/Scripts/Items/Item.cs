using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Item : MonoBehaviour
{
    protected abstract void takeItem();
    protected GameObject character;
    protected void OnCollisionEnter2D(Collision2D other){
        if (other.gameObject.layer == 6)
        {
            takeItem();
            Destroy(gameObject);
        }
    }
}





    


