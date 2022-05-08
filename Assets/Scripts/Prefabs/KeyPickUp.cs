using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyPickUp : MonoBehaviour
{
    public bool key, diamond;

    public PlayerInventory playerInventory;
    
    void Start() {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other) {
        if(other.CompareTag("Player")){
            if(diamond){
                playerInventory.incrementDiamonds();
                Debug.Log(playerInventory.getDiamonds());
            }
            if(key){
                playerInventory.setKey();
                Debug.Log(playerInventory.getKey());
            }
            //set visibility of object
            gameObject.SetActive(false);

            
            
        }
    }
}
