using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomZombieSkin : MonoBehaviour {
    public GameObject [] allSkins;

    void Start() {
        ActivateRandomSkin();
    }

    private void DeactivateAll() {
        foreach (GameObject skin in allSkins) {
            skin.SetActive(false);
        }
    }

    private void ActivateRandomSkin() {
        DeactivateAll();
        allSkins [CommonUtils.RandomBetweenTwoIntegers(0, allSkins.Length-1)].SetActive(true);
    }
}
