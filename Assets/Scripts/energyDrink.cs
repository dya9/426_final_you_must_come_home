using UnityEngine;
 
public class energyDrink : MonoBehaviour
{
    public System.Action OnDestroyed;
    public AudioClip drinkSound;        // drag your mp3 here in the Inspector
 
    private AudioSource audioSource;
    private HealthManager healthManager;
    private bool choiceMade = false;
 
    // ── Arrow Pointer Lifecycle ───────────────────────────────────────────────
 
    void OnEnable()
    {
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
 
    // ── Init ──────────────────────────────────────────────────────────────────
 
    void Start()
    {
        healthManager = FindObjectOfType<HealthManager>();
        audioSource   = gameObject.AddComponent<AudioSource>();
 
        if (healthManager == null)
            Debug.LogError("[energyDrink] No HealthManager found in scene!");
    }
 
    // ── Trigger ───────────────────────────────────────────────────────────────
 
    void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;
 
        // Re-fetch in case Start() missed it
        if (healthManager == null)
            healthManager = FindObjectOfType<HealthManager>();
 
        DrinkPromptUI.Instance?.ShowPrompt(this);
    }
 
    void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player")) return;
 
        if (!choiceMade)
            DrinkPromptUI.Instance?.HidePrompt();
    }
 
    // ── Player Choices ────────────────────────────────────────────────────────
 
    // Called when player clicks "Drink"
    public void Consume()
    {
        if (choiceMade) return;
        choiceMade = true;
 
        if (healthManager != null)
        {
            bool success = healthManager.DrinkEnergy();
            Debug.Log("[energyDrink] DrinkEnergy() called. Success: " + success);
        }
        else
        {
            Debug.LogError("[energyDrink] Consume() failed — HealthManager is null!");
        }
 
        // Play sound BEFORE destroying so it isn't cut off
        if (drinkSound != null)
            AudioSource.PlayClipAtPoint(drinkSound, transform.position);
 
        DrinkPromptUI.Instance?.HidePrompt();
        Destroy(gameObject);
    }
 
    // Called when player clicks "Leave"
    public void Leave()
    {
        if (choiceMade) return;
        choiceMade = true;
 
        DrinkPromptUI.Instance?.HidePrompt();
        Destroy(gameObject);
    }
}