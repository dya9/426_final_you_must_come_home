// using UnityEngine;
// using UnityEngine.AI;
// using UnityEngine.UI;

// public class EnemyScript : MonoBehaviour
// {
//     [Header("Health & UI")]
//     public float maxHealth = 100f;
//     private float currentHealth;
//     public Slider healthBarSlider;
//     public Image healthBarFill;
//     public GameObject damagePopupPrefab;
//     public float popupHeightOffset = 2.2f;

//     [Header("Health Bar Colors")]
//     public Color fullHealthColor = Color.green;
//     public Color halfHealthColor = Color.yellow;
//     public Color lowHealthColor = Color.red;

//     [Header("Detection & Combat")]
//     public Transform player;
//     public float chaseRange = 10f;
//     public float attackRange = 2f;
//     public float attackCooldown = 1.5f;
//     private float lastAttackTime;

//     [Header("Patrol Settings")]
//     public Transform[] patrolPoints;
//     public float waitTimeAtPoint = 3f; 

//     private NavMeshAgent agent;
//     private Animator anim;
//     private int currentPointIndex;
//     private float waitCounter;
//     private bool isWaiting = false;
//     private bool isDead = false;

//     void Start()
//     {
//         agent = GetComponent<NavMeshAgent>();
//         anim = GetComponent<Animator>();
//         currentHealth = maxHealth;
        
//         UpdateHealthBar();

//         if (patrolPoints.Length > 0) {
//             GoToNextPoint();
//         }
//     }

//     void Update() 
//     {
//         if (isDead || player == null) return;

//         float distanceToPlayer = Vector3.Distance(transform.position, player.position);

//         if (distanceToPlayer < attackRange) {
//             if (Time.time > lastAttackTime + attackCooldown) {
//                 AttackPlayer();
//             }
//         } else if (distanceToPlayer < chaseRange) {
//             ChasePlayer();
//         } else {
//             Patrol();
//         }
//     }

//     // --- MOVEMENT LOGIC ---

//     void Patrol() {
//         if (isWaiting) {
//             waitCounter -= Time.deltaTime;
//             if (waitCounter <= 0) {
//                 isWaiting = false;
//                 GoToNextPoint();
//             }
//             return; 
//         }

//         if (!agent.pathPending && agent.remainingDistance < 0.5f) {
//             BeginWaiting();
//         }
//     }

//     void BeginWaiting() {
//         isWaiting = true;
//         waitCounter = waitTimeAtPoint;
//         agent.ResetPath();
//         anim.SetBool("running", false); 
//     }

//     void GoToNextPoint() {
//         if (patrolPoints.Length == 0) return;
//         isWaiting = false;
//         agent.isStopped = false;
//         agent.destination = patrolPoints[currentPointIndex].position;
//         currentPointIndex = (currentPointIndex + 1) % patrolPoints.Length;
//         anim.SetBool("running", true);
//     }

//     void ChasePlayer() {
//         isWaiting = false;
//         agent.isStopped = false;
//         agent.SetDestination(player.position);
//         anim.SetBool("running", true);
//     }

//     void AttackPlayer() {
//         lastAttackTime = Time.time;
//         agent.isStopped = true;
//         anim.SetBool("running", false);
//         anim.SetTrigger("idle"); // Or "attack" if you have a specific attack trigger

//         HealthManager health = player.GetComponent<HealthManager>();
//         if (health != null) {
//             health.TakeDamage();
//         }
//     }

//     // --- HEALTH & DAMAGE LOGIC ---

//     public void TakeDamage(float amount)
//     {
//         if (isDead) return;
//         currentHealth -= amount;
//         currentHealth = Mathf.Clamp(currentHealth, 0f, maxHealth);
//         UpdateHealthBar();
//         SpawnDamagePopup(amount);

//         if (currentHealth <= 0f) Die();
//     }

//     void UpdateHealthBar()
//     {
//         if (healthBarSlider != null) {
//             healthBarSlider.maxValue = maxHealth;
//             healthBarSlider.value = currentHealth;
//         }
//         if (healthBarFill != null) {
//             float pct = currentHealth / maxHealth;
//             if (pct > 0.5f)
//                 healthBarFill.color = Color.Lerp(halfHealthColor, fullHealthColor, (pct - 0.5f) * 2f);
//             else
//                 healthBarFill.color = Color.Lerp(lowHealthColor, halfHealthColor, pct * 2f);
//         }
//     }

