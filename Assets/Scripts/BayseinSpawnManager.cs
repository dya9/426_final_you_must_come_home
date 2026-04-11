using UnityEngine;
using System.Collections.Generic;

public class BayesianSpawnManager : MonoBehaviour
{
    [Header("References")]
    public Transform player;
    public GameObject energyDrinkPrefab;
    public Transform[] spawnPoints;
    public HealthManager healthManager;

    [Header("Spawn Settings")]
    public float checkInterval = 3f;      // How often we run the Bayesian check
    public int maxDrinksOnScreen = 2;
    public float spawnProbabilityThreshold = 0.6f; // Minimum probability to trigger spawn

    [Header("Bayesian Weights (must add up to 1)")]
    [Range(0f, 1f)] public float healthWeight = 0.5f;       // Health is most important
    [Range(0f, 1f)] public float distanceWeight = 0.3f;     // Distance second
    [Range(0f, 1f)] public float timeWeight = 0.2f;         // Time since last drink third

    [Header("Debug")]
    public bool showDebugLogs = true;

    private float checkTimer = 0f;
    private float timeSinceLastDrink = 0f;
    private int currentDrinksOnScreen = 0;
    private List<GameObject> activeDrinks = new List<GameObject>();

    // Bayesian probability thresholds
    private const float LOW_HEALTH_THRESHOLD = 0.35f;    // Below 35% = high probability
    private const float FAR_DISTANCE_THRESHOLD = 20f;    // Beyond 20 units = high probability
    private const float LONG_TIME_THRESHOLD = 15f;       // 15 seconds without drink = high probability


    void Update()
    {
        timeSinceLastDrink += Time.deltaTime;
        checkTimer += Time.deltaTime;

        // Clean up destroyed drinks from our list
        activeDrinks.RemoveAll(d => d == null);
        currentDrinksOnScreen = activeDrinks.Count;

        if (checkTimer >= checkInterval)
        {
            checkTimer = 0f;
            RunBayesianCheck();
        }
    }

    void RunBayesianCheck()
    {
        if (currentDrinksOnScreen >= maxDrinksOnScreen) return;

        // ── Factor 1: Health Probability ──────────────────────────────
        // Low health → high probability
        float healthPercent = healthManager.currentHealth / healthManager.maxHealth;
        float healthProbability = CalculateHealthProbability(healthPercent);

        // ── Factor 2: Distance Probability ───────────────────────────
        // No drinks nearby → high probability
        float nearestDrinkDistance = GetNearestDrinkDistance();
        float distanceProbability = CalculateDistanceProbability(nearestDrinkDistance);

        // ── Factor 3: Time Probability ────────────────────────────────
        // Long time without a drink → high probability
        float timeProbability = CalculateTimeProbability(timeSinceLastDrink);

        // ── Bayesian Combined Probability ─────────────────────────────
        // Weighted combination of all 3 factors
        float combinedProbability = (healthProbability   * healthWeight) +
                                    (distanceProbability * distanceWeight) +
                                    (timeProbability     * timeWeight);

        if (showDebugLogs)
        {
            Debug.Log($"[Bayesian] Health: {healthProbability:F2} | " +
                      $"Distance: {distanceProbability:F2} | " +
                      $"Time: {timeProbability:F2} | " +
                      $"Combined: {combinedProbability:F2}");
        }

        // ── Spawn Decision ────────────────────────────────────────────
        if (combinedProbability >= spawnProbabilityThreshold)
        {
            Transform bestSpawnPoint = GetBestSpawnPoint();
            if (bestSpawnPoint != null)
                SpawnDrink(bestSpawnPoint);
        }
    }

    // ── Probability Calculators ───────────────────────────────────────

    float CalculateHealthProbability(float healthPercent)
    {
        // Below threshold: high probability, scales up as health drops
        if (healthPercent <= LOW_HEALTH_THRESHOLD)
            return Mathf.Lerp(1f, 0.7f, healthPercent / LOW_HEALTH_THRESHOLD);
        else
            // Above threshold: low probability, scales down as health rises
            return Mathf.Lerp(0.7f, 0.1f, (healthPercent - LOW_HEALTH_THRESHOLD) 
                                           / (1f - LOW_HEALTH_THRESHOLD));
    }

    float CalculateDistanceProbability(float distance)
    {
        if (currentDrinksOnScreen == 0)
            return 1f; // No drinks at all — always high probability

        // Far from any drink = high probability
        return Mathf.Clamp01(distance / FAR_DISTANCE_THRESHOLD);
    }

    float CalculateTimeProbability(float time)
    {
        // More time passed = higher probability, caps at 1.0
        return Mathf.Clamp01(time / LONG_TIME_THRESHOLD);
    }

    // ── Spawn Point Selection ─────────────────────────────────────────
    // Picks the spawn point that is:
    // - Not too close to the player (not unfair)
    // - Not too far from the player (still reachable)

    Transform GetBestSpawnPoint()
    {
        Transform best = null;
        float bestScore = -1f;

        foreach (Transform point in spawnPoints)
        {
            // Skip if a drink already exists very close to this point
            if (DrinkAlreadyNear(point.position, 3f)) continue;

            float distToPlayer = Vector3.Distance(point.position, player.position);

            // Score: prefer points that are reachable but not on top of player
            // Sweet spot: between 8 and 25 units away
            float score = 0f;
            if (distToPlayer >= 8f && distToPlayer <= 25f)
                score = 1f - Mathf.Abs(distToPlayer - 15f) / 15f; // Peak at 15 units
            else if (distToPlayer < 8f)
                score = 0.1f; // Too close, low score
            else
                score = 0.2f; // Too far, low score

            if (score > bestScore)
            {
                bestScore = score;
                best = point;
            }
        }

        return best;
    }

    // ── Helpers ───────────────────────────────────────────────────────

    float GetNearestDrinkDistance()
    {
        if (activeDrinks.Count == 0) return float.MaxValue;

        float nearest = float.MaxValue;
        foreach (GameObject drink in activeDrinks)
        {
            if (drink == null) continue;
            float dist = Vector3.Distance(player.position, drink.transform.position);
            if (dist < nearest) nearest = dist;
        }
        return nearest;
    }

    bool DrinkAlreadyNear(Vector3 position, float radius)
    {
        foreach (GameObject drink in activeDrinks)
        {
            if (drink == null) continue;
            if (Vector3.Distance(drink.transform.position, position) < radius)
                return true;
        }
        return false;
    }

    void SpawnDrink(Transform spawnPoint)
    {
        GameObject drink = Instantiate(energyDrinkPrefab, 
                                       spawnPoint.position, 
                                       Quaternion.identity);
        activeDrinks.Add(drink);
        currentDrinksOnScreen++;
        timeSinceLastDrink = 0f; // Reset timer

        // Hook into the drink's destroy event
        energyDrink drinkScript = drink.GetComponent<energyDrink>();
        if (drinkScript != null)
        {
            drinkScript.OnDestroyed += () => {
                activeDrinks.Remove(drink);
                currentDrinksOnScreen--;
                timeSinceLastDrink = 0f;
            };
        }

        if (showDebugLogs)
            Debug.Log($"[Bayesian] Spawned drink at {spawnPoint.name}");
    }

    // ── Public method so energyDrink.cs can notify consumption ────────
    public void NotifyDrinkConsumed()
    {
        timeSinceLastDrink = 0f;
    }
}