using UnityEngine;

public class DrinkSpawn : MonoBehaviour
{
    public GameObject energyDrinkPrefab;
    public Transform[] spawnPoints;      // Assign positions in Inspector
    public float minSpawnTime = 8f;
    public float maxSpawnTime = 20f;
    public int maxDrinksOnScreen = 2;    // Keeps it fair

    private int currentDrinks = 0;

    void Start()
    {
        ScheduleNextSpawn();
    }

    void ScheduleNextSpawn()
    {
        if (this == null || !gameObject.activeInHierarchy) return; 
        float delay = Random.Range(minSpawnTime, maxSpawnTime);
        Invoke(nameof(SpawnDrink), delay);
    }

    void SpawnDrink()
    {
        if (currentDrinks >= maxDrinksOnScreen)
        {
            ScheduleNextSpawn();
            return;
        }

        Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];
        GameObject drink = Instantiate(energyDrinkPrefab, spawnPoint.position, Quaternion.identity);
        
        // Track count
        currentDrinks++;
        drink.GetComponent<energyDrink>().OnDestroyed += () => {
            currentDrinks--;
            if (this != null)
            ScheduleNextSpawn();
        };
    }

   
}