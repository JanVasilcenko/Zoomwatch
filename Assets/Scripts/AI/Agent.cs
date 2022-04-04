using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(SphereCollider))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(Rigidbody))]
public class Agent : MonoBehaviour {
    [Header("AI settings")]
    [SerializeField]
    [Tooltip("Brain Speed is how fast will the zombies react **PERFORMANCE HEAVY")]
    private float brainSpeed;
    [SerializeField]
    [Tooltip("Range how far will the AI notice player")]
    private float noticeRange = 6f;

    protected Animator animator;
    protected NavMeshAgent navMeshAgent;
    private float brainTimer;

    protected virtual void Awake() {
        navMeshAgent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        SetColliderRadius();
    }

    protected virtual void Update() {
        brainTimer += Time.deltaTime;
        if (brainSpeed <= brainTimer) {
            AILogic();
            brainTimer = 0;
        }
    }

    protected virtual void AILogic() { }

    private void SetColliderRadius() {
        GetComponent<SphereCollider>().radius = noticeRange;
    }
}
