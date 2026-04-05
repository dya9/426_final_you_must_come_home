using UnityEngine;

public class energyDrink : MonoBehaviour
{
    // This gets triggered when player walks into the drink's collider
    // The player MUST decide immediately — no pickup to inventory
    public System.Action OnDestroyed;
    private bool playerInRange = false;
    private bool isConsumed = false;
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
            if (!isConsumed)
            {
                DrinkPromptUI.Instance?.HidePrompt();
                // Don't destroy — let it stay so player can come back
                // If you WANT it to disappear on exit, keep the Destroy below
                // Destroy(gameObject);
            }
        }
    }

    public void Consume()
    {
        if (isConsumed) return;
        healthManager?.DrinkEnergy();
        DrinkPromptUI.Instance?.HidePrompt();
        Destroy(gameObject);
    }
     void OnDestroy()           
    {
        OnDestroyed?.Invoke();
    }
}