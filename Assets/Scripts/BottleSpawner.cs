using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// Place this on any GameObject in the scene (e.g. GameManager).
/// Spawns bottle pickup prefabs at random NavMesh-valid positions within
/// a defined radius of this object's position (treat it as the map centre).
/// Automatically respawns to always maintain the target count.
/// </summary>
public class BottleSpawner : MonoBehaviour
{
    [Header("Prefab")]
    [Tooltip("Assign your Bottle Pickup prefab here")]
    public GameObject bottlePickupPrefab;

    [Header("Spawn Settings")]
    [Tooltip("How many bottles should exist in the world at all times")]
    public int targetBottleCount = 5;

    [Tooltip("Radius around this GameObject's position to spawn bottles")]
    public float spawnRadius = 30f;

    [Tooltip("How high above the NavMesh surface to place the bottle")]
    public float spawnHeightOffset = 0.5f;

    [Header("Respawn Delay")]
    [Tooltip("Seconds to wait before spawning a replacement after one is picked up")]
    public float respawnDelay = 2f;

    [Header("NavMesh Sampling")]
    [Tooltip("Max distance from random point to find a valid NavMesh position")]
    public float navMeshSampleDistance = 3f;

    [Tooltip("Max attempts to find a valid NavMesh point before giving up on that spawn")]
    public int maxSampleAttempts = 20;

    // ── Private state ─────────────────────────────────────────────────────────
    private List<GameObject> activeBottles = new List<GameObject>();

    // ──────────────────────────────────────────────────────────────────────────
    void Start()
    {
        StartCoroutine(InitialSpawn());
    }

    IEnumerator InitialSpawn()
    {
        yield return new WaitForSeconds(0.2f);
        for (int i = 0; i < targetBottleCount; i++)
            SpawnOneBottle();
    }

    public void OnBottlePickedUp(GameObject bottle)
    {
        activeBottles.Remove(bottle);
        StartCoroutine(RespawnAfterDelay());
    }

    IEnumerator RespawnAfterDelay()
    {
        yield return new WaitForSeconds(respawnDelay);
        SpawnOneBottle();
    }

    void SpawnOneBottle()
    {
        if (bottlePickupPrefab == null)
        {
            Debug.LogWarning("[BottleSpawner] No bottlePickupPrefab assigned!");
            return;
        }

        Vector3 spawnPos;
        if (!TryGetRandomNavMeshPoint(out spawnPos))
        {
            Debug.LogWarning("[BottleSpawner] Could not find a valid NavMesh point after max attempts.");
            return;
        }

        spawnPos.y += spawnHeightOffset;

        // FIX 1: Explicitly use UnityEngine.Random to avoid System.Random ambiguity
        GameObject bottle = Instantiate(
            bottlePickupPrefab,
            spawnPos,
            Quaternion.Euler(0f, UnityEngine.Random.Range(0f, 360f), 0f));

        BottlePickup pickup = bottle.GetComponent<BottlePickup>();
        if (pickup != null)
            pickup.spawner = this;

        activeBottles.Add(bottle);
    }

    bool TryGetRandomNavMeshPoint(out Vector3 result)
    {
        Vector3 centre = transform.position;

        for (int i = 0; i < maxSampleAttempts; i++)
        {
            // FIX 1: Explicitly use UnityEngine.Random here too
            Vector2 randomCircle = UnityEngine.Random.insideUnitCircle * spawnRadius;
            Vector3 candidate = new Vector3(
                centre.x + randomCircle.x,
                centre.y,
                centre.z + randomCircle.y);

            NavMeshHit hit;
            if (NavMesh.SamplePosition(candidate, out hit, navMeshSampleDistance, NavMesh.AllAreas))
            {
                result = hit.position;
                return true;
            }
        }

        result = Vector3.zero;
        return false;
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(0f, 1f, 0.5f, 0.3f);
        Gizmos.DrawSphere(transform.position, spawnRadius);
        Gizmos.color = new Color(0f, 1f, 0.5f, 1f);
        Gizmos.DrawWireSphere(transform.position, spawnRadius);
    }
}