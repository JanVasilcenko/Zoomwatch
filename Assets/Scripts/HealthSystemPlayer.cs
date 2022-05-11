using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using UnityEngine;
using Random = UnityEngine.Random;

public class HealthSystemPlayer : MonoBehaviour {
    public int maxHealth;
    public int currentHealth;
    

    public HealthBarScript healthBarScript;
    public GameObject deathscreen;

    public AudioSource audioSource;
    public AudioClip damageSound;
    public AudioClip deathSound;

    private void Awake() {
       
    }

    private void Update()
    {
        
    }

    void Start() {
        currentHealth = maxHealth;
        healthBarScript.SetMaxHealth(maxHealth);
    }

    public void TakeDamage(int damageTaken) {
        currentHealth -= damageTaken;
        healthBarScript.SetHealth(currentHealth);
        audioSource.volume = Random.Range(0.10f, 0.15f);
        audioSource.pitch = Random.Range(0.9f, 1f);
        audioSource.PlayOneShot(damageSound);
        
        if (currentHealth <= 0) {
            deathscreen.SetActive(true);
            audioSource.PlayOneShot(deathSound);
            enabled = false;
            PauseMenu.isPaused = false;
            Time.timeScale = 0f;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }

    public void Heal(int healTaken) {
        currentHealth += healTaken;
        if (currentHealth > maxHealth)
            currentHealth = maxHealth;
    }

   


}