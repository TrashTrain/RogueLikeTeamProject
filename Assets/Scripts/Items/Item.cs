using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public abstract class Item : MonoBehaviour
{
    // protected PlayerController player;
    public ItemGetText itemGetText;
    
    protected abstract void OnTriggerEnter2D(Collider2D other);
    
}





    


