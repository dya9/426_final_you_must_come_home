using UnityEngine;

public class energyDrink : MonoBehaviour
{
    // This gets triggered when player walks into the drink's collider
    // The player MUST decide immediately — no pickup to inventory
    public System.Action OnDestroyed;
    private bool playerInRange = false;
    private HealthManager healthManager;

    void Update()
    {
        if (playerInRange)
        {
            // Show prompt (handled in UI)
            // Auto-trigger or wait for input — see below
        }
    }

    void OnTriggerEnter(Collider other)  // Use OnTriggerEnter2D for 2D
    {
        Debug.Log("Trigger hit by: " + other.name);
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
            healthManager = FindObjectOfType<HealthManager>();
            // Show "Drink or Leave?" prompt immediately
            DrinkPromptUI.Instance?.ShowPrompt(this);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            // If player walked away = they chose to LEAVE it
            DrinkPromptUI.Instance?.HidePrompt();
            Destroy(gameObject); // Drink is gone — no coming back
        }
    }

    public void Consume()
    {
        healthManager?.DrinkEnergy();
        DrinkPromptUI.Instance?.HidePrompt();
        Destroy(gameObject);
    }
     void OnDestroy()           
    {
        OnDestroyed?.Invoke();
    }
}