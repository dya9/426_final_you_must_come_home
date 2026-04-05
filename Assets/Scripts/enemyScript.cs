// using UnityEngine;
// using UnityEngine.AI;

// public class enemyScript : MonoBehaviour
// {
//     public Transform[] patrolPoints;
//     public Transform player;
//     public float chaseRange = 10f;
//     public float attackRange = 2f;
    
//     private NavMeshAgent agent;
//     private Animator anim;
//     private int currentPointIndex;

//     void Start() {
//         agent = GetComponent<NavMeshAgent>();
//         anim = GetComponent<Animator>();
//         GoToNextPoint();
//     }

//     void Update() {
//         float distanceToPlayer = Vector3.Distance(transform.position, player.position);

//         if (distanceToPlayer < attackRange) {
//             AttackPlayer();
//         } else if (distanceToPlayer < chaseRange) {
//             agent.SetDestination(player.position);
//             anim.SetBool("isRunning", true);
//         } else {
//             Patrol();
//         }
//     }

//     void Patrol() {
//         if (!agent.pathPending && agent.remainingDistance < 0.5f)
//             GoToNextPoint();
//         anim.SetBool("isRunning", true);
//     }

//     void GoToNextPoint() {
//         if (patrolPoints.Length == 0) return;
//         currentPointIndex = (currentPointIndex + 1) % patrolPoints.Length;
//         agent.destination = patrolPoints[currentPointIndex].position;
//     }

//     void AttackPlayer() {
//         // Trigger attack animation and logic here
//         agent.isStopped = true;
//         anim.SetTrigger("attack"); 
//         // Logic to deal damage (see step 2)
//     }
// }

using UnityEngine;
using UnityEngine.AI;

public class enemyScript : MonoBehaviour
{
    [Header("Detection Settings")]
    public Transform player;
    public float chaseRange = 10f;
    public float attackRange = 2f;
    
    [Header("Patrol Settings")]
    public Transform[] patrolPoints;
    public float waitTimeAtPoint = 3f; // How long he sways while "drunk"

    private NavMeshAgent agent;
    private Animator anim;
    private int currentPointIndex;
    private float waitCounter;
    private bool isWaiting = false;

    // Attack Cooldown to prevent losing all 4 strikes in 1 second
    private float attackCooldown = 1.5f;
    private float lastAttackTime;

    void Start() {
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
        
        if (patrolPoints.Length > 0) {
            GoToNextPoint();
        }
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