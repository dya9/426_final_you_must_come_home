using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class stationMove : MonoBehaviour
{
    [Header("Waypoints - Set these in Inspector")]
    public Transform[] waypoints;
    public float moveSpeed = 3f;
    public float rotationSpeed = 3f;

    [Header("Scene to load at end")]
    public string nextSceneName = "Train";

    [Header("Fade Settings")]
    public float fadeDuration = 1.5f;

    private Image fadeImage;

    void Start()
    {
        SetupFadeCanvas();
        StartCoroutine(PlayCutscene());
    }

    void SetupFadeCanvas()
    {
        // Create a canvas for the fade overlay
        GameObject canvasObj = new GameObject("FadeCanvas");
        Canvas canvas = canvasObj.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvas.sortingOrder = 999; // On top of everything
        canvasObj.AddComponent<CanvasScaler>();
        canvasObj.AddComponent<GraphicRaycaster>();

        // Create the black image that covers the screen
        GameObject imageObj = new GameObject("FadeImage");
        imageObj.transform.SetParent(canvasObj.transform, false);
        fadeImage = imageObj.AddComponent<Image>();
        fadeImage.color = new Color(0f, 0f, 0f, 0f); // Start transparent

        // Stretch to fill screen
        RectTransform rect = fadeImage.GetComponent<RectTransform>();
        rect.anchorMin = Vector2.zero;
        rect.anchorMax = Vector2.one;
        rect.sizeDelta = Vector2.zero;
    }

    IEnumerator PlayCutscene()
    {
        // Fade in from black at the start
        yield return StartCoroutine(Fade(1f, 0f));

        for (int i = 0; i < waypoints.Length; i++)
        {
            while (Vector3.Distance(transform.position, waypoints[i].position) > 0.1f)
            {
                // Move
                transform.position = Vector3.MoveTowards(
                    transform.position,
                    waypoints[i].position,
                    moveSpeed * Time.deltaTime
                );

                // Rotate to face direction of travel
                Vector3 direction = (waypoints[i].position - transform.position).normalized;
                if (direction != Vector3.zero)
                {
                    Quaternion targetRot = Quaternion.LookRotation(direction);
                    transform.rotation = Quaternion.Slerp(
                        transform.rotation,
                        targetRot,
                        rotationSpeed * Time.deltaTime
                    );
                }

                yield return null;
            }

            // Small pause at each waypoint
            yield return new WaitForSeconds(0.2f);
        }

        // Pause at the train then fade to black
        yield return new WaitForSeconds(1f);
        yield return StartCoroutine(Fade(0f, 1f));

        // Load next scene
        SceneManager.LoadScene(nextSceneName);
    }

    IEnumerator Fade(float startAlpha, float endAlpha)
    {
        float elapsed = 0f;
        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            float alpha = Mathf.Lerp(startAlpha, endAlpha, elapsed / fadeDuration);
            fadeImage.color = new Color(0f, 0f, 0f, alpha);
            yield return null;
        }
        fadeImage.color = new Color(0f, 0f, 0f, endAlpha);
    }
}