using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Human : Agent {
    [SerializeField]
    [Tooltip("How far will the zombie go when wandering in a single movement")]
    private float walkDistance;
    [SerializeField]
    [Tooltip("This field sets the max delay where zombie stands, before wandering")]
    private float maxDelayWhenSearchingForNewWaypoint;

    protected Transform currentTarget;

    private Vector3 currentWalkPoint;
    private bool isWalkPointSet;

    private IEnumerator searchWalkPointCycle;

    public virtual void AttackTarget() {
        navMeshAgent.isStopped = true;
        animator.SetBool("Chase", false);

        Quaternion quaternation = Quaternion.LookRotation((currentTarget.transform.position - transform.position).normalized);
        Quaternion rotateTo = new Quaternion(transform.rotation.x, quaternation.y, transform.rotation.z, quaternation.w);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, rotateTo, Time.deltaTime * 8000f);
    }

    protected virtual void OnTriggerEnter(Collider other) {
        if (other.gameObject.tag.Equals("Player") || other.gameObject.tag.Equals("Zombie")) {
            currentTarget = other.transform;
        }
    }
}
