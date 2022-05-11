using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using UnityEngine;

public class HealthSystemPlayer : MonoBehaviour {
    public int maxHealth;
    public int currentHealth;
    

    public HealthBarScript healthBarScript;

    public AudioSource audioSource;
    public AudioClip damageSound;
    public AudioClip deathSound;

    private void Awake() {
       
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            TakeDamage(20);
        }
    }

    void Start() {
        currentHealth = maxHealth;
        healthBarScript.SetMaxHealth(maxHealth);
    }

    public void TakeDamage(int damageTaken) {
        currentHealth -= damageTaken;
        healthBarScript.SetHealth(currentHealth);
        audioSource.PlayOneShot(damageSound);
        if (currentHealth <= 0) {
            audioSource.PlayOneShot(deathSound);
            enabled = false;
        }
    }

    public void Heal(int healTaken) {
        currentHealth += healTaken;
        if (currentHealth > maxHealth)
            currentHealth = maxHealth;
    }

   


}