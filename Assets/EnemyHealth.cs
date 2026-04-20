using System;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHealth : MonoBehaviour
{
    [Header("Health")]
    public float maxHealth = 3f; // Set to 3 for the 3-hit rule
    private float currentHealth;
    public Button nextSceneButton; // Reference to the UI button

    [Header("World Space Health Bar")]
    public Slider healthBarSlider;
    public Image healthBarFill;

    [Header("Damage Popup")]
    public GameObject damagePopupPrefab;
    public float popupHeightOffset = 2.2f;

    [Header("Health Bar Colors")]
    public Color fullHealthColor = Color.green;
    public Color halfHealthColor = Color.yellow;
    public Color lowHealthColor = Color.red;

    private bool isDead = false;

    void Start()
    {
        currentHealth = maxHealth;
        UpdateHealthBar();
        
        // Ensure button is hidden at start
        if (nextSceneButton != null) nextSceneButton.gameObject.SetActive(false);
    }

    public void TakeDamage(float amount)
    {
        if (isDead) return;

        // Force exactly 1 damage per hit to ensure a 3-hit down
        currentHealth -= 1f; 
        currentHealth = Mathf.Clamp(currentHealth, 0f, maxHealth);
        UpdateHealthBar();
        SpawnDamagePopup(1f);

        Debug.Log($"[Enemy] {gameObject.name} HP: {currentHealth}/{maxHealth}");
        if (currentHealth <= 0f) Die();
    }

    void UpdateHealthBar()
    {
        if (healthBarSlider != null)
        {
            healthBarSlider.maxValue = maxHealth;
            healthBarSlider.value = currentHealth;
        }
        if (healthBarFill != null)
        {
            float pct = currentHealth / maxHealth;
            if (pct > 0.5f)
                healthBarFill.color = Color.Lerp(halfHealthColor, fullHealthColor, (pct - 0.5f) * 2f);
            else
                healthBarFill.color = Color.Lerp(lowHealthColor, halfHealthColor, pct * 2f);
        }
    }

    void SpawnDamagePopup(float amount)
    {
        if (damagePopupPrefab == null) return;
        Vector3 spawnPos = transform.position + Vector3.up * popupHeightOffset;
        GameObject popup = Instantiate(damagePopupPrefab, spawnPos, Quaternion.identity);
        DamagePopup dp = popup.GetComponent<DamagePopup>();
        if (dp != null) dp.Setup(amount);
    }

    void Die()
    {
        isDead = true;
        Debug.Log($"[Enemy] {gameObject.name} defeated!");

        // Show button and unlock cursor
        if (nextSceneButton != null) {
            nextSceneButton.gameObject.SetActive(true);
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }

        // Disable movement if EnemyScript is also on this object
        EnemyScript moveScript = GetComponent<EnemyScript>();
        if (moveScript != null) moveScript.enabled = false;

        Destroy(gameObject, 0.5f);
    }
}