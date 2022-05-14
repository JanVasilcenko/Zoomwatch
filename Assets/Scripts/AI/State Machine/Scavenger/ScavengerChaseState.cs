using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScavengerChaseState : State {
    public ScavengerAttackState scavengerAttackState;
    public ScavengerWanderState scavengerWanderState;
    private EnemyManager enemyManager;

    public override State Execute(EnemyManager enemyManager, EnemyAnimationController enemyAnimationController) {
        if (this.enemyManager == null) {
            this.enemyManager = enemyManager;
        }

        if (enemyManager.currentTarget == null) {
            return scavengerWanderState;
        }

        enemyManager.navMeshAgent.stoppingDistance = 100f;
        enemyManager.navMeshAgent.SetDestination(enemyManager.currentTarget.transform.position);
        enemyAnimationController.SetChasingAnimation();

        if (enemyManager.IsTargetClose()) {
            return scavengerAttackState;
        }

        return this;
    }
}
