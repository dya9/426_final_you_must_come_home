using UnityEngine;

public class energyDrink : MonoBehaviour
{
    public System.Action OnDestroyed;

    private bool playerInRange = false;
    private bool isConsumed = false;
    private HealthManager healthManager;

    void OnEnable()
    {
        // prevent duplicates
        if (!ArrowPointer.ActiveDrinks.Contains(this))
            ArrowPointer.ActiveDrinks.Add(this);
    }

    void OnDisable()
    {
        ArrowPointer.ActiveDrinks.Remove(this);
    }

    void OnDestroy()
    {
        ArrowPointer.ActiveDrinks.Remove(this);
        OnDestroyed?.Invoke();
    }

    void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        playerInRange = true;

        if (healthManager == null)
            healthManager = FindObjectOfType<HealthManager>();

        DrinkPromptUI.Instance?.ShowPrompt(this);
    }

    void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        playerInRange = false;

        if (!isConsumed)
        {
            DrinkPromptUI.Instance?.HidePrompt();
        }
    }

    public void Consume()
    {
        if (isConsumed) return;

        isConsumed = true;

        healthManager?.DrinkEnergy();
        DrinkPromptUI.Instance?.HidePrompt();

        Destroy(gameObject);
    }
}