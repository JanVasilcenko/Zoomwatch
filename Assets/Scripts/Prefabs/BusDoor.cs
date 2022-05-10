using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BusDoor : MonoBehaviour
{
    public PlayerInventory playerInventory;

    void Update()
    {
        if(playerInventory.getKey())
            gameObject.SetActive(false);
    }
}
