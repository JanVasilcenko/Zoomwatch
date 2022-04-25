using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScavengerAttackState : State {
    public ScavengerWanderState scavengerWanderState;
    public ScavengerChaseState scavengerChaseState;

    public float fireRange = 15f;
    private bool isAttacking;
    private float attackDuration = 0.3f;
    private EnemyManager enemyManager;
    private AimingManager aimingManager;
    private void Awake() {
        aimingManager = GetComponentInParent<AimingManager>();
    }

    public override State Execute(EnemyManager enemyManager, EnemyAnimationController enemyAnimationController) {
        if (this.enemyManager == null) {
            this.enemyManager = enemyManager;
        }

        aimingManager.SetTargetTransform(enemyManager.currentTarget);

        if (enemyManager.currentTarget == null) {
            aimingManager.ResetTargetTransform();
            return scavengerWanderState;
        }

        if (enemyManager.currentTarget.CompareTag(Tags.zombie) && !enemyManager.currentTarget.gameObject.GetComponent<HealthSystem>().enabled) {
            enemyManager.navMeshAgent.SetDestination(enemyManager.gameObject.transform.position);
            enemyManager.currentTarget = null;
            aimingManager.ResetTargetTransform();

            return scavengerWanderState;
        }

        if (isTargetFar()) {
            return scavengerChaseState;
        }

        enemyManager.LookAtTarget();

        if (!isAttacking) {
            isAttacking = true;
            enemyAnimationController.SetAttackAnimation();
            Invoke("PerformAttack", attackDuration);
        }

        return this;
    }

    private bool isTargetFar() {
        if (Vector3.Distance(transform.position, enemyManager.currentTarget.transform.position) >= fireRange) {
            return true;
        }
        return false;
    }

    private void PerformAttack() {
        isAttacking = false;
    }
}
