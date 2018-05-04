using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaintPickUp : MonoBehaviour {

    public Inventory _Inventory;
    public AudioClip pickupSound;
    private AudioSource pickupSource;
    
   void OnTriggerEnter2D(Collider2D collider)
    {

        if(collider.tag == "Player")
        {
            GameObject replica = this.gameObject;
            Sprite _paint = replica.GetComponent<SpriteRenderer>().sprite;
            _Inventory.AddItem(_paint);
            this.gameObject.SetActive(false);
        }
    }
}
