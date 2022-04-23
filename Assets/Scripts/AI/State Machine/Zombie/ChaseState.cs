using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChaseState : State {
    public bool isFast;
    private EnemyManager enemyManager;
    public AttackState attackState;
    public WanderState wanderState;

    public override State Execute(EnemyManager enemyManager, EnemyAnimationController enemyAnimationController) {
        if (this.enemyManager == null) {
            this.enemyManager = enemyManager;
        }

        if (enemyManager.currentTarget == null) {
            return wanderState;
        }

        if (IsTargetClose()) {
            return attackState;
        }

        enemyManager.navMeshAgent.SetDestination(enemyManager.currentTarget.transform.position);
        LookAtTarget();
        if (isFast) {
            EnableMovement();
            IncreaseSpeed();
        }
        enemyAnimationController.SetChasingAnimation();
        return this;
    }

    private void LookAtTarget() {
        Quaternion quaternion = Quaternion.LookRotation((enemyManager.currentTarget.transform.position - transform.position).normalized);
        Quaternion rotateTo = new Quaternion(transform.rotation.x, quaternion.y, transform.rotation.z, quaternion.w);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, rotateTo, Time.deltaTime * 8000000000f);
    }

    private bool IsTargetClose() {
        if (Vector3.Distance(transform.position, enemyManager.currentTarget.transform.position) <= enemyManager.navMeshAgent.stoppingDistance) {
            return true;
        }
        return false;
    }

    private void IncreaseSpeed() {
        enemyManager.navMeshAgent.speed = 5f;
    }

    private void EnableMovement() {
        enemyManager.navMeshAgent.isStopped = false;
    }
}
