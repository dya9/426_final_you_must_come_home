using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 5.0f;
    public float rotationSpeed = 50.0f;

    Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
    }

    void Update()
    {
        // Rotate left / right (via Rigidbody to avoid physics conflicts)
        if (Input.GetKey(KeyCode.D))
            rb.MoveRotation(rb.rotation * Quaternion.Euler(0, rotationSpeed * Time.deltaTime, 0));
        else if (Input.GetKey(KeyCode.A))
            rb.MoveRotation(rb.rotation * Quaternion.Euler(0, -rotationSpeed * Time.deltaTime, 0));
    }

    void FixedUpdate()
    {
        // float moveInput = Input.GetKey(KeyCode.W) ? 1f : Input.GetKey(KeyCode.S) ? -1f : 0f;
        // rb.linearVelocity = transform.forward * moveInput * speed + Vector3.up * rb.linearVelocity.y;
        // Use FixedUpdate for physics-based movement
        if (Input.GetKey(KeyCode.W))
            rb.AddForce(transform.forward * speed, ForceMode.Acceleration);
        else if (Input.GetKey(KeyCode.S))
            rb.AddForce(-transform.forward * speed, ForceMode.Acceleration);

        // Cap max speed
        if (rb.linearVelocity.magnitude > speed)
            rb.linearVelocity = rb.linearVelocity.normalized * speed;
    }
}

