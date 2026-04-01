// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;

// public class PlayerMovement : MonoBehaviour
// {
//     public float speed = 5.0f;
//     public float rotationSpeed = 50.0f;

//     Rigidbody rb;

//     void Start()
//     {
//         rb = GetComponent<Rigidbody>();
//     }

//     void Update()
//     {
//         // Rotate left / right (via Rigidbody to avoid physics conflicts)
//         if (Input.GetKey(KeyCode.D))
//             rb.MoveRotation(rb.rotation * Quaternion.Euler(0, rotationSpeed * Time.deltaTime, 0));
//         else if (Input.GetKey(KeyCode.A))
//             rb.MoveRotation(rb.rotation * Quaternion.Euler(0, -rotationSpeed * Time.deltaTime, 0));
//     }

//     void FixedUpdate()
//     {
//         // Use FixedUpdate for physics-based movement
//         if (Input.GetKey(KeyCode.W))
//             rb.AddForce(transform.forward * speed, ForceMode.Acceleration);
//         else if (Input.GetKey(KeyCode.S))
//             rb.AddForce(-transform.forward * speed, ForceMode.Acceleration);

//         // Cap max speed
//         if (rb.linearVelocity.magnitude > speed)
//             rb.linearVelocity = rb.linearVelocity.normalized * speed;
//     }
// }

// using UnityEngine;

// public class PlayerMovement : MonoBehaviour
// {
//     public float speed = 5.0f;
//     public float rotationSpeed = 100.0f; // Increased for better feel

//     Rigidbody rb;

//     void Start()
//     {
//         rb = GetComponent<Rigidbody>();
        
//         // This ensures the bug is fixed even if you forget to check the boxes in the Inspector
//         rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
//     }

//     void FixedUpdate()
//     {
//         // 1. Handling Rotation
//         float rotationInput = 0;
//         if (Input.GetKey(KeyCode.D)) rotationInput = 1;
//         else if (Input.GetKey(KeyCode.A)) rotationInput = -1;

//         if (rotationInput != 0)
//         {
//             Quaternion deltaRotation = Quaternion.Euler(0, rotationInput * rotationSpeed * Time.fixedDeltaTime, 0);
//             rb.MoveRotation(rb.rotation * deltaRotation);
//         }

//         // 2. Handling Movement
//         float moveInput = 0;
//         if (Input.GetKey(KeyCode.W)) moveInput = 1;
//         else if (Input.GetKey(KeyCode.S)) moveInput = -1;

//         if (moveInput != 0)
//         {
//             rb.AddForce(transform.forward * moveInput * speed, ForceMode.Acceleration);
//         }

//         // 3. Cap max speed (using the newer linearVelocity property)
//         if (rb.linearVelocity.magnitude > speed)
//         {
//             rb.linearVelocity = rb.linearVelocity.normalized * speed;
//         }
//     }
// }
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 5.0f;
    public float rotationSpeed = 100.0f;

    Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        
        // 1. Lock Rotation on X and Z so the player stays upright
        // Lock Position Y is UNCHECKED in Inspector to allow natural physics
        rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
    }

    void FixedUpdate()
    {
        // 2. THE FIX: Manually kill any spinning energy caused by collisions
        // This prevents the "uncontrollable spinning" even if a collision is violent.
        rb.angularVelocity = Vector3.zero;

        // 3. Smooth Rotation
        float rotationInput = 0;
        if (Input.GetKey(KeyCode.D)) rotationInput = 1;
        else if (Input.GetKey(KeyCode.A)) rotationInput = -1;

        float turn = rotationInput * rotationSpeed * Time.fixedDeltaTime;
        Quaternion turnRotation = Quaternion.Euler(0f, turn, 0f);
        rb.MoveRotation(rb.rotation * turnRotation);

        // 4. Direct Position Movement
        float moveInput = 0;
        if (Input.GetKey(KeyCode.W)) moveInput = 1;
        else if (Input.GetKey(KeyCode.S)) moveInput = -1;

        // MovePosition provides more stability than AddForce for character movement
        Vector3 movement = transform.forward * moveInput * speed * Time.fixedDeltaTime;
        rb.MovePosition(rb.position + movement);
    }
}