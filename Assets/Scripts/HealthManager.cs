using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
 
public class HealthManager : MonoBehaviour
{
    [Header("Health Settings")]
    public float maxHealth = 100f;
    [SerializeField] private float currentHealth;
    public float CurrentHealth => currentHealth;
 
    public float drainRate = 2f;
    public float energyDrinkBoost = 30f;
 
    [Header("Heart Attack Settings")]
    // Player dies if health reaches or exceeds this value (i.e. too many drinks)
    public float heartAttackThreshold = 95f;
 
    [Header("Strike Settings")]
    public int maxStrikes = 4;
    private int currentStrikes = 0;
 
    [Header("UI")]
    public Slider healthSlider;
    public TextMeshProUGUI statusText;
    private Image fillImage;
 
    [Header("Colors")]
    public Color healthyColor = Color.green;
    public Color warningColor = Color.yellow;
    public Color dangerColor = Color.red;
    public Color overloadColor = new Color(1f, 0f, 0.5f); // hot pink — too wired
 
    private int drinksConsumed = 0;
    private bool isDead = false;
 
    void Start()
    {
        // Start at 50% health
        currentHealth = maxHealth * 0.5f;
 
        if (healthSlider == null)
        {
            Debug.LogError("[HealthManager] healthSlider is NULL — assign it in Inspector!");
            return;
        }
 
        // Auto-find the Fill Image inside the Slider
        Transform fillTransform = healthSlider.transform.Find("Fill Area/Fill");
        if (fillTransform != null)
        {
            fillImage = fillTransform.GetComponent<Image>();
            if (fillImage != null)
            {
                Color c = fillImage.color;
                c.a = 1f;
                fillImage.color = c;
                Debug.Log("[HealthManager] Fill Image found. Alpha forced to 1.");
            }
        }
        else
        {
            Debug.LogError("[HealthManager] Could not find 'Fill Area/Fill' inside Slider!");
        }
 
        if (healthSlider.fillRect == null)
            Debug.LogError("[HealthManager] Slider.fillRect is NULL! " +
                           "Drag the Fill object into the Slider's Fill Rect field in the Inspector.");
 
        healthSlider.minValue = 0f;
        healthSlider.maxValue = maxHealth;
        healthSlider.value    = currentHealth;
 
        Debug.Log($"[HealthManager] Started — health:{currentHealth} slider:{healthSlider.value}");
        UpdateUI();
    }
 
    void Update()
    {
        if (isDead) return;
 
        // Health drains over time
        currentHealth -= drainRate * Time.deltaTime;
        currentHealth  = Mathf.Clamp(currentHealth, 0f, maxHealth);
 
        if (healthSlider != null)
            healthSlider.value = currentHealth;
 
        UpdateFillColor();
 
        // Too little energy — microsleep
        if (currentHealth <= 0f)
            TriggerMicrosleep();
 
        // Too much energy — heart attack (in case boost pushed it over mid-frame)
        if (currentHealth >= heartAttackThreshold)
            TriggerHeartAttack();
    }
 
    public void TakeDamage()
    {
        if (isDead) return;
 
        currentStrikes++;
        if (statusText != null)
            statusText.text = $"CAUGHT! Strike {currentStrikes}/{maxStrikes}";
 
        if (currentStrikes >= maxStrikes)
            TriggerCaughtGameOver();
    }
 
    public bool DrinkEnergy()
    {
        if (isDead) return false;
 
        drinksConsumed++;
        Debug.Log($"[HealthManager] DrinkEnergy() — drinks:{drinksConsumed} health before:{currentHealth}");
 
        currentHealth = Mathf.Clamp(currentHealth + energyDrinkBoost, 0f, maxHealth);
 
        if (healthSlider != null)
        {
            healthSlider.value = currentHealth;
            Canvas.ForceUpdateCanvases();
            Debug.Log($"[HealthManager] Slider set to {healthSlider.value} | health:{currentHealth}");
        }
 
        UpdateFillColor();
 
        // Check immediately if the boost pushed health into heart attack range
        if (currentHealth >= heartAttackThreshold)
        {
            TriggerHeartAttack();
            return true;
        }
 
        if (statusText != null)
            statusText.text = $"Drank #{drinksConsumed} — feeling wired!";
 
        return true;
    }
 
    void UpdateFillColor()
    {
        if (fillImage == null) return;
 
        float percent = currentHealth / maxHealth;
 
        // Low  (0–25%)   → red danger
        // Mid  (25–60%)  → yellow warning → green healthy
        // High (75–100%) → green → pink overload (too much caffeine)
        if (percent > 0.75f)
            fillImage.color = Color.Lerp(healthyColor, overloadColor, (percent - 0.75f) * 4f);
        else if (percent > 0.5f)
            fillImage.color = Color.Lerp(warningColor, healthyColor, (percent - 0.5f) * 4f);
        else if (percent > 0.25f)
            fillImage.color = Color.Lerp(dangerColor, warningColor, (percent - 0.25f) * 4f);
        else
            fillImage.color = dangerColor;
 
        // Status text warnings
        if (statusText != null && !isDead && currentStrikes < maxStrikes)
        {
            if (percent <= 0.25f)
                statusText.text = "WARNING: Micro-sleep incoming...";
            else if (percent >= 0.85f)
                statusText.text = "WARNING: Heart racing — too much caffeine!";
        }
    }
 
    void UpdateUI()
    {
        if (healthSlider != null) healthSlider.value = currentHealth;
        UpdateFillColor();
    }
 
    void TriggerMicrosleep()
    {
        isDead = true;
        if (statusText != null) statusText.text = "MICRO-SLEEP! Game Over.";
        Debug.Log("GAME OVER: Micro-sleep");
        LoadDeathScene();
    }
 
    void TriggerHeartAttack()
    {
        isDead = true;
        if (statusText != null) statusText.text = "HEART ATTACK! Too many energy drinks!";
        Debug.Log("GAME OVER: Heart attack");
        LoadDeathScene();
    }
 
    void TriggerCaughtGameOver()
    {
        isDead = true;
        if (statusText != null) statusText.text = "CAUGHT TOO MANY TIMES! Game Over.";
        Debug.Log("GAME OVER: Caught by NPC");
        LoadDeathScene();
    }
 
    void LoadDeathScene()
    {
        SceneManager.LoadScene("Death Screen");
    }
}