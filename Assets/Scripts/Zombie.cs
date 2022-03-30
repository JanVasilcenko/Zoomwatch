using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Zombie : MonoBehaviour {
    [Header("Settings")]
    [SerializeField]
    [Tooltip("This field sets the max delay where zombie stands, before wandering")]
    private float maxDelayWhenSearchingForNewWaypoint;
    [SerializeField]
    [Tooltip("How far will the zombie go when wandering in a single movement")]
    private float walkDistance;
    [SerializeField]
    [Tooltip("Brain Speed is how fast will the zombies react **PERFORMANCE HEAVY")]
    private float brainSpeed;

    private float timeBetweenAttacks = 1.18f;
    private Vector3 currentWalkPoint;
    private bool isWalkPointSet;
    private Transform currentTarget;
    private float brainTimer;

    private IEnumerator attackCycle;
    private IEnumerator searchWalkPointCycle;

    private NavMeshAgent navMeshAgent;
    private Animator animator;

    void Awake() {
        animator = GetComponent<Animator>();
        navMeshAgent = GetComponent<NavMeshAgent>();
    }

    void Update() {
        brainTimer += Time.deltaTime;
        if (brainSpeed <= brainTimer) {
            ZombieLogic();
            brainTimer = 0;
        }
    }

    private void ZombieLogic() {
        if (!currentTarget) {
            Wander();
        }
        else {
            if (isCloseToAttackTarget()) {
                AttackTarget();
            }
            else {
                ChaseTarget();
            }
        }
    }

    private void Wander() {
        if (!isWalkPointSet) {
            animator.SetBool("Walking", false);
            animator.SetBool("Chase", false);
            if (searchWalkPointCycle == null) {
                searchWalkPointCycle = SearchForRandomWalkPoint();
                StartCoroutine(searchWalkPointCycle);
            }
        }

        if (isWalkPointSet) {
            animator.SetBool("Walking", true);
            animator.SetBool("Chase", false);
            navMeshAgent.SetDestination(currentWalkPoint);
        }

        Vector3 distance = currentWalkPoint - transform.position;

        if (distance.sqrMagnitude < 1) {
            isWalkPointSet = false;
        }
    }

    private IEnumerator SearchForRandomWalkPoint() {
        float randomZ = UnityEngine.Random.Range(-walkDistance, walkDistance);
        float randomX = UnityEngine.Random.Range(-walkDistance, walkDistance);

        currentWalkPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);
        float delay = UnityEngine.Random.Range(0, maxDelayWhenSearchingForNewWaypoint);
        yield return new WaitForSeconds(delay);
        isWalkPointSet = true;
        searchWalkPointCycle = null;
    }

    private void ChaseTarget() {
        animator.SetBool("Chase", true);
        animator.SetBool("Walking", false);
        navMeshAgent.SetDestination(currentTarget.transform.position);
    }

    private void AttackTarget() {
        navMeshAgent.SetDestination(transform.position);
        animator.SetBool("Walking", false);
        animator.SetBool("Chase", false);

        Quaternion quaternation = Quaternion.LookRotation((currentTarget.transform.position - transform.position).normalized);
        Quaternion rotateTo = new Quaternion(transform.rotation.x, quaternation.y, transform.rotation.z, quaternation.w);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, rotateTo, Time.deltaTime * 4000f);

        if (attackCycle == null) {
            attackCycle = AttackLogic();
            StartCoroutine(attackCycle);
        }
    }

    private IEnumerator AttackLogic() {
        animator.SetTrigger("Attack");
        yield return new WaitForSeconds(timeBetweenAttacks);
        attackCycle = null;
    }

    private bool isCloseToAttackTarget() {
        if (Vector3.Distance(transform.position, currentTarget.transform.position) <= navMeshAgent.stoppingDistance) {
            return true;
        }
        return false;
    }

    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.tag.Equals("Player")) {
            currentTarget = other.transform;
        }
    }
}
