using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieWanderState : State {
    public int idleTime;
    public LayerMask detectionLayer;
    public ZombieChaseState chaseState;
    public float walkDistance;

    private bool isSearchingForWaypoint;
    private Vector3 waypoint;
    private bool isWaypointSet;
    private EnemyManager enemyManager;

    public override State Execute(EnemyManager enemyManager, EnemyAnimationController enemyAnimationController) {
        if (this.enemyManager == null) {
            this.enemyManager = enemyManager;
        }

        Collider [] colliders = Physics.OverlapSphere(transform.position, enemyManager.detectionRadius);

        for (int i = 0; i < colliders.Length; i++) {

            if (colliders [i].CompareTag(Tags.player) || colliders [i].CompareTag(Tags.scavenger)) {
                Vector3 direction = colliders [i].transform.position - transform.position;
                float viewableAngle = Vector3.Angle(direction, transform.forward);
                if (IsTargetInViewableAngle(viewableAngle)) {
                    enemyManager.currentTarget = colliders [i].transform;
                    return chaseState;
                }
            }
        }

        if (!isWaypointSet) {
            if (!isSearchingForWaypoint) {
                enemyAnimationController.ClearAllStates();
                isSearchingForWaypoint = true;
                Invoke("SearchForRandomWaypoint", CommonUtils.RandomBetweenTwoFloats(0, idleTime));
            }
        }
        else {
            if (IsWaypointClose()) {
                isWaypointSet = false;
            }
            enemyAnimationController.SetWalkingAnimation();
        }
        return this;
    }

    private bool IsTargetInViewableAngle(float viewableAngle) {
        return viewableAngle > -enemyManager.viewableAngle && viewableAngle < enemyManager.viewableAngle;
    }

    private void SearchForRandomWaypoint() {
        waypoint = new Vector3(transform.position.x + CommonUtils.RandomBetweenTwoFloats(-walkDistance, walkDistance), transform.position.y, transform.position.z + CommonUtils.RandomBetweenTwoFloats(-walkDistance, walkDistance));
        enemyManager.navMeshAgent.SetDestination(waypoint);
        isWaypointSet = true;
        isSearchingForWaypoint = false;
    }

    private bool IsWaypointClose() {
        if (Vector3.Distance(waypoint, transform.position) < 1) {
            return true;
        }
        return false;
    }
}
