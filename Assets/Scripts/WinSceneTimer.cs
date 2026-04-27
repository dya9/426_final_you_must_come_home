using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class WinSceneTimer : MonoBehaviour
{
    public string creditSceneName = "Credit scene";
    public float delay = 20f;

    void Start()
    {
        StartCoroutine(LoadCredits());
    }

    IEnumerator LoadCredits()
    {
        yield return new WaitForSeconds(delay);
        SceneManager.LoadScene(creditSceneName);
    }
}