//     void SpawnDamagePopup(float amount)
//     {
//         if (damagePopupPrefab == null) return;
//         Vector3 spawnPos = transform.position + Vector3.up * popupHeightOffset;
//         GameObject popup = Instantiate(damagePopupPrefab, spawnPos, Quaternion.identity);
//         // Assumes your DamagePopup script has a Setup(float) method
//         popup.GetComponent<DamagePopup>()?.Setup(amount);
//     }

//     void Die()
//     {
//         isDead = true;
//         agent.isStopped = true; // Stop moving on death
//         agent.enabled = false;   // Disable agent so it doesn't block others
//         anim.SetTrigger("die");  // Assumes you have a "die" trigger in your Animator
//         Destroy(gameObject, 2.0f); // Longer delay to allow death animation to play
//     }
// }

using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class EnemyScript : MonoBehaviour
{
    [Header("Health & UI")]
    public float maxHealth = 3f; // Changed to 3 for the 3-hit rule
    private float currentHealth;
    public Slider healthBarSlider;
    public Image healthBarFill;
    public GameObject damagePopupPrefab;
    public float popupHeightOffset = 2.2f;
    public Button nextSceneButton; // Reference to the button on your canvas

    [Header("Health Bar Colors")]
    public Color fullHealthColor = Color.green;
    public Color halfHealthColor = Color.yellow;
    public Color lowHealthColor = Color.red;

    [Header("Detection & Combat")]
    public Transform player;
    public float chaseRange = 10f;
    public float attackRange = 2f;
    public float attackCooldown = 1.5f;
    private float lastAttackTime;

    [Header("Patrol Settings")]
    public Transform[] patrolPoints;
    public float waitTimeAtPoint = 3f; 

    private NavMeshAgent agent;
    private Animator anim;
    private int currentPointIndex;
    private float waitCounter;
    private bool isWaiting = false;
    private bool isDead = false;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
        currentHealth = maxHealth;
        
        // Hide the button at the start of the level
        if (nextSceneButton != null) nextSceneButton.gameObject.SetActive(false);

        UpdateHealthBar();

        if (patrolPoints.Length > 0) {
            GoToNextPoint();
        }
    }

    void Update() 
    {
        if (isDead || player == null) return;

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

    // --- MOVEMENT LOGIC ---

    void Patrol() {
        if (isWaiting) {
            waitCounter -= Time.deltaTime;
            if (waitCounter <= 0) {
                isWaiting = false;
                GoToNextPoint();
            }
            return; 
        }

        if (!agent.pathPending && agent.remainingDistance < 0.5f) {
            BeginWaiting();
        }
    }

    void BeginWaiting() {
        isWaiting = true;
        waitCounter = waitTimeAtPoint;
        agent.ResetPath();
        anim.SetBool("running", false); 
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
        anim.SetBool("running", false);
        anim.SetTrigger("idle"); 

        HealthManager health = player.GetComponent<HealthManager>();
        if (health != null) {
            health.TakeDamage();
        }
    }

    // --- HEALTH & DAMAGE LOGIC ---

    public void TakeDamage(float amount)
    {
        if (isDead) return;
        
        // Force the damage to be 1 so it always takes exactly 3 hits
        currentHealth -= 1f; 
        
        currentHealth = Mathf.Clamp(currentHealth, 0f, maxHealth);
        UpdateHealthBar();
        SpawnDamagePopup(1f);

        if (currentHealth <= 0f) Die();
    }

    void UpdateHealthBar()
    {
        if (healthBarSlider != null) {
            healthBarSlider.maxValue = maxHealth;
            healthBarSlider.value = currentHealth;
        }
        if (healthBarFill != null) {
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
        popup.GetComponent<DamagePopup>()?.Setup(amount);
    }

    void Die()
    {
        isDead = true;
        agent.isStopped = true; 
        agent.enabled = false;   
        anim.SetTrigger("die");  

        // Show the Next Scene button when the enemy is defeated
        if (nextSceneButton != null) {
            nextSceneButton.gameObject.SetActive(true);
            
            // Unlocks cursor so player can actually click the button
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }

        // We remove Destroy(gameObject) so the button stays visible 
        // (If the script is destroyed, the button logic might fail depending on setup)
    }
}