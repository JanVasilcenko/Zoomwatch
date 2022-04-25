using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieAttackManager : MonoBehaviour {
    public int damage;
    private EnemyManager enemyManager;

    private void Awake() {
        enemyManager = GetComponent<EnemyManager>();
    }

    public void Attack() {
        if (enemyManager.currentTarget == null) {
            return;
        }

        if (enemyManager.currentTarget.CompareTag(Tags.scavenger)) {
            enemyManager.currentTarget.gameObject.GetComponent<HealthSystem>().TakeDamage(damage);
        }
    }
}
