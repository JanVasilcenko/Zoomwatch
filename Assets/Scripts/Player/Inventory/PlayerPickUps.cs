using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPickUps : MonoBehaviour {
    public bool ammoPickUp;
    public bool healthPickUp;

    public int healthAmount, ammoAmount2, ammoAmount3, ammoAmount4;
    public AudioSource audioSource;
    public AudioClip ammoSound;
    public AudioClip healthSound;

    public Gun gun;
    public HealthSystemPlayer healthSystem;

    private void addAmmo() {
        gun.bulletAmmo2 += ammoAmount2;
        gun.bulletAmmo3 += ammoAmount3;
        gun.bulletAmmo4 += ammoAmount4;
        UpdateUIIfAmmoEquipped();
    }

    private void UpdateUIIfAmmoEquipped() {
        if (gun.bullet == gun.bullet1) {
            gun.UpdateAmmunitionText(-1);
        }
        else if (gun.bullet == gun.bullet2) {
            gun.UpdateAmmunitionText(gun.bulletAmmo2);
        }
        else if (gun.bullet == gun.bullet3) {
            gun.UpdateAmmunitionText(gun.bulletAmmo3);
        }
        else if (gun.bullet == gun.bullet4) {
            gun.UpdateAmmunitionText(gun.bulletAmmo4);
        }
    }

    private void addHealth() {
        audioSource.volume = 0.1f;
        audioSource.PlayOneShot(healthSound);
        healthSystem.Heal(healthAmount);
    }

    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag(Tags.player)) {
            if (ammoPickUp)
                addAmmo();
            if (healthPickUp) {
                if (healthSystem != null && healthSystem.currentHealth != healthSystem.maxHealth) {
                    Debug.Log("Got to heling");
                    addHealth();
                }
                else {
                    Debug.Log("Fuck heling");
                    return;
                }
            }

            //set visibility of object
            StartCoroutine(DisableObject());
        }
    }

    IEnumerator DisableObject() {
        if (ammoSound != null) {
            audioSource.PlayOneShot(ammoSound);
        }
        yield return new WaitForSeconds(0.4f);
        gameObject.SetActive(false);

    }
}
