using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class BottleProjectile : MonoBehaviour
{
    [Header("Damage")]
    public float damage = 25f;

    [Header("Flight Visual")]
    public float spinSpeed = 400f;

    [Header("Impact")]
    public GameObject impactVFXPrefab;

    private Rigidbody rb;
    private bool hasHit = false;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.useGravity = true;
        rb.isKinematic = false;

        // FIXED: ignore collisions with the Player layer so it
        // doesn't immediately collide with the thrower and vanish
        GameObject player = GameObject.FindWithTag("Player");
        if (player != null)
        {
            Collider[] playerCols = player.GetComponentsInChildren<Collider>();
            Collider[] myCols = GetComponentsInChildren<Collider>();
            foreach (Collider pc in playerCols)
                foreach (Collider mc in myCols)
                    Physics.IgnoreCollision(mc, pc);
        }

        Debug.Log($"[Projectile] Spawned at {transform.position}");
    }

    void Update()
    {
        if (!hasHit)
            transform.Rotate(Vector3.right, spinSpeed * Time.deltaTime, Space.Self);
    }

    void OnCollisionEnter(Collision collision)
    {
        if (hasHit) return;
        hasHit = true;

        EnemyHealth enemy = collision.gameObject.GetComponent<EnemyHealth>();
        if (enemy != null)
        {
            enemy.TakeDamage(damage);
            Debug.Log($"[Projectile] Hit {collision.gameObject.name} for {damage} damage.");
        }

        if (impactVFXPrefab != null)
        {
            ContactPoint contact = collision.contacts[0];
            Instantiate(impactVFXPrefab, contact.point, Quaternion.LookRotation(contact.normal));
        }

        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        Destroy(gameObject, 0.1f);
    }
}