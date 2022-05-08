using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    private int diamondValue;
    private bool keyValue;
    // Start is called before the first frame update
    void Start()
    {
        diamondValue = 0;
        keyValue = false;
    }

    public void incrementDiamonds(){
        diamondValue++;
    }

    public void setKey(){
        keyValue = true;
    }

    public bool getKey(){
        return keyValue;
    }

    public int getDiamonds(){
        return diamondValue;
    }
}
