using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerCollectibles : MonoBehaviour
{
    public string currentGun, currentMelee;
    public bool hasGrenade;
    public int currentHealth, currentAmmo, currentMoney;
    
    public AudioClip healthSound;
    // Start is called before the first frame update
    void Start()
    {
        // currentMelee = "knife";
        // currentGun = "uzi";
        currentHealth = 100;
        currentAmmo = 10;
        currentMoney = 0;
        gameObject.AddComponent<AudioSource>();
        // hasGrenade = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // private void setGun(string gun)
    // {
    //     currentGun = gun;
    // }

    // private void setMelee(string melee)
    // {
    //     currentMelee = melee;
    // }

    private void addAmmo(int shotgunAmmo, int laserAmmo, int grenadeAmmo)
    {
        Gun gun = GetComponent<Gun>();
        gun.bulletAmmo2 += shotgunAmmo;
        gun.bulletAmmo3 += laserAmmo;
        gun.bulletAmmo4 += grenadeAmmo;
    }

    private void addHealth(int healthAmount)
    {
        if (currentHealth + healthAmount < 100)
        {
            Debug.Log("added health");
            currentHealth += healthAmount;
        }
            
        else
            currentHealth = 100;
    }

    private void addMoney(int moneyAmount)
    {
        currentMoney += moneyAmount;
    }

    private void OnTriggerEnter(Collider other) {
        // if(other.CompareTag("weaponGrenade")){
        //     other.gameObject.SetActive(false);
        //     if(!hasGrenade)
        //         hasGrenade = true;
        // }
        // if(other.CompareTag("weaponKalashnikov")){
        //     other.gameObject.SetActive(false);
        //     setGun("kalashnikov");
        // }
        // if(other.CompareTag("weaponRifle")){
        //     other.gameObject.SetActive(false);
        //     setGun("rifle");
        // }
        // if(other.CompareTag("weaponUzi")){
        //     other.gameObject.SetActive(false);
        //     setGun("uzi");
        // }
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
        if(other.CompareTag(Tags.smallAmmo)){
            other.gameObject.SetActive(false);
            addAmmo(10, 50, 2);
        }
        if(other.CompareTag(Tags.largeAmmo)){
            other.gameObject.SetActive(false);
            addAmmo(20, 100, 4);
        }
        if(other.CompareTag(Tags.smallMedic)){
            other.gameObject.SetActive(false);
            addHealth(20);
        }
        if(other.CompareTag(Tags.largeMedic)){
            AudioSource.PlayClipAtPoint(healthSound, gameObject.transform.position);

            other.gameObject.SetActive(false);
            addHealth(50);
        }
        if(other.CompareTag(Tags.smallMoney)){
            other.gameObject.SetActive(false);
            addMoney(500);
        }
        if(other.CompareTag(Tags.mediumMoney)){
            other.gameObject.SetActive(false);
            addMoney(1000);
        }
        if(other.CompareTag(Tags.largeMoney)){
            other.gameObject.SetActive(false);
            addMoney(1500);
        }
    }
}
