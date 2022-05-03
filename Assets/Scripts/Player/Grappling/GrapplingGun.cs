using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Code copied from Dani Devy - https://www.youtube.com/watch?v=Xgh4v1w5DxU

public class GrapplingGun : MonoBehaviour
{
    private Vector3 grapplePoint;
    public LayerMask whatIsGrappleable;
    public LayerMask whatIsGrapplePull;
    public Transform gunTip, camera, player;
    private float maxDistance = 100f;
    private SpringJoint joint;
    public AudioClip grappleSound;
    public AudioSource audioSource;
    void Update() {
        if (Input.GetButtonDown("Grapple")) {

            StartGrapple();
        }
        else if (Input.GetButtonUp("Grapple")) {
            StopGrapple();
        }
    }

    /// <summary>
    /// Call whenever we want to start a grapple
    /// </summary>
    void StartGrapple() {
        RaycastHit hit;
        if (Physics.Raycast(camera.position, camera.forward, out hit, maxDistance, whatIsGrappleable)) {
            grapplePoint = hit.point;
            joint = player.gameObject.AddComponent<SpringJoint>();
            joint.autoConfigureConnectedAnchor = false;
            joint.connectedAnchor = grapplePoint;

            float distanceFromPoint = Vector3.Distance(player.position, grapplePoint);

            //The distance grapple will try to keep from grapple point. 
            joint.maxDistance = distanceFromPoint * 0.8f;
            joint.minDistance = distanceFromPoint * 0.25f;

            //Adjust these values to fit your game.
            // joint.spring = 4.5f;
            // joint.damper = 7f;
            // joint.massScale = 4.5f;
            audioSource.PlayOneShot(grappleSound);

            if (Physics.Raycast(camera.position, camera.forward, out hit, maxDistance, whatIsGrapplePull))
            {
                joint.spring = 100f;
                joint.damper = 5f;
                joint.massScale = 4f;

            }

            else
            {
                joint.spring = 7f;
                joint.damper = 2f;
                joint.massScale = 4f; 
            }
        }
    }


    /// <summary>
    /// Call whenever we want to stop a grapple
    /// </summary>
    void StopGrapple() {
        Destroy(joint);
    }

    public bool IsGrappling() {

        return joint != null;
    }

    public Vector3 GetGrapplePoint() {
        return grapplePoint;
    }
}
