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
    [Header("Detection Settings")]
    public Transform player;
    public float chaseRange = 10f;
    public float attackRange = 2f;
    
    [Header("Patrol Settings")]
    public Transform[] patrolPoints;
    public float waitTimeAtPoint = 3f; // How long he sways while "drunk"

    private UnityEngine.AI.NavMeshAgent agent;
    private Animator anim;
    private int currentPointIndex;
    private float waitCounter;
    private bool isWaiting = false;

    // Attack Cooldown to prevent losing all 4 strikes in 1 second
    private float attackCooldown = 1.5f;
    private float lastAttackTime;
    void Start()
    {
         agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        anim = GetComponent<Animator>();
        
        if (patrolPoints.Length > 0) {
            GoToNextPoint();
        }
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

     void Update() {
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        if (distanceToPlayer < attackRange) {
            if (Time.time > lastAttackTime + attackCooldown) {
                AttackPlayer();
            }
        } else if (distanceToPlayer < chaseRange) {
            ChasePlayer();
        } else {
            Patrol();
        }
    }

    void Patrol() {
        if (isWaiting) {
            waitCounter -= Time.deltaTime;
            if (waitCounter <= 0) {
                isWaiting = false;
                GoToNextPoint();
            }
            return; 
        }

        // Check if we reached the point
        if (!agent.pathPending && agent.remainingDistance < 0.5f) {
            BeginWaiting();
        }
    }

    void BeginWaiting() {
        isWaiting = true;
        waitCounter = waitTimeAtPoint;
        agent.ResetPath();
        
        anim.SetBool("running", false); //switch to idle/drunk sway
    }

    void GoToNextPoint() {
        if (patrolPoints.Length == 0) return;

        isWaiting = false;
        agent.isStopped = false;
        agent.destination = patrolPoints[currentPointIndex].position;
        
        currentPointIndex = (currentPointIndex + 1) % patrolPoints.Length;
        anim.SetBool("running", true);
    }

    void ChasePlayer() {
        isWaiting = false;
        agent.isStopped = false;
        agent.SetDestination(player.position);
        anim.SetBool("running", true);
    }

    void AttackPlayer() {
        lastAttackTime = Time.time;
        agent.isStopped = true;
        
        // Trigger your attack or idle/drunk animation
        anim.SetTrigger("idle"); 

        // Call the HealthManager on the player to count a strike
        HealthManager health = player.GetComponent<HealthManager>();
        if (health != null) {
            health.TakeDamage();
        }
    }
}


