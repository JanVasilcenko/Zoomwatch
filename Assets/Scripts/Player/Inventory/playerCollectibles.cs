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
        // currentMelee = "";
        currentGun = "";
        currentHealth = 100;
        currentAmmo = 10;
        currentMoney = 0;
        hasGrenade = false;

        setEquippedGun(currentGun);
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void setGun(string gun)
    {
        currentGun = gun;
        setEquippedGun(gun);
    }

    // private void setMelee(string melee)
    // {
    //     currentMelee = melee;
    // }

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
        if(other.CompareTag("weaponGrenade") && !hasGrenade){
            other.gameObject.SetActive(false);
            hasGrenade = true;
        }

        if(currentGun == ""){
            if(other.CompareTag("weaponKalashnikov")){
                other.gameObject.SetActive(false);
                setGun("kalashnikov");
            }
            if(other.CompareTag("weaponRifle")){
                other.gameObject.SetActive(false);
                setGun("rifle");
            }
            if(other.CompareTag("weaponUzi")){
                other.gameObject.SetActive(false);
                setGun("uzi");
            }
            if(other.CompareTag("weaponShotgun")){
                other.gameObject.SetActive(false);
                setGun("shotgun");
            }
        }

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

    public void setEquippedGun(string gun)
    {
        var uzi = GameObject.FindWithTag("equipWeaponUzi");
        var rifle = GameObject.FindWithTag("equipWeaponRifle");
        var kalashnikov = GameObject.FindWithTag("equipWeaponKalashnikov");
        var pistol = GameObject.FindWithTag("equipWeaponPistol");
        var shotgun = GameObject.FindWithTag("equipWeaponShotgun");
        var grenade = GameObject.FindWithTag("equipWeaponGrenade");

        uzi.SetActive(false);
        rifle.SetActive(false);
        kalashnikov.SetActive(false);
        pistol.SetActive(false);
        shotgun.SetActive(false);
        grenade.SetActive(false);
        
        switch (gun)
        {
            case "uzi":
                uzi.SetActive(true);
                break;
            case "rifle":
                rifle.SetActive(true);
                break;
            case "kalashnikov":
                kalashnikov.SetActive(true);
                break;
            case "shotgun":
                shotgun.SetActive(true);
                break;
            case "grenade":
                grenade.SetActive(true);
                break;
            default:
                pistol.SetActive(true);
            break;
        }
    }
}
