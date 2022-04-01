using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(SphereCollider))]
[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(Rigidbody))]
public class Zombie : MonoBehaviour {
    [Header("General Settings")]
    [SerializeField]
    [Tooltip("This field sets the max delay where zombie stands, before wandering")]
    private float maxDelayWhenSearchingForNewWaypoint;
    [SerializeField]
    [Tooltip("How far will the zombie go when wandering in a single movement")]
    private float walkDistance;
    [SerializeField]
    [Tooltip("Range how far will the zombies notice player")]
    private float noticeRange = 6f;
    [SerializeField]
    [Tooltip("Brain Speed is how fast will the zombies react **PERFORMANCE HEAVY")]
    private float brainSpeed;
    [SerializeField]
    [Tooltip("If this is enabled zombies will lose track of player if out of range")]
    private bool losesTrackWhenOutOfRange;


    [Header("Fast Zombies")]
    [SerializeField]
    [Tooltip("Determines if the zombie is fast")]
    private bool isFastZombie;
    [SerializeField]
    [Tooltip("Speed of zombie running")]
    private float fastZombieSpeed = 5f;

    [Header("Animation Settings")]
    [SerializeField]
    [Tooltip("Pause between the attacks MUST MATCH Animation")]
    private float timeBetweenAttacks = 1.38f;
    [SerializeField]
    [Tooltip("By how much will the zombie rotate when attacking the target")]
    private float rotationMultiplier = 4000f;
    [SerializeField]
    [Range(0, 100)]
    [Tooltip("Chance for zombie to scream on seeing target")]
    private int chanceToScream;

    private bool screamed;
    private Vector3 currentWalkPoint;
    private bool isWalkPointSet;
    private Transform currentTarget;
    private float brainTimer;
    private bool screaming;
    private IEnumerator attackCycle;
    private IEnumerator searchWalkPointCycle;

    private NavMeshAgent navMeshAgent;
    private Animator animator;

    void Awake() {
        animator = GetComponent<Animator>();
        navMeshAgent = GetComponent<NavMeshAgent>();
        GetComponent<SphereCollider>().radius = noticeRange;
    }

    void Update() {
        brainTimer += Time.deltaTime;
        if (brainSpeed <= brainTimer) {
            if (!screaming) {
                ZombieLogic();
            }
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
        decreaseSpeedIfFastZombie();

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
        increaseSpeedIfFastZombie();
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
        transform.rotation = Quaternion.RotateTowards(transform.rotation, rotateTo, Time.deltaTime * rotationMultiplier);

        if (attackCycle == null) {
            disableMovementIfFastZombie();
            attackCycle = AttackLogic();
            StartCoroutine(attackCycle);
        }
    }

    private IEnumerator AttackLogic() {
        animator.SetTrigger("Attack");
        yield return new WaitForSeconds(timeBetweenAttacks);
        attackCycle = null;
        enableMovementIfFastZombie();
    }

    private bool isCloseToAttackTarget() {
        if (Vector3.Distance(transform.position, currentTarget.transform.position) <= navMeshAgent.stoppingDistance) {
            return true;
        }
        return false;
    }

    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.tag.Equals("Player")) {

            if (isFastZombie && !screamed) {

                if (isLookingApproxToTarget(other.transform.position) && isGoingToScream()) {
                    StartCoroutine("zombieScream");
                }

            }
            screamed = true;
            currentTarget = other.transform;
        }
    }

    private void OnTriggerExit(Collider other) {
        if (losesTrackWhenOutOfRange && other.gameObject.tag.Equals("Player")) {
            currentTarget = null;
        }
    }

    private IEnumerator zombieScream() {
        disableMovementIfFastZombie();
        screaming = true;
        animator.SetTrigger("Scream");
        yield return new WaitForSeconds(2.4f);
        enableMovementIfFastZombie();
        screaming = false;
        StopCoroutine("zombieScream");
    }

    private bool isLookingApproxToTarget(Vector3 targetPos) {
        Vector3 direction = (targetPos - transform.position).normalized;
        float howMuchLookingAtTarget = Vector3.Dot(direction, transform.forward);

        if (howMuchLookingAtTarget > 0.9) {
            return true;
        }

        return false;
    }

    private bool isGoingToScream() {
        int chance = UnityEngine.Random.Range(0, 100);

        if (chance < chanceToScream) {
            return true;
        }
        return false;
    }

    private void decreaseSpeedIfFastZombie() {
        if (isFastZombie) {
            navMeshAgent.speed = 0.1f;
        }
    }

    private void increaseSpeedIfFastZombie() {
        if (isFastZombie) {
            navMeshAgent.speed = fastZombieSpeed;
        }
    }

    private void disableMovementIfFastZombie() {
        if (isFastZombie) {
            navMeshAgent.isStopped = true;
        }
    }

    private void enableMovementIfFastZombie() {
        if (isFastZombie) {
            navMeshAgent.isStopped = false;
        }
    }
}
