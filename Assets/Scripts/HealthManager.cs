using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HealthManager : MonoBehaviour
{
    [Header("Health Settings")]
    public float maxHealth = 100f;
    public float currentHealth;
    public float drainRate = 2f;          // Health lost per second
    public float energyDrinkBoost = 30f;  // Health gained per drink
    public int maxDrinksBeforeHeartAttack = 4; // Heart attack threshold

    [Header("UI")]
    public Slider healthSlider;
    public Image fillImage;
    public TextMeshProUGUI statusText;

    [Header("Colors")]
    public Color healthyColor = Color.green;
    public Color warningColor = Color.yellow;
    public Color dangerColor = Color.red;

    private int drinksConsumed = 0;
    private bool isDead = false;

    void Start()
    {
        currentHealth = maxHealth;
        healthSlider.maxValue = maxHealth;
    }

    void Update()
    {
        if (isDead) return;

        // Continuously drain health
        currentHealth -= drainRate * Time.deltaTime;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        UpdateUI();

        if (currentHealth <= 0)
            TriggerMicrosleep();
    }

    public bool DrinkEnergy()
    {
        if (isDead) return false;

        drinksConsumed++;

        if (drinksConsumed >= maxDrinksBeforeHeartAttack)
        {
            TriggerHeartAttack();
            return true;
        }

        currentHealth = Mathf.Clamp(currentHealth + energyDrinkBoost, 0, maxHealth);
        statusText.text = $"Drank #{drinksConsumed} — feeling wired!";
        return true;
    }

    void UpdateUI()
    {
        healthSlider.value = currentHealth;
        float percent = currentHealth / maxHealth;

        // Color transitions: green → yellow → red
        if (percent > 0.5f)
            fillImage.color = Color.Lerp(warningColor, healthyColor, (percent - 0.5f) * 2f);
        else
            fillImage.color = Color.Lerp(dangerColor, warningColor, percent * 2f);

        if (percent < 0.25f && !isDead)
            statusText.text = "⚠️ Micro-sleep incoming...";
    }

    void TriggerMicrosleep()
    {
        isDead = true;
        statusText.text = "😴 MICRO-SLEEP! Game Over.";
        // Add your Game Over logic here (freeze player, show screen, etc.)
        Debug.Log("GAME OVER: Micro-sleep");
    }

    void TriggerHeartAttack()
    {
        isDead = true;
        statusText.text = "💀 HEART ATTACK! Too many energy drinks!";
        Debug.Log("GAME OVER: Heart attack");
    }
}