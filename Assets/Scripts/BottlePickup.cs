 using UnityEngine;

/// <summary>
/// Place this on each Bottle pickup prefab.
/// Collider on this GameObject MUST have "Is Trigger" checked.
/// Player MUST have the "Player" tag AND a Rigidbody component.
/// </summary>
public class BottlePickup : MonoBehaviour
{
    [Header("Optional spin visual")]
    public float spinSpeed = 90f;

    [HideInInspector]
    public BottleSpawner spawner;

    private bool playerInRange = false;

    void Start()
    {
        // ── Self-check on startup so you catch setup errors immediately ────────
        Collider col = GetComponent<Collider>();
        if (col == null)
            Debug.LogError($"[BottlePickup] {gameObject.name} has NO Collider! Add one and enable Is Trigger.");
        else if (!col.isTrigger)
            Debug.LogError($"[BottlePickup] {gameObject.name}'s Collider is NOT set to Is Trigger! Fix this in the Inspector.");
        else
            Debug.Log($"[BottlePickup] {gameObject.name} ready. Waiting for Player to enter trigger.");
    }

    void Update()
    {
        if (spinSpeed != 0f)
            transform.Rotate(Vector3.up, spinSpeed * Time.deltaTime);

        if (playerInRange && Input.GetKeyDown(KeyCode.E))
        {
            Debug.Log("[BottlePickup] E pressed in range — picking up!");
            BottleInventory.Instance?.SetPickupInRange(false);
            BottleInventory.Instance?.AddBottle();
            spawner?.OnBottlePickedUp(gameObject);
            Destroy(gameObject);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        Debug.Log($"[BottlePickup] OnTriggerEnter hit by: {other.name} (tag: {other.tag})");
        if (!other.CompareTag("Player")) return;

        Debug.Log("[BottlePickup] Player entered range — press E to pick up.");
        playerInRange = true;
        BottleInventory.Instance?.SetPickupInRange(true);
    }

    void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        Debug.Log("[BottlePickup] Player left range.");
        playerInRange = false;
        BottleInventory.Instance?.SetPickupInRange(false);
    }

    void OnDestroy()
    {
        if (playerInRange)
            BottleInventory.Instance?.SetPickupInRange(false);
    }
}