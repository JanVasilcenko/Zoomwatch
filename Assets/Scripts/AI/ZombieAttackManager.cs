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
            if (enemyManager.currentTarget.gameObject.GetComponent<HealthSystem>() != null) {
                enemyManager.currentTarget.gameObject.GetComponent<HealthSystem>().TakeDamage(damage);
            }
        }

        if (enemyManager.currentTarget.CompareTag(Tags.player)) {
            if (enemyManager.currentTarget.gameObject.GetComponent<HealthSystemPlayer>() != null) {
                enemyManager.currentTarget.gameObject.GetComponent<HealthSystemPlayer>().TakeDamage(damage);
            }
        }
    }
}
