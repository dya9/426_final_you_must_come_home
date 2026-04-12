using UnityEngine;
using TMPro;

public class BottleInventory : MonoBehaviour
{
    public static BottleInventory Instance { get; private set; }

    [Header("UI")]
    public TextMeshProUGUI bottleCountText;

    [Header("Throwing")]
    public GameObject bottleProjectilePrefab;
    public Transform throwOrigin;
    public float throwForce = 20f;

    private int bottleCount = 0;
    private bool pickupInRange = false;

    void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
    }

    void Start() => UpdateUI();

    void Update()
    {
        // FIXED: only E throws, and only when no pickup is in range
        if (Input.GetKeyDown(KeyCode.E) && !pickupInRange)
            TryThrow();
    }

    public void SetPickupInRange(bool inRange) => pickupInRange = inRange;

    public void AddBottle()
    {
        bottleCount++;
        UpdateUI();
        Debug.Log($"[Bottle] Picked up! Total: {bottleCount}");
    }

    public bool HasBottle() => bottleCount > 0;

    void TryThrow()
    {
        if (bottleCount <= 0)
        {
            Debug.Log("[Bottle] No bottles to throw.");
            return;
        }

        if (bottleProjectilePrefab == null || throwOrigin == null)
        {
            Debug.LogWarning("[Bottle] Missing prefab or throw origin!");
            return;
        }

        bottleCount--;
        UpdateUI();

        GameObject proj = Instantiate(bottleProjectilePrefab, throwOrigin.position, throwOrigin.rotation);

        Rigidbody rb = proj.GetComponent<Rigidbody>();
        if (rb != null)
            rb.linearVelocity = GetAimDirection() * throwForce;

        // Safety destroy in case it never hits anything
        Destroy(proj, 8f);
    }

    Vector3 GetAimDirection()
    {
        Camera cam = Camera.main;
        if (cam == null) return throwOrigin.forward;

        Ray ray = cam.ScreenPointToRay(new Vector3(Screen.width / 2f, Screen.height / 2f, 0f));
        if (Physics.Raycast(ray, out RaycastHit hit, 500f))
            return (hit.point - throwOrigin.position).normalized;

        return ray.direction;
    }

    void UpdateUI()
    {
        if (bottleCountText != null)
            bottleCountText.text = $"Bottles: {bottleCount}";
    }
}