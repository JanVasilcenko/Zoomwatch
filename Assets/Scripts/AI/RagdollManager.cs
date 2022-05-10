using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class RagdollManager : MonoBehaviour {
    private Rigidbody [] rigidbodies;
    private Animator animator;
    private Rigidbody mainRigidbody;
    private NavMeshAgent navMeshAgent;
    public int deleteAfterDeathCountdown;

    private void Awake() {
        rigidbodies = GetComponentsInChildren<Rigidbody>();
        animator = GetComponent<Animator>();
        mainRigidbody = GetComponent<Rigidbody>();
        navMeshAgent = GetComponent<NavMeshAgent>();
        DeactivateRagdoll();
    }

    public void ActivateRagdoll() {
        foreach (Rigidbody rigidbody in rigidbodies) {
            rigidbody.isKinematic = false;
        }
        mainRigidbody.freezeRotation = false;
        animator.enabled = false;
        navMeshAgent.enabled = false;
        GetComponent<EnemyManager>().enabled = false;
    }

    public void DeactivateRagdoll() {
        foreach (Rigidbody rigidbody in rigidbodies) {
            rigidbody.collisionDetectionMode = CollisionDetectionMode.ContinuousSpeculative;
            rigidbody.isKinematic = true;
        }
        animator.enabled = true;
    }

    public void DestroyCorpseTimer() {
        Invoke("DestroyCorpse", deleteAfterDeathCountdown);
    }

    private void DestroyCorpse() {
        Destroy(this.gameObject);
    }
}
