using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slow_Zombie : Zombie {
    [SerializeField]
    [Tooltip("Pause between the attacks MUST MATCH Animation")]
    private float timeBetweenAttacks = 1.38f;

    private IEnumerator attackCycle;

    protected override void AILogic() {
        if (!currentTarget) {
            Wander();
        }
        else {
            if (base.isCloseToAttackTarget()) {
                AttackTarget();
            }
            else {
                base.ChaseTarget();
            }
        }
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
}
