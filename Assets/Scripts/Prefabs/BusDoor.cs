using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BusDoor : MonoBehaviour
{
    public PlayerInventory playerInventory;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(playerInventory.getKey())
            gameObject.SetActive(false);
    }
}
