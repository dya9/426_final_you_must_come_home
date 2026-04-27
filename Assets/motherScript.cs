// using UnityEngine;

// [RequireComponent(typeof(Animator))]
// public class MotherScript : MonoBehaviour
// {
//     [Header("Detection Settings")]
//     public Transform ghostEntity;       
//     public float nervousDistance = 5f;  
//     public float fallDistance = 1.5f;   

//     private Animator anim;
//     private bool isNervous = false;
//     private bool hasFallen = false;

//     // Animator Parameter Names (Matches your screenshot)
//     private readonly string nervousTrigger = "nervous"; 
//     private readonly string fallTrigger = "fall";

//     void Start()
//     {
//         anim = GetComponent<Animator>();
        
//         // Auto-find the ghost if not assigned in Inspector
//         if (ghostEntity == null)
//         {
//             GameObject ghostObj = GameObject.FindGameObjectWithTag("Ghost");
//             if (ghostObj != null) 
//                 ghostEntity = ghostObj.transform;
//             else
//                 Debug.LogWarning("MotherScript: No Ghost assigned or found with tag 'Ghost'!");
//         }
//     }

//     void Update()
//     {
//         // Stop checking once she has fallen or if ghost is missing
//         if (hasFallen || ghostEntity == null) return;

//         float distance = Vector3.Distance(transform.position, ghostEntity.position);

//         // Stage 1: Transition from talkingInplace to nervousLook
//         if (distance <= nervousDistance && !isNervous)
//         {
//             isNervous = true;
//             anim.SetTrigger(nervousTrigger);
//             Debug.Log("Mother is getting nervous...");
//         }

//         // Stage 2: Transition from nervousLook to fallingDown
//         if (distance <= fallDistance)
//         {
//             TriggerFall();
//         }
//     }

//     void TriggerFall()
//     {
//         hasFallen = true;
//         anim.SetTrigger(fallTrigger);
//         Debug.Log("Mother fainted!");
        
//         // Optional: Disables the script so it stops calculating distance
//         this.enabled = false;
//     }
// }

// using UnityEngine;

// [RequireComponent(typeof(Animator))]
// public class MotherScript : MonoBehaviour
// {
//     [Header("Sequence Timings")]
//     public float timeToBecomeNervous = 1f;
//     public float timeToFall = 4f; 

//     private Animator anim;
//     private float timer = 0f;
//     private bool isNervous = false;
//     private bool hasFallen = false;

//     // Animator Parameter Names (Matches your Animator Controller)
//     private readonly string nervousTrigger = "nervous"; 
//     private readonly string fallTrigger = "fall";

//     void Start()
//     {
//         anim = GetComponent<Animator>();
//         timer = 0f;
//     }

//     void Update()
//     {
//         // If she already fell, stop counting
//         if (hasFallen) return;

//         timer += Time.deltaTime;

//         // Stage 1: Nervous after 3 seconds
//         if (timer >= timeToBecomeNervous && !isNervous)
//         {
//             isNervous = true;
//             anim.SetTrigger(nervousTrigger);
//             Debug.Log("Mother started feeling nervous at: " + timer + "s");
//         }

//         // Stage 2: Collapse after 4 more seconds (Total 7 seconds)
//         if (timer >= timeToFall && !hasFallen)
//         {
//             TriggerFall();
//         }
//     }

//     void TriggerFall()
//     {
//         hasFallen = true;
//         anim.SetTrigger(fallTrigger);
//         Debug.Log("Mother collapsed at: " + timer + "s");
        
//         // Disables the script so the timer stops running
//         this.enabled = false;
//     }
// }
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class MotherScript : MonoBehaviour
{
    [Header("Sequence Timings")]
    public float timeToBecomeNervous = 2f;
    public float timeToFall = 3f; 

    [Header("Transition")]
    public SceneChanger sceneChanger; // Drag the FadeCanvas here
    public string nextSceneName = "Friend's Apartment";

    private Animator anim;
    private float timer = 0f;
    private bool isNervous = false;
    private bool hasFallen = false;

    private readonly string nervousTrigger = "nervous"; 
    private readonly string fallTrigger = "fall";

    void Start()
    {
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        if (hasFallen) return;

        timer += Time.deltaTime;

        if (timer >= timeToBecomeNervous && !isNervous)
        {
            isNervous = true;
            anim.SetTrigger(nervousTrigger);
        }

        if (timer >= timeToFall)
        {
            TriggerFall();
        }
    }

    void TriggerFall()
    {
        hasFallen = true;
        anim.SetTrigger(fallTrigger);
        
        // Start the fade to black
        if (sceneChanger != null)
        {
            sceneChanger.StartFade(nextSceneName);
        }

        this.enabled = false;
    }
}