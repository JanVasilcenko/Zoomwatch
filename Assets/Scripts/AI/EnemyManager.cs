using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyManager : MonoBehaviour {
    [Header("General Enemy Settings")]
    public State currentState;
    public int detectionRadius;
    public int viewableAngle;
    public float turningSpeed = 200f;

    [HideInInspector] private EnemyAnimationController enemyAnimationController;
    [HideInInspector] public NavMeshAgent navMeshAgent;
    public Transform currentTarget;

    private void Awake() {
        navMeshAgent = GetComponent<NavMeshAgent>();
        enemyAnimationController = GetComponent<EnemyAnimationController>();
    }

    private void Start() {
        navMeshAgent.updatePosition = false;
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

    public void LookAtTarget() {
        Quaternion quaternion = Quaternion.LookRotation((currentTarget.transform.position - transform.position).normalized);
        Quaternion rotateTo = new Quaternion(transform.rotation.x, quaternion.y, transform.rotation.z, quaternion.w);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, rotateTo, Time.deltaTime * turningSpeed);
    }

    public bool IsTargetClose() {
        if (Vector3.Distance(transform.position, currentTarget.transform.position) <= navMeshAgent.stoppingDistance) {
            return true;
        }
        return false;
    }

    private void SwitchToNextState(State nextState) {
        currentState = nextState;
    }

    void OnAnimatorMove() {
        Vector3 position = enemyAnimationController.GetAnimator().rootPosition;
        position.y = navMeshAgent.nextPosition.y;
        transform.position = position;
        navMeshAgent.nextPosition = transform.position;
    }
}
