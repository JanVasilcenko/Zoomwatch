using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class KeyPickUp : MonoBehaviour
{
    public bool key, diamond;
    public PlayerInventory playerInventory;

    private void OnTriggerEnter(Collider other) {
        if(other.CompareTag(Tags.player)){
            if(diamond)
            {
                playerInventory.incrementDiamonds();
            }
            if(key){
                playerInventory.setKey();
            }
            //set visibility of object
            gameObject.SetActive(false);
        }
    }
}
