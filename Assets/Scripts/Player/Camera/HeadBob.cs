using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeadBob : MonoBehaviour {
    [SerializeField] private bool enable = true;

    [SerializeField, Range(0, 0.1f)] private float amplitude = 0.015f;
    [SerializeField, Range(0, 30)] private float frequency = 10.0f;

    [SerializeField] private Transform camera = null;
    [SerializeField] private Transform cameraHolder = null;

    private float toggleSpeed = 30.0f;
    private Vector3 startPosition;
    private bool grounded;
    public float playerHeight;
    public LayerMask whatIsGround;
    private Rigidbody rb;

    void Start() {
        rb = GetComponent<Rigidbody>();
        startPosition = camera.localPosition;
    }

    void Update() {
        grounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.2f, whatIsGround);
        if (!enable)
            return;
        CheckMotion();
    }

    private void PlayMotion(Vector3 motion) {
        camera.localPosition += motion;
    }

    private void CheckMotion() {
        float speed = new Vector3(rb.velocity.x, 0f, rb.velocity.z).magnitude;

        ResetPosition();

        if (speed < toggleSpeed)
            return;
        if (!grounded)
            return;

        PlayMotion(FootStepMotion());
    }

    private Vector3 FootStepMotion() {
        Vector3 position = Vector3.zero;
        position.y += Mathf.Sin(Time.deltaTime * frequency) * amplitude;
        position.x += Mathf.Cos(Time.deltaTime * frequency / 2) * amplitude * 2;
        return position;
    }

    private void ResetPosition() {
        if (camera.localPosition == startPosition)
            return;
        camera.localPosition = Vector3.Lerp(camera.localPosition, startPosition, 1 * Time.deltaTime);
    }
}
