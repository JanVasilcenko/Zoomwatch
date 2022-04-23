using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackState : State {
    public bool isFast;
    public ChaseState chaseState;
    public WanderState wanderState;
    public float attackDuration = 1.5f;

    private bool isAttacking;
    private EnemyManager enemyManager;

    public override State Execute(EnemyManager enemyManager, EnemyAnimationController enemyAnimationController) {
        if (this.enemyManager == null) {
            this.enemyManager = enemyManager;
        }

        if (enemyManager.currentTarget == null) {
            return wanderState;
        }

        LookAtTarget();

        if (!IsTargetClose()) {
            return chaseState;
        }

        if (!isAttacking) {
            if (isFast) {
                DisableMovement();
            }
            isAttacking = true;
            enemyAnimationController.SetAttackAnimation();
            Invoke("PerformAttack", attackDuration);
        }
        return this;
    }

    private void PerformAttack() {
        isAttacking = false;
        EnableMovement();
    }

    private void LookAtTarget() {
        Quaternion quaternion = Quaternion.LookRotation((enemyManager.currentTarget.transform.position - transform.position).normalized);
        Quaternion rotateTo = new Quaternion(transform.rotation.x, quaternion.y, transform.rotation.z, quaternion.w);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, rotateTo, Time.deltaTime * 80f);
    }

    private bool IsTargetClose() {
        if (Vector3.Distance(transform.position, enemyManager.currentTarget.transform.position) <= enemyManager.navMeshAgent.stoppingDistance) {
            return true;
        }
        return false;
    }

    private bool IsLookingAtTarget() {
        Vector3 direction = enemyManager.currentTarget.transform.position - transform.position;
        float viewableAngle = Vector3.Angle(direction, transform.forward);

        if (viewableAngle > -40 && viewableAngle < 40) {
            return true;
        }
        return false;
    }

    private void DisableMovement() {
        enemyManager.navMeshAgent.isStopped = true;
    }

    private void EnableMovement() {
        enemyManager.navMeshAgent.isStopped = false;
    }
}
