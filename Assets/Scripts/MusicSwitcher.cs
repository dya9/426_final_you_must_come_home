using UnityEngine;

public class MusicSwitcher : MonoBehaviour
{
    public AudioSource musicSource;   // MusicManager here
    public AudioClip calmMusic;      // Blueline track here
    public AudioClip combatMusic;    // scary song here
    public Transform player;         //  Player object here
    public float detectionRange = 10f; // How close the enemy needs to be

    private bool isEnemyNear = false;

    void Update()
    {
        float distance = Vector3.Distance(transform.position, player.position);

        if (distance < detectionRange && !isEnemyNear)
        {
            SwitchTrack(combatMusic);
            isEnemyNear = true;
        }
        else if (distance > detectionRange && isEnemyNear)
        {
            SwitchTrack(calmMusic);
            isEnemyNear = false;
        }
    }

    void SwitchTrack(AudioClip newClip)
    {
        musicSource.Stop();
        musicSource.clip = newClip;
        musicSource.Play();
    }
}