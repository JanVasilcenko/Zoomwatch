using System.Collections;
using UnityEngine;

public abstract class Zombie : Agent {
    [Header("General Settings")]
    [SerializeField]
    [Tooltip("This field sets the max delay where zombie stands, before wandering")]
    private float maxDelayWhenSearchingForNewWaypoint;
    [SerializeField]
    [Tooltip("How far will the zombie go when wandering in a single movement")]
    private float walkDistance;
    [SerializeField]
    [Tooltip("If this is enabled zombies will lose track of player if out of range")]
    private bool losesTrackWhenOutOfRange;

    [Header("Animation Settings")]
    [SerializeField]
    [Tooltip("By how much will the zombie rotate when attacking the target")]
    private float rotationMultiplier = 4000f;

    private Vector3 currentWalkPoint;
    private bool isWalkPointSet;

    protected Transform currentTarget;
    private IEnumerator searchWalkPointCycle;

    public virtual void Wander() {

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
        float randomZ = Random.Range(-walkDistance, walkDistance);
        float randomX = Random.Range(-walkDistance, walkDistance);

        currentWalkPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);
        float delay = Random.Range(0, maxDelayWhenSearchingForNewWaypoint);
        yield return new WaitForSeconds(delay);
        isWalkPointSet = true;
        searchWalkPointCycle = null;
    }

    public virtual void ChaseTarget() {
        animator.SetBool("Chase", true);
        animator.SetBool("Walking", false);
        navMeshAgent.SetDestination(currentTarget.transform.position);
    }

    public virtual void AttackTarget() {
        navMeshAgent.SetDestination(transform.position);
        animator.SetBool("Walking", false);
        animator.SetBool("Chase", false);

        Quaternion quaternation = Quaternion.LookRotation((currentTarget.transform.position - transform.position).normalized);
        Quaternion rotateTo = new Quaternion(transform.rotation.x, quaternation.y, transform.rotation.z, quaternation.w);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, rotateTo, Time.deltaTime * rotationMultiplier);
    }

    public bool isCloseToAttackTarget() {
        if (Vector3.Distance(transform.position, currentTarget.transform.position) <= navMeshAgent.stoppingDistance) {
            return true;
        }
        return false;
    }

    protected virtual void OnTriggerEnter(Collider other) {
        if (other.gameObject.tag.Equals("Player") || other.gameObject.tag.Equals("Scavenger")) {
            currentTarget = other.transform;
        }
    }

    private void OnTriggerExit(Collider other) {
        if (losesTrackWhenOutOfRange && other.gameObject.tag.Equals("Player")) {
            currentTarget = null;
        }
    }
}
