using UnityEngine;
 
public class energyDrink : MonoBehaviour
{
    public System.Action OnDestroyed;
    public AudioClip drinkSound;        // drag your mp3 here in the Inspector
 
    private AudioSource audioSource;
    private HealthManager healthManager;
    private bool choiceMade = false;
    private bool playerInRange = false;
 
    // ── Init ──────────────────────────────────────────────────────────────────
 
    void Start()
    {
        healthManager = FindObjectOfType<HealthManager>();
        audioSource   = gameObject.AddComponent<AudioSource>();
 
        if (healthManager == null)
            Debug.LogError("[energyDrink] No HealthManager found in scene!");
    }
 
    // ── Input ─────────────────────────────────────────────────────────────────
 
    void Update()
    {
        if (playerInRange && !choiceMade && Input.GetKeyDown(KeyCode.X))
        {
            Consume();
        }
    }
 
    // ── Trigger ───────────────────────────────────────────────────────────────
 
    void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;
 
        playerInRange = true;
 
        // Re-fetch in case Start() missed it
        if (healthManager == null)
            healthManager = FindObjectOfType<HealthManager>();
 
        DrinkPromptUI.Instance?.ShowPrompt(this);
    }
 
    void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player")) return;
 
        playerInRange = false;
 
        if (!choiceMade)
            DrinkPromptUI.Instance?.HidePrompt();
    }
 
    // ── Player Choices ────────────────────────────────────────────────────────
 
    // Called when player clicks "Drink" or presses X
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