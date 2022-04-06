using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scavenger_Pistol : Human {
    [SerializeField]
    [Tooltip("Range on ")]
    private float range;
    [SerializeField]
    [Tooltip("Pause between the attacks MUST MATCH Animation")]
    private float timeBetweenAttacks = 1.5f;
    [SerializeField]
    [Range(0, 100)]
    [Tooltip("Percentage how accurate will AI be")]
    private int accuracy = 10;
    [SerializeField]
    private Transform firePoint;
    [SerializeField]
    private GameObject gunEffect;
    [SerializeField]
    private GameObject bullet;

    private IEnumerator attackCycle;
    protected override void AILogic() {
        if (!currentTarget) {
            animator.SetBool("Chase", false);
        }
        else {
            if (IsInRange()) {
                AttackTarget();
            }
            else {
                FollowTarget();
            }
        }
    }

    public void SpawnGunEffect() {
        Instantiate(gunEffect, firePoint.transform.position, Quaternion.identity);
        GameObject newbullet = Instantiate(bullet, firePoint.transform.position, Quaternion.identity);
        Vector3 accuracy = (firePoint.transform.forward * 400f) + IsAccurate();
        newbullet.GetComponent<Rigidbody>().velocity = accuracy;
    }

    private Vector3 IsAccurate() {
        int random = UnityEngine.Random.Range(0, 101);
        if (accuracy <= random) {
            if (UnityEngine.Random.Range(0, 2) == 1) {
                return new Vector3(-50f, 0, 0);
            }
            return new Vector3(50f, 0, 0);
        }

        return Vector3.zero;
    }

    public override void AttackTarget() {
        base.AttackTarget();
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

    private void FollowTarget() {
        navMeshAgent.isStopped = false;
        animator.SetBool("Chase", true);
        navMeshAgent.SetDestination(currentTarget.transform.position);
    }

    private bool IsInRange() {
        if (Vector3.Distance(transform.position, currentTarget.transform.position) <= range) {
            return true;
        }
        return false;
    }
}
