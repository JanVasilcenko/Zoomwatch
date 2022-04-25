using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieAttackState : State {
    public ZombieChaseState chaseState;
    public ZombieWanderState wanderState;
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

        if (enemyManager.currentTarget.CompareTag(Tags.scavenger)) {
            if (!enemyManager.currentTarget.GetComponent<HealthSystem>().enabled) {
                enemyManager.currentTarget = null;
                return wanderState;
            }
        }

        if (!enemyManager.IsTargetClose()) {
            return chaseState;
        }

        enemyManager.LookAtTarget();

        if (!isAttacking) {
            isAttacking = true;
            enemyAnimationController.SetAttackAnimation();
            Invoke("PerformAttack", attackDuration);
        }
        return this;
    }

    private void PerformAttack() {
        isAttacking = false;
    }
}
