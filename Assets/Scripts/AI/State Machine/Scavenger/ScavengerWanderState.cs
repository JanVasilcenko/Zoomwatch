using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScavengerWanderState : State {
    private EnemyManager enemyManager;
    public float walkDistance;
    public ScavengerChaseState scavengerChaseState;
    public LayerMask detectionLayer;

    private Vector3 waypoint;
    private bool isWaypointSet;
    private bool isSearchingForWaypoint;

    public override State Execute(EnemyManager enemyManager, EnemyAnimationController enemyAnimationController) {
        if (this.enemyManager == null) {
            this.enemyManager = enemyManager;
        }

        Collider [] colliders = Physics.OverlapSphere(transform.position, enemyManager.detectionRadius);

        for (int i = 0; i < colliders.Length; i++) {

            if (colliders [i].CompareTag(Tags.player) || colliders [i].CompareTag(Tags.zombie)) {
                if (colliders [i].GetComponent<HealthSystem>() != null && !colliders [i].GetComponent<HealthSystem>().enabled) {
                    continue;
                }

                Vector3 direction = colliders [i].transform.position - transform.position;
                float viewableAngle = Vector3.Angle(direction, transform.forward);
                if (IsTargetInViewableAngle(viewableAngle)) {
                    enemyManager.currentTarget = colliders [i].transform;
                    return scavengerChaseState;
                }
            }
        }

        if (!isWaypointSet) {
            if (!isSearchingForWaypoint) {
                enemyAnimationController.ClearAllStates();
                isSearchingForWaypoint = true;
                Invoke("SearchForRandomWaypoint", CommonUtils.RandomBetweenTwoFloats(0, 4));
            }
        }
        else {
            if (IsWaypointClose()) {
                isWaypointSet = false;
            }
            enemyAnimationController.SetChasingAnimation();
        }
        return this;
    }

    private bool IsWaypointClose() {
        if (Vector3.Distance(waypoint, transform.position) < 1) {
            return true;
        }
        return false;
    }

    public bool IsTargetInViewableAngle(float viewableAngle) {
        return viewableAngle > -enemyManager.viewableAngle && viewableAngle < enemyManager.viewableAngle;
    }

    private void SearchForRandomWaypoint() {
        waypoint = new Vector3(transform.position.x + CommonUtils.RandomBetweenTwoFloats(-walkDistance, walkDistance), transform.position.y, transform.position.z + CommonUtils.RandomBetweenTwoFloats(-walkDistance, walkDistance));
        enemyManager.navMeshAgent.SetDestination(waypoint);
        isWaypointSet = true;
        isSearchingForWaypoint = false;
    }
}
