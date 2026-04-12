using UnityEngine;

/// <summary>
/// Attach this to the world-space Canvas GameObject above the enemy.
/// Makes the health bar always face the camera (billboard effect).
/// </summary>
public class HealthBarBillboard : MonoBehaviour
{
    private Camera mainCam;

    void Start()
    {
        mainCam = Camera.main;
    }

    void LateUpdate()
    {
        if (mainCam == null) mainCam = Camera.main;
        if (mainCam != null)
            transform.rotation = mainCam.transform.rotation;
    }
}