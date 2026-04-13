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
    public int maxDrinksBeforeHeartAttack = 4;
 
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
 
    private int drinksConsumed = 0;
    private bool isDead = false;
 
    void Start()
    {
        currentHealth = maxHealth;
 
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
            // Make sure it's fully opaque
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
 
        // Also check fillRect is assigned on the Slider itself
        if (healthSlider.fillRect == null)
            Debug.LogError("[HealthManager] Slider.fillRect is NULL! " +
                           "Drag the Fill object into the Slider's Fill Rect field in the Inspector.");
 
        healthSlider.minValue = 0f;
        healthSlider.maxValue = maxHealth;
        healthSlider.value    = maxHealth;
 
        Debug.Log($"[HealthManager] Started — health:{currentHealth} slider:{healthSlider.value}");
        UpdateUI();
    }
 
    void Update()
    {
        if (isDead) return;
 
        currentHealth -= drainRate * Time.deltaTime;
        currentHealth  = Mathf.Clamp(currentHealth, 0, maxHealth);
 
        if (healthSlider != null)
            healthSlider.value = currentHealth;
 
        UpdateFillColor();
 
        if (currentHealth <= 0)
            TriggerMicrosleep();
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
 
        if (drinksConsumed >= maxDrinksBeforeHeartAttack)
        {
            TriggerHeartAttack();
            return true;
        }
 
        currentHealth = Mathf.Clamp(currentHealth + energyDrinkBoost, 0, maxHealth);
 
        if (healthSlider != null)
        {
            healthSlider.value = currentHealth;
            Canvas.ForceUpdateCanvases();
            Debug.Log($"[HealthManager] Slider set to {healthSlider.value} | health:{currentHealth}");
        }
 
        UpdateFillColor();
 
        if (statusText != null)
            statusText.text = $"Drank #{drinksConsumed} - feeling wired!";
 
        return true;
    }
 
    void UpdateFillColor()
    {
        if (fillImage == null) return;
 
        float percent = currentHealth / maxHealth;
 
        if (percent > 0.5f)
            fillImage.color = Color.Lerp(warningColor, healthyColor, (percent - 0.5f) * 2f);
        else
            fillImage.color = Color.Lerp(dangerColor, warningColor, percent * 2f);
 
        if (statusText != null && percent < 0.25f && !isDead && currentStrikes < maxStrikes)
            statusText.text = "WARNING: Micro-sleep incoming...";
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