using UnityEngine;

public class Gate : MonoBehaviour
{
    private HingeJoint hinge;
    private bool isOpen = false;
    private bool playerNearby = false;

    void Start()
    {
        hinge = GetComponent<HingeJoint>();

        JointLimits limits = hinge.limits;
        limits.min = 0;
        limits.max = 0;
        hinge.limits = limits;
        hinge.useLimits = true;
        hinge.useSpring = false;
        hinge.useMotor = false;
    }

    void Update()
    {
        if (playerNearby && Input.GetKeyDown(KeyCode.E))
        {
            if (!isOpen)
                OpenGate();
            else
                CloseGate();
        }
    }

    void OpenGate()
    {
        // Remove limits so it can swing freely
        hinge.useLimits = false;
        hinge.useSpring = false;

        // Use a strong motor to DRIVE it open to 90 degrees
        JointMotor motor = hinge.motor;
        motor.force = 10000;          // very strong force
        motor.targetVelocity = 120;   // fast swing speed
        motor.freeSpin = false;
        hinge.motor = motor;
        hinge.useMotor = true;

        // Set limits to stop at 90 degrees
        JointLimits limits = hinge.limits;
        limits.min = -1;
        limits.max = 90;
        hinge.limits = limits;
        hinge.useLimits = true;

        isOpen = true;
    }

    void CloseGate()
    {
        // Reverse the motor to close
        JointMotor motor = hinge.motor;
        motor.force = 10000;
        motor.targetVelocity = -120;  // negative = reverse direction
        motor.freeSpin = false;
        hinge.motor = motor;
        hinge.useMotor = true;

        JointLimits limits = hinge.limits;
        limits.min = 0;
        limits.max = 0;
        hinge.limits = limits;
        hinge.useLimits = true;

        isOpen = false;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerNearby = true;
            Debug.Log("Press E to open gate");
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
            playerNearby = false;
    }
}