using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AimingManager : MonoBehaviour {
    public Transform targetTransform;
    public Transform aimTransform;
    public GameObject bullet;
    public Vector3 targetOffset;
    public float shootForce = 200f;

    public float angleLimit = 90f;
    public float distanceLimit = 1.5f;

    public int iterations = 10;
    [Range(0, 1)] public float weight = 1.0f;

    public HumanBone [] humanBones;
    Transform [] boneTransforms;

    void Start() {
        Animator animator = GetComponent<Animator>();
        boneTransforms = new Transform [humanBones.Length];
        for (int i = 0; i < boneTransforms.Length; i++) {
            boneTransforms [i] = animator.GetBoneTransform(humanBones [i].bone);
        }
    }

    Vector3 GetTargetPosition() {
        Vector3 targetDirection = (targetTransform.position + targetOffset) - aimTransform.position;
        Vector3 aimDirection = aimTransform.forward;
        float blendOut = 0.0f;

        float targetAngle = Vector3.Angle(targetDirection, aimDirection);
        if (targetAngle > angleLimit) {
            blendOut += (targetAngle - angleLimit) / 50.0f;
        }

        float targetDistance = targetDirection.magnitude;
        if (targetDistance < distanceLimit) {
            blendOut += distanceLimit - targetDistance;
        }

        Vector3 direction = Vector3.Slerp(targetDirection, aimDirection, blendOut);
        return aimTransform.position + direction;
    }

    private void LateUpdate() {
        if (aimTransform == null || targetTransform == null) {
            return;
        }

        if (targetTransform.CompareTag(Tags.zombie)) {
            targetOffset = new Vector3(0, 1.1f, 0);
        }
        else if (targetTransform.CompareTag(Tags.player)) {
            targetOffset = Vector3.zero;
        }

        Vector3 targetPosition = GetTargetPosition();
        for (int i = 0; i < iterations; i++) {
            for (int j = 0; j < boneTransforms.Length; j++) {
                AimAtTarget(boneTransforms [j], targetPosition, weight);
            }
        }
    }

    private void AimAtTarget(Transform bone, Vector3 targetPosition, float weight) {
        Vector3 aimDirection = aimTransform.forward;
        Vector3 targetDirection = targetPosition - aimTransform.position;
        Quaternion aimTowards = Quaternion.FromToRotation(aimDirection, targetDirection);
        Quaternion blendedRotation = Quaternion.Slerp(Quaternion.identity, aimTowards, weight);
        bone.rotation = blendedRotation * bone.rotation;
    }

    public void ResetTargetTransform() { targetTransform = null; }


    public void SetTargetTransform(Transform target) {
        targetTransform = target;
    }

    public void SetAimTransform(Transform aim) {
        aimTransform = aim;
    }

    public void Fire() {
        GameObject currentBullet = Instantiate(bullet, aimTransform.position, Quaternion.identity);
        currentBullet.GetComponent<Rigidbody>().AddForce(aimTransform.forward * shootForce, ForceMode.Impulse);
    }
}

[System.Serializable]
public class HumanBone {
    public HumanBodyBones bone;
}
