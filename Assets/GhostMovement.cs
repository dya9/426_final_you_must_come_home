using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class GhostMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 5f;        // Forward/Backward speed
    public float turnSpeed = 100f;     // Left/Right rotation speed
    public float floatSpeed = 4f;      // Up/Down speed

    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        
        // Setup physics for a floating entity
        rb.useGravity = false; 
        rb.linearDamping = 5f; 
        rb.angularDamping = 5f;
        
        // This keeps the ghost upright so it doesn't tip over like a fallen bottle
        rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;

        // Fix Camera position so you aren't looking at the inside of the model
        // Camera mainCam = GetComponentInChildren<Camera>();
        // if (mainCam != null)
        // {
        //     // Position the camera slightly in front of the center
        //     mainCam.transform.localPosition = new Vector3(0, 0.6f, 0.3f);
        //     mainCam.transform.localRotation = Quaternion.identity;
        // }
    }

    void FixedUpdate()
    {
        // 1. ROTATION (Turning Left/Right with A and D)
        float turnInput = Input.GetAxis("Horizontal");
        float turnAmount = turnInput * turnSpeed * Time.fixedDeltaTime;
        Quaternion turnRotation = Quaternion.Euler(0f, turnAmount, 0f);
        rb.MoveRotation(rb.rotation * turnRotation);

        // 2. FORWARD/BACKWARD (W and S)
        float moveInput = Input.GetAxis("Vertical");
        Vector3 moveDirection = transform.forward * moveInput * moveSpeed;

        // 3. FLOATING UP/DOWN (Space and Left Shift)
        float upDown = 0f;
        if (Input.GetKey(KeyCode.Space)) upDown = 1f;
        if (Input.GetKey(KeyCode.LeftShift)) upDown = -1f;
        Vector3 verticalDirection = Vector3.up * upDown * floatSpeed;

        // Apply the combined velocity
        rb.linearVelocity = moveDirection + verticalDirection;
    }
}