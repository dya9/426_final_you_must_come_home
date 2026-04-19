using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class ApartmentMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 5f;
    public float rotationSpeed = 700f;

    [Header("Interaction Settings")]
    public float interactionRange = 2f;
    public LayerMask collectibleLayer;

    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();

        // Ensure the character doesn't fall over like a physics prop
        rb.freezeRotation = true;
        
        // Use Interpolate to smooth out camera jitter if the camera is a child
        rb.interpolation = RigidbodyInterpolation.Interpolate;
    }

    void Update()
    {
        // Interaction should stay in Update for frame-perfect button detection
        if (Input.GetKeyDown(KeyCode.X))
        {
            TryCollectObject();
        }
    }

    void FixedUpdate()
    {
        // Physics movement belongs in FixedUpdate to prevent shaking/stuttering
        HandleMovement();
    }

    void HandleMovement()
    {
        float moveInput = Input.GetAxis("Vertical");
        float turnInput = Input.GetAxis("Horizontal");

        // 1. Handle Rotation
        if (Mathf.Abs(turnInput) > 0.1f)
        {
            Quaternion deltaRotation = Quaternion.Euler(Vector3.up * turnInput * rotationSpeed * Time.fixedDeltaTime);
            rb.MoveRotation(rb.rotation * deltaRotation);
        }

        // 2. Handle Forward/Backward Movement
        Vector3 moveVector = transform.forward * moveInput * moveSpeed * Time.fixedDeltaTime;
        rb.MovePosition(rb.position + moveVector);
    }

    void TryCollectObject()
    {
        RaycastHit hit;
        // Visualizing the ray in the editor for debugging
        Debug.DrawRay(transform.position, transform.forward * interactionRange, Color.yellow, 0.5f);

        if (Physics.Raycast(transform.position, transform.forward, out hit, interactionRange, collectibleLayer))
        {
            Debug.Log("Collected: " + hit.collider.gameObject.name);
            Destroy(hit.collider.gameObject);
        }
    }
}