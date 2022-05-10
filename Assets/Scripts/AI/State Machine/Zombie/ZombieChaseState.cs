using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieChaseState : State {
    private EnemyManager enemyManager;
    public ZombieAttackState attackState;
    public ZombieWanderState wanderState;

    public override State Execute(EnemyManager enemyManager, EnemyAnimationController enemyAnimationController) {
        if (this.enemyManager == null) {
            this.enemyManager = enemyManager;
        }

        if (enemyManager.currentTarget == null) {
            return wanderState;
        }

        if (enemyManager.IsTargetClose()) {
            return attackState;
        }

        enemyManager.navMeshAgent.SetDestination(enemyManager.currentTarget.transform.position);
        enemyManager.LookAtTarget();
        enemyAnimationController.SetChasingAnimation();
        return this;
    }
}
