using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthSystem : MonoBehaviour {
    public int maxHealth;
    public int currentHealth;
    private Rigidbody rigidbody;
    private RagdollManager ragdollManager;

    public AudioSource audioSource;
    public AudioClip deathSound;

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
            audioSource.PlayOneShot(deathSound);
            ragdollManager.ActivateRagdoll();
            ragdollManager.DestroyCorpseTimer();
            enabled = false;
        }
    }
    
    private void OnCollisionEnter(Collision collision) {
        if (collision.gameObject.CompareTag(Tags.bullet)) {
            rigidbody.velocity = rigidbody.velocity;
            rigidbody.angularVelocity = rigidbody.angularVelocity;
        }
    }


}
