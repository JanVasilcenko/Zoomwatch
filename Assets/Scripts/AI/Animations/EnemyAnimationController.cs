using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnimationController : MonoBehaviour {
    public int chaseAnimationsCount = 1;
    public int attackAnimationsCount = 1;
    private Animator animator;

    private void Awake() {
        animator = GetComponent<Animator>();
        animator.logWarnings = false;
    }

    public Animator GetAnimator() {
        return animator;
    }

    public void ClearAllStates() {
        animator.SetBool(AnimationParameters.walking, false);
        animator.SetBool(AnimationParameters.chasing, false);
    }

    public void SetChasingAnimation() {
        if (!animator.GetBool(AnimationParameters.chasing)) {
            ClearAllStates();
            animator.SetBool(AnimationParameters.chasing, true);
            animator.SetInteger(AnimationParameters.chasingAnimType, DetermineChaseAnimationType());
        }
    }

    public void SetWalkingAnimation() {
        ClearAllStates();
        animator.SetBool(AnimationParameters.walking, true);
    }

    public void SetAttackAnimation() {
        ClearAllStates();
        animator.SetTrigger(AnimationParameters.attack);
        animator.SetInteger(AnimationParameters.attackingAnimType, DetermineAttackAnimationType());
    }

    private int DetermineChaseAnimationType() {
        return CommonUtils.RandomBetweenTwoIntegers(1, chaseAnimationsCount);
    }

    private int DetermineAttackAnimationType() {
        return CommonUtils.RandomBetweenTwoIntegers(1, chaseAnimationsCount);
    }
}
