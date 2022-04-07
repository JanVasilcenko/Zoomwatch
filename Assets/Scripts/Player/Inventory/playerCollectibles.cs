using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollectibles : MonoBehaviour
{
    public string currentGun;
    // public string currentMelee;
    public bool hasGrenade;
    public int currentHealth, currentAmmo, currentMoney;
    // Start is called before the first frame update
    void Start()
    {
        currentHealth = 100;
        currentAmmo = 10;
        currentMoney = 0;
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void addAmmo(int ammoAmount)
    {
        currentAmmo += ammoAmount;
    }

    private void addHealth(int healthAmount)
    {
        if(currentHealth + healthAmount < 100)
            currentHealth += healthAmount;
        else
            currentHealth = 100;
    }

    private void addMoney(int moneyAmount)
    {
        currentMoney += moneyAmount;
    }

    private void OnTriggerEnter(Collider other) {

        // if(other.CompareTag("weaponCrowbar")){
        //     other.gameObject.SetActive(false);
        //     setMelee("crowbar");
        // }
        // if(other.CompareTag("weaponKnife")){
        //     other.gameObject.SetActive(false);
        //     setMelee("knife");
        // }
        // if(other.CompareTag("weaponMachete")){
        //     other.gameObject.SetActive(false);
        //     setMelee("machette");
        // }
        
        if(other.CompareTag("collectibleAmmoSmall")){
            other.gameObject.SetActive(false);
            addAmmo(50);
        }
        if(other.CompareTag("collectibleAmmoLarge")){
            other.gameObject.SetActive(false);
            addAmmo(100);
        }
        if(other.CompareTag("collectibleMedicSmall")){
            other.gameObject.SetActive(false);
            addHealth(20);
        }
        if(other.CompareTag("collectibleMedicLarge")){
            other.gameObject.SetActive(false);
            addHealth(50);
        }
        if(other.CompareTag("collectibleSmallMoney")){
            other.gameObject.SetActive(false);
            addMoney(500);
        }
        if(other.CompareTag("collectibleMediumMoney")){
            other.gameObject.SetActive(false);
            addMoney(1000);
        }
        if(other.CompareTag("collectibleLargeMoney")){
            other.gameObject.SetActive(false);
            addMoney(1500);
        }
    }
}
