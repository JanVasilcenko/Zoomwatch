using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChurchDoor : MonoBehaviour
{
    public PlayerInventory playerInventory;

    void Update()
    {
        if(playerInventory.getDiamonds() > 6)
            gameObject.SetActive(false);
    }
}
