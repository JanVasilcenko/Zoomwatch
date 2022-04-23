using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyManager : MonoBehaviour {
    [Header("General Enemy Settings")]
    public State currentState;
    public int detectionRadius;
    public int viewableAngle;

    [HideInInspector] private EnemyAnimationController enemyAnimationController;
    [HideInInspector] public NavMeshAgent navMeshAgent;
    public Transform currentTarget;

    private void Awake() {
        navMeshAgent = GetComponent<NavMeshAgent>();
        enemyAnimationController = GetComponent<EnemyAnimationController>();
    }

    private void FixedUpdate() {
        ExecuteCurrentState();
    }

    private void ExecuteCurrentState() {
        if (currentState != null) {
            State nextState = currentState.Execute(this, enemyAnimationController);

            if (nextState != null) {
                SwitchToNextState(nextState);
            }
        }
    }

    private void SwitchToNextState(State nextState) {
        currentState = nextState;
    }
}
