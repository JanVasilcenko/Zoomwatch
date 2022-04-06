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
    [SerializeField]
    [Tooltip("Speed of an bullet")]
    protected float bulletSpeed = 400f;
    [SerializeField]
    [Tooltip("By how much will the zombie rotate when attacking the target")]
    private float rotationMultiplier = 8000f;

    protected Transform currentTarget;

    public virtual void AttackTarget() {
        navMeshAgent.isStopped = true;
        animator.SetBool("Chase", false);

        Quaternion quaternation = Quaternion.LookRotation((currentTarget.transform.position - transform.position).normalized);
        Quaternion rotateTo = new Quaternion(transform.rotation.x, quaternation.y, transform.rotation.z, quaternation.w);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, rotateTo, Time.deltaTime * rotationMultiplier);
    }

    protected virtual void OnTriggerEnter(Collider other) {
        if (other.gameObject.tag.Equals("Player") || other.gameObject.tag.Equals("Zombie")) {
            currentTarget = other.transform;
        }
    }
}
