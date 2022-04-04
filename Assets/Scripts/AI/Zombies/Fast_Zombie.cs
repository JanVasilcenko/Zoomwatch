using System.Collections;
using UnityEngine;

public class Fast_Zombie : Zombie {
    [Header("Fast Zombies")]
    [SerializeField]
    [Tooltip("Speed of zombie running")]
    private float fastZombieSpeed = 5f;
    [SerializeField]
    [Range(0, 100)]
    [Tooltip("Chance for zombie to scream on seeing target")]
    private int chanceToScream;
    [SerializeField]
    [Tooltip("Pause between the attacks MUST MATCH Animation")]
    private float timeBetweenAttacks = 1.38f;
    [SerializeField]
    [Tooltip("Approximate Percentage Precision on which zombie will scream given looking at the target")]
    private int percentageApproxToScreamAtTarget;

    private bool screaming;
    private bool screamed;
    private IEnumerator attackCycle;

    protected override void AILogic() {
        if (!screaming) {
            if (!currentTarget) {
                DecreaseSpeed();
                Wander();
            }
            else {
                if (isCloseToAttackTarget()) {
                    AttackTarget();
                }
                else {
                    IncreaseSpeed();
                    ChaseTarget();
                }
            }
        }
    }

    public override void AttackTarget() {
        base.AttackTarget();

        if (attackCycle == null) {
            DisableMovement();
            attackCycle = AttackLogic();
            StartCoroutine(attackCycle);
        }
    }

    private IEnumerator AttackLogic() {
        animator.SetTrigger("Attack");
        yield return new WaitForSeconds(timeBetweenAttacks);
        attackCycle = null;
        EnableMovement();
    }

    protected override void OnTriggerEnter(Collider other) {
        if (other.gameObject.tag.Equals("Player")) {
            if (!screamed) {
                if (isLookingApproxToTarget(other.transform.position) && isGoingToScream()) {
                    StartCoroutine("zombieScream");
                }
            }
            screamed = true;
            currentTarget = other.transform;
        }
    }

    private bool isLookingApproxToTarget(Vector3 targetPos) {
        Vector3 direction = (targetPos - transform.position).normalized;
        float howMuchLookingAtTarget = Vector3.Dot(direction, transform.forward);

        if (howMuchLookingAtTarget > 1f - (percentageApproxToScreamAtTarget/100)) {
            return true;
        }

        return false;
    }

    private bool isGoingToScream() {
        int chance = Random.Range(0, 100);

        if (chance < chanceToScream) {
            return true;
        }
        return false;
    }

    private void DecreaseSpeed() {
        navMeshAgent.speed = 0.1f;
    }

    private void IncreaseSpeed() {
        navMeshAgent.speed = fastZombieSpeed;
    }

    private void DisableMovement() {
        navMeshAgent.isStopped = true;
    }

    private void EnableMovement() {
        navMeshAgent.isStopped = false;
    }
}
