// using UnityEngine;
// using UnityEngine.UI;
// using TMPro;

// public class HealthManager : MonoBehaviour
// {
//     [Header("Health Settings")]
//     public float maxHealth = 100f;
//     public float currentHealth;
//     public float drainRate = 2f;          // Health lost per second
//     public float energyDrinkBoost = 30f;  // Health gained per drink
//     public int maxDrinksBeforeHeartAttack = 4; // Heart attack threshold

//     [Header("UI")]
//     public Slider healthSlider;
//     public Image fillImage;
//     public TextMeshProUGUI statusText;

//     [Header("Colors")]
//     public Color healthyColor = Color.green;
//     public Color warningColor = Color.yellow;
//     public Color dangerColor = Color.red;

//     private int drinksConsumed = 0;
//     private bool isDead = false;

//     void Start()
//     {
//         currentHealth = maxHealth;
//         healthSlider.maxValue = maxHealth;
//     }

//     void Update()
//     {
//         if (isDead) return;

//         // Continuously drain health
//         currentHealth -= drainRate * Time.deltaTime;
//         currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

//         UpdateUI();

//         if (currentHealth <= 0)
//             TriggerMicrosleep();
//     }

//     public bool DrinkEnergy()
//     {
//         if (isDead) return false;

//         drinksConsumed++;

//         if (drinksConsumed >= maxDrinksBeforeHeartAttack)
//         {
//             TriggerHeartAttack();
//             return true;
//         }

//         currentHealth = Mathf.Clamp(currentHealth + energyDrinkBoost, 0, maxHealth);
//         statusText.text = $"Drank #{drinksConsumed} — feeling wired!";
//         return true;
//     }

//     void UpdateUI()
//     {
//         healthSlider.value = currentHealth;
//         float percent = currentHealth / maxHealth;

//         // Color transitions: green → yellow → red
//         if (percent > 0.5f)
//             fillImage.color = Color.Lerp(warningColor, healthyColor, (percent - 0.5f) * 2f);
//         else
//             fillImage.color = Color.Lerp(dangerColor, warningColor, percent * 2f);

//         if (percent < 0.25f && !isDead)
//             statusText.text = "⚠️ Micro-sleep incoming...";
//     }

//     void TriggerMicrosleep()
//     {
//         isDead = true;
//         statusText.text = "😴 MICRO-SLEEP! Game Over.";
//         // Add your Game Over logic here (freeze player, show screen, etc.)
//         Debug.Log("GAME OVER: Micro-sleep");
//     }

//     void TriggerHeartAttack()
//     {
//         isDead = true;
//         statusText.text = "💀 HEART ATTACK! Too many energy drinks!";
//         Debug.Log("GAME OVER: Heart attack");
//     }
// }
// using UnityEngine;
// using UnityEngine.UI;
// using TMPro;

// public class HealthManager : MonoBehaviour
// {
//     [Header("Health Settings")]
//     public float maxHealth = 100f;
//     public float currentHealth;
//     public float drainRate = 2f;
//     public float energyDrinkBoost = 30f;
//     public int maxDrinksBeforeHeartAttack = 4;

//     [Header("Strike Settings")]
//     public int maxStrikes = 4;
//     private int currentStrikes = 0;

//     [Header("UI")]
//     public Slider healthSlider;
//     public Image fillImage;
//     public TextMeshProUGUI statusText;

//     [Header("Colors")]
//     public Color healthyColor = Color.green;
//     public Color warningColor = Color.yellow;
//     public Color dangerColor = Color.red;

//     private int drinksConsumed = 0;
//     private bool isDead = false;

//     void Start()
//     {
//         currentHealth = maxHealth;
//         healthSlider.maxValue = maxHealth;
//     }

//     void Update()
//     {
//         if (isDead) return;

//         currentHealth -= drainRate * Time.deltaTime;
//         currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

//         UpdateUI();

//         if (currentHealth <= 0)
//             TriggerMicrosleep();
//     }

//     // --- NEW METHOD FOR NPC ATTACKS ---
//     public void TakeDamage()
//     {
//         if (isDead) return;

//         currentStrikes++;
//         statusText.text = $"CAUGHT! Strike {currentStrikes}/{maxStrikes}";

//         if (currentStrikes >= maxStrikes)
//         {
//             TriggerCaughtGameOver();
//         }
//     }

//     public bool DrinkEnergy()
//     {
//         if (isDead) return false;
//         drinksConsumed++;

//         if (drinksConsumed >= maxDrinksBeforeHeartAttack)
//         {
//             TriggerHeartAttack();
//             return true;
//         }

//         currentHealth = Mathf.Clamp(currentHealth + energyDrinkBoost, 0, maxHealth);
//         statusText.text = $"Drank #{drinksConsumed} — feeling wired!";
//         return true;
//     }

//     void UpdateUI()
//     {
//         healthSlider.value = currentHealth;
//         float percent = currentHealth / maxHealth;

//         if (percent > 0.5f)
//             fillImage.color = Color.Lerp(warningColor, healthyColor, (percent - 0.5f) * 2f);
//         else
//             fillImage.color = Color.Lerp(dangerColor, warningColor, percent * 2f);

//         if (percent < 0.25f && !isDead && currentStrikes < maxStrikes)
//             statusText.text = "⚠️ Micro-sleep incoming...";
//     }

//     // --- GAME OVER CONDITIONS ---

//     void TriggerMicrosleep()
//     {
//         isDead = true;
//         statusText.text = "😴 MICRO-SLEEP! Game Over.";
//         Debug.Log("GAME OVER: Micro-sleep");
//     }

//     void TriggerHeartAttack()
//     {
//         isDead = true;
//         statusText.text = "💀 HEART ATTACK! Too many energy drinks!";
//         Debug.Log("GAME OVER: Heart attack");
//     }

//     void TriggerCaughtGameOver()
//     {
//         isDead = true;
//         statusText.text = "🚫 CAUGHT TOO MANY TIMES! Game Over.";
//         Debug.Log("GAME OVER: Caught by NPC");
//     }
// }

using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
 
public class HealthManager : MonoBehaviour
{
    [Header("Health Settings")]
    public float maxHealth = 100f;
    public float currentHealth;
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
        statusText.text = $"CAUGHT! Strike {currentStrikes}/{maxStrikes}";

        if (currentStrikes >= maxStrikes)
        {
            TriggerCaughtGameOver();
        }
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

        if (percent < 0.25f && !isDead && currentStrikes < maxStrikes)
            statusText.text = "⚠️ Micro-sleep incoming...";
    }

    // --- GAME OVER CONDITIONS ---

    void TriggerMicrosleep()
    {
        isDead = true;
        statusText.text = "😴 MICRO-SLEEP! Game Over.";
        Debug.Log("GAME OVER: Micro-sleep");
        LoadDeathScene(); // Switches scenes
    }
 
    void TriggerHeartAttack()
    {
        isDead = true;
        if (statusText != null) statusText.text = "HEART ATTACK! Too many energy drinks!";
        Debug.Log("GAME OVER: Heart attack");
        LoadDeathScene(); // Switches scenes
    }

    void TriggerCaughtGameOver()
    {
        isDead = true;
        statusText.text = "🚫 CAUGHT TOO MANY TIMES! Game Over.";
        Debug.Log("GAME OVER: Caught by NPC");
        LoadDeathScene(); // Switches scenes
    }

    // Helper method to actually switch the scene
    void LoadDeathScene()
    {
        // Make sure "Death Scene" matches the name in your Build Settings exactly!
        SceneManager.LoadScene("Death Screen");
    }
}