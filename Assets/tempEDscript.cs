using UnityEngine;

public class tempEDscript : MonoBehaviour
{
    public AudioClip drinkSound;
    private AudioSource audioSource;
    private bool canDrink = false;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
    }

    void Update()
    {
        //player is inside the trigger and presses X
        if (canDrink && Input.GetKeyDown(KeyCode.X))
        {
            Drink();
        }
    }

    void Drink()
    {
        if (drinkSound != null)
        {
            audioSource.PlayOneShot(drinkSound);
            Debug.Log("Sound playing.");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) 
        {
            canDrink = true;
            Debug.Log("Press X to Drink");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            canDrink = false;
        }
    }
}