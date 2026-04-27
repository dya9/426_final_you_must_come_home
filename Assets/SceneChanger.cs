using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class SceneChanger : MonoBehaviour
{
    public Image fadeImage;
    public float fadeDuration = 2f;

    public void StartFade(string sceneName)
    {
        StartCoroutine(FadeOut(sceneName));
    }

    private IEnumerator FadeOut(string sceneName)
    {
        float timer = 0;
        Color tempColor = fadeImage.color;

        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;
            tempColor.a = Mathf.Lerp(0, 1, timer / fadeDuration);
            fadeImage.color = tempColor;
            yield return null;
        }

        SceneManager.LoadScene(sceneName);
    }
}