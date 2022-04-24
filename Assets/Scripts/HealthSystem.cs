using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthSystem : MonoBehaviour {
    public int maxHealth;
    public int currentHealth;
    private Rigidbody rigidbody;
    private RagdollManager ragdollManager;

    private void Awake() {
        ragdollManager = GetComponent<RagdollManager>();
        rigidbody = GetComponent<Rigidbody>();
    }

    void Start() {
        currentHealth = maxHealth;
    }

    public void TakeDamage(int damageTaken) {
        currentHealth -= damageTaken;

        if (currentHealth <= 0) {
            ragdollManager.ActivateRagdoll();
        }
    }

    public void Heal(int healTaken) {
        currentHealth += healTaken;
        if (currentHealth > maxHealth)
            currentHealth = maxHealth;
    }

    private void OnCollisionEnter(Collision collision) {
        if (collision.gameObject.CompareTag(Tags.bullet)) {
            rigidbody.velocity = rigidbody.velocity;
            rigidbody.angularVelocity = rigidbody.angularVelocity;
        }
    }
}
