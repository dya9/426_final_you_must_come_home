// using UnityEngine;
// using UnityEngine.AI;
// using UnityEngine.UI;

// public class EnemyScript : MonoBehaviour
// {
//     [Header("Health & UI")]
//     public float maxHealth = 3f; // Changed to 3 for the 3-hit rule
//     private float currentHealth;
//     public Slider healthBarSlider;
//     public Image healthBarFill;
//     public GameObject damagePopupPrefab;
//     public float popupHeightOffset = 2.2f;
//     public Button nextSceneButton; // Reference to the button on your canvas

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
        
//         // Hide the button at the start of the level
//         if (nextSceneButton != null) nextSceneButton.gameObject.SetActive(false);

//         UpdateHealthBar();

//         if (patrolPoints.Length > 0) {
//             GoToNextPoint();
//         }
//     }

//     void Update() 
//     {
//         if (isDead || player == null) return;
//         anim.SetBool("running", agent.velocity.magnitude > 0.1f);

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
//         //anim.SetTrigger("idle"); 

//         HealthManager health = player.GetComponent<HealthManager>();
//         if (health != null) {
//             health.TakeDamage();
//         }
//     }

//     // --- HEALTH & DAMAGE LOGIC ---

//     public void TakeDamage(float amount)
//     {
//         if (isDead) return;
        
//         // Force the damage to be 1 so it always takes exactly 3 hits
//         currentHealth -= 1f; 
        
//         currentHealth = Mathf.Clamp(currentHealth, 0f, maxHealth);
//         UpdateHealthBar();
//         SpawnDamagePopup(1f);

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
//         popup.GetComponent<DamagePopup>()?.Setup(amount);
//     }

//     void Die()
//     {
//         isDead = true;
//         agent.isStopped = true; 
//         agent.enabled = false;   
//         anim.SetTrigger("die");  

//         // Show the Next Scene button when the enemy is defeated
//         if (nextSceneButton != null) {
//             nextSceneButton.gameObject.SetActive(true);
            
//             // Unlocks cursor so player can actually click the button
//             Cursor.lockState = CursorLockMode.None;
//             Cursor.visible = true;
//         }

//         // We remove Destroy(gameObject) so the button stays visible 
//         // (If the script is destroyed, the button logic might fail depending on setup)
//     }
// }

using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(Animator))]
public class EnemyScript : MonoBehaviour
{
    [Header("Health & UI")]
    public float maxHealth = 3f;
    private float currentHealth;
    public Slider healthBarSlider;
    public Image healthBarFill;
    public GameObject damagePopupPrefab;
    public float popupHeightOffset = 2.2f;
    public Button nextSceneButton; 

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
        
        // Setup UI
        if (nextSceneButton != null) nextSceneButton.gameObject.SetActive(false);
        UpdateHealthBar();

        if (patrolPoints.Length > 0) {
            GoToNextPoint();
        }
    }

    void Update() 
    {
        if (isDead || player == null) return;

        // Keep animator updated based on NavMesh velocity
        anim.SetBool("running", agent.velocity.magnitude > 0.1f);

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
    }

    void ChasePlayer() {
        isWaiting = false;
        agent.isStopped = false;
        agent.SetDestination(player.position);
    }

    void AttackPlayer() {
        lastAttackTime = Time.time;
        agent.isStopped = true;
        anim.SetBool("running", false);

        // Assumes your player has a script called HealthManager
        HealthManager playerHealth = player.GetComponent<HealthManager>();
        if (playerHealth != null) {
            playerHealth.TakeDamage();
        }
    }

    // --- HEALTH & DAMAGE LOGIC ---

    public void TakeDamage(float amount)
    {
        if (isDead) return;
        
        // Force exactly 1 damage per hit for the 3-hit rule
        currentHealth -= 1f; 
        currentHealth = Mathf.Clamp(currentHealth, 0f, maxHealth);
        
        UpdateHealthBar();
        SpawnDamagePopup(1f);

        Debug.Log($"[Enemy] Hit! Current HP: {currentHealth}");

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
        
        // Uses the Setup method from your DamagePopup script
        popup.GetComponent<DamagePopup>()?.Setup(amount);
    }

    void Die()
    {
        if (isDead) return;
        isDead = true;

        Debug.Log($"[Enemy] {gameObject.name} has died.");

        // Stop AI
        agent.isStopped = true; 
        agent.enabled = false;   
        
        // Trigger Animation
        anim.SetTrigger("die");  

        // Show UI & Unlock Mouse
        if (nextSceneButton != null) {
            nextSceneButton.gameObject.SetActive(true);
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }

        // Optional: Destroy after 3 seconds to let animation finish
        // Destroy(gameObject, 3f); 
    }
}