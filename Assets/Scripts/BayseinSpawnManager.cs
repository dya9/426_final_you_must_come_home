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
    public float checkInterval = 3f;
    public int maxDrinksOnScreen = 2;
    public float spawnProbabilityThreshold = 0.6f;
 
    [Header("Bayesian Weights (must add up to 1)")]
    [Range(0f, 1f)] public float healthWeight = 0.5f;
    [Range(0f, 1f)] public float distanceWeight = 0.3f;
    [Range(0f, 1f)] public float timeWeight = 0.2f;
 
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
 
        // FIX: use CurrentHealth (capital C) — the public read-only property
        float healthPercent = healthManager.CurrentHealth / healthManager.maxHealth;
        float healthProbability = CalculateHealthProbability(healthPercent);
 
        float nearestDrinkDistance = GetNearestDrinkDistance();
        float distanceProbability = CalculateDistanceProbability(nearestDrinkDistance);
 
        float timeProbability = CalculateTimeProbability(timeSinceLastDrink);
 
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
 
        if (combinedProbability >= spawnProbabilityThreshold)
        {
            Transform bestSpawnPoint = GetBestSpawnPoint();
            if (bestSpawnPoint != null)
                SpawnDrink(bestSpawnPoint);
        }
    }
 
    float CalculateHealthProbability(float healthPercent)
    {
        if (healthPercent <= LOW_HEALTH_THRESHOLD)
            return Mathf.Lerp(1f, 0.7f, healthPercent / LOW_HEALTH_THRESHOLD);
        else
            return Mathf.Lerp(0.7f, 0.1f, (healthPercent - LOW_HEALTH_THRESHOLD)
                                           / (1f - LOW_HEALTH_THRESHOLD));
    }
 
    float CalculateDistanceProbability(float distance)
    {
        if (currentDrinksOnScreen == 0)
            return 1f;
 
        return Mathf.Clamp01(distance / FAR_DISTANCE_THRESHOLD);
    }
 
    float CalculateTimeProbability(float time)
    {
        return Mathf.Clamp01(time / LONG_TIME_THRESHOLD);
    }
 
    Transform GetBestSpawnPoint()
    {
        Transform best = null;
        float bestScore = -1f;
 
        foreach (Transform point in spawnPoints)
        {
            if (DrinkAlreadyNear(point.position, 3f)) continue;
 
            float distToPlayer = Vector3.Distance(point.position, player.position);
 
            float score = 0f;
            if (distToPlayer >= 8f && distToPlayer <= 25f)
                score = 1f - Mathf.Abs(distToPlayer - 15f) / 15f;
            else if (distToPlayer < 8f)
                score = 0.1f;
            else
                score = 0.2f;
 
            if (score > bestScore)
            {
                bestScore = score;
                best = point;
            }
        }
 
        return best;
    }
 
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
        timeSinceLastDrink = 0f;
 
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
 
    public void NotifyDrinkConsumed()
    {
        timeSinceLastDrink = 0f;
    }
}