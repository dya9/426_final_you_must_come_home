using System;
using UnityEngine;
using UnityEngine.UI;

public class EnemyScript : MonoBehaviour
{
    [Header("Health")]
    public float maxHealth = 100f;
    private float currentHealth;

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
    }

    public void TakeDamage(float amount)
    {
        if (isDead) return;
        currentHealth -= amount;
        currentHealth = Mathf.Clamp(currentHealth, 0f, maxHealth);
        UpdateHealthBar();
        SpawnDamagePopup(amount);
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
        Destroy(gameObject, 0.5f);
    }
}


//using UnityEngine;
//using UnityEngine.UI;
//using TMPro;

///// <summary>
///// Attach to every Enemy GameObject.
///// Requires a world-space Canvas child with a Slider (health bar).
///// Floating damage numbers are spawned as a separate prefab.
///// </summary>
//public class EnemyHealth : MonoBehaviour
//{
//    [Header("Health")]
//    public float maxHealth = 100f;
//    private float currentHealth;

//    [Header("World Space Health Bar")]
//    [Tooltip("The Slider component on the world-space Canvas above the enemy")]
//    public Slider healthBarSlider;

//    [Tooltip("Fill image of the slider — used for color transitions")]
//    public Image healthBarFill;

//    [Header("Damage Popup")]
//    [Tooltip("Assign the DamagePopup prefab here")]
//    public GameObject damagePopupPrefab;

//    [Tooltip("How high above the enemy the popup spawns")]
//    public float popupHeightOffset = 2.2f;

//    [Header("Colors")]
//    public Color fullHealthColor = Color.green;
//    public Color halfHealthColor = Color.yellow;
//    public Color lowHealthColor = Color.red;

//    private bool isDead = false;

//    // ──────────────────────────────────────────────────────────────────────────
//    void Start()
//    {
//        currentHealth = maxHealth;
//        UpdateHealthBar();
//    }

//    // ── Public API ────────────────────────────────────────────────────────────
//    public void TakeDamage(float amount)
//    {
//        if (isDead) return;

//        currentHealth -= amount;
//        currentHealth = Mathf.Clamp(currentHealth, 0f, maxHealth);

//        UpdateHealthBar();
//        SpawnDamagePopup(amount);

//        Debug.Log($"[Enemy] {gameObject.name} HP: {currentHealth}/{maxHealth}");

//        if (currentHealth <= 0f) Die();
//    }

//    // ── Health bar ────────────────────────────────────────────────────────────
//    void UpdateHealthBar()
//    {
//        if (healthBarSlider != null)
//        {
//            healthBarSlider.maxValue = maxHealth;
//            healthBarSlider.value = currentHealth;
//        }

//        if (healthBarFill != null)
//        {
//            float pct = currentHealth / maxHealth;
//            if (pct > 0.5f)
//                healthBarFill.color = Color.Lerp(halfHealthColor, fullHealthColor, (pct - 0.5f) * 2f);
//            else
//                healthBarFill.color = Color.Lerp(lowHealthColor, halfHealthColor, pct * 2f);
//        }
//    }

//    // ── Floating damage number ────────────────────────────────────────────────
//    void SpawnDamagePopup(float amount)
//    {
//        if (damagePopupPrefab == null) return;

//        Vector3 spawnPos = transform.position + Vector3.up * popupHeightOffset;
//        GameObject popup = Instantiate(damagePopupPrefab, spawnPos, Quaternion.identity);

//        DamagePopup dp = popup.GetComponent<DamagePopup>();
//        if (dp != null) dp.Setup(amount);
//    }

//    // ── Death ─────────────────────────────────────────────────────────────────
//    void Die()
//    {
//        isDead = true;
//        Debug.Log($"[Enemy] {gameObject.name} defeated!");
//        Destroy(gameObject, 0.5f);
//    }
//}