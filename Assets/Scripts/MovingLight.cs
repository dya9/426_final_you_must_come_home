using UnityEngine;
 
public class MovingLight : MonoBehaviour
{
    [Header("Points")]
    public Transform startPoint;  // back of the train
    public Transform endPoint;    // front of the train
 
    [Header("Settings")]
    public float speed       = 5f;   // how fast the light moves
    public float pauseTime   = 1f;   // how long it waits at each end before returning
    public bool  loop        = true; // keep sweeping back and forth
    public bool  oneWayLoop  = false;// if true, snaps back to start instead of reversing
 
    private float   pauseTimer  = 0f;
    private bool    isPausing   = false;
    private bool    goingForward = true; // true = back to front, false = front to back
 
    void Update()
    {
        if (startPoint == null || endPoint == null) return;
 
        // ── Pause at end points ───────────────────────────────────────────────
        if (isPausing)
        {
            pauseTimer -= Time.deltaTime;
            if (pauseTimer <= 0f)
            {
                isPausing = false;
 
                if (oneWayLoop)
                {
                    // Snap back to start instantly and go forward again
                    transform.position = startPoint.position;
                }
                else
                {
                    // Reverse direction
                    goingForward = !goingForward;
                }
            }
            return;
        }
 
        // ── Move the light ────────────────────────────────────────────────────
        Transform target = goingForward ? endPoint : startPoint;
 
        transform.position = Vector3.MoveTowards(
            transform.position,
            target.position,
            speed * Time.deltaTime
        );
 
        // ── Check if reached target ───────────────────────────────────────────
        if (Vector3.Distance(transform.position, target.position) < 0.05f)
        {
            transform.position = target.position;
 
            if (loop)
            {
                isPausing  = true;
                pauseTimer = pauseTime;
            }
        }
    }
}