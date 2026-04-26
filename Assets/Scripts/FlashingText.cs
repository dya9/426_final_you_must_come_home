using UnityEngine;
using TMPro;
using System.Collections;

public class FlashingText : MonoBehaviour
{
    [Header("Text Settings")]
    public TextMeshProUGUI flashText;
    public float timeBetweenFlashes = 5f;
    public float displayDuration = 0.5f;

    [Header("Messages")]
    public string[] messages = new string[]
    {
        "YOU MUST COME HOME",
        "IT'S WATCHING",
        "TURN BACK",
        "SHE NEEDS YOU",
        "IT FOLLOWED YOU HERE"
    };

    void Start()
    {
        flashText.alpha = 0f;
        StartCoroutine(FlashRoutine());
    }

    IEnumerator FlashRoutine()
    {
        while (true)
        {
            // Wait between 3-7 seconds randomly around your base time
            float waitTime = Random.Range(timeBetweenFlashes - 2f, timeBetweenFlashes + 2f);
            yield return new WaitForSeconds(waitTime);

            // Pick a random message
            flashText.text = messages[Random.Range(0, messages.Length)];

            // Fade in
            yield return StartCoroutine(FadeText(0f, 1f, 0.1f));

            // Hold
            yield return new WaitForSeconds(displayDuration);

            // Fade out
            yield return StartCoroutine(FadeText(1f, 0f, 0.1f));
        }
    }

    IEnumerator FadeText(float startAlpha, float endAlpha, float duration)
    {
        float elapsed = 0f;
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            flashText.alpha = Mathf.Lerp(startAlpha, endAlpha, elapsed / duration);
            yield return null;
        }
        flashText.alpha = endAlpha;
    }
}