using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthSystem : MonoBehaviour
{
    public int maxHealth;
    public int currentHealth;

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(int damageTaken)
    {
        currentHealth -= damageTaken;

        if(currentHealth <= 0)
            Debug.Log("Die pls");
    }

    public void Heal(int healTaken)
    {
        currentHealth += healTaken;
        if(currentHealth > maxHealth)
            currentHealth = maxHealth;
    }
}
