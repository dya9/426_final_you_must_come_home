using UnityEngine;
using UnityEngine.SceneManagement;

public class NextSceneButton : MonoBehaviour
{
    public string nextSceneName;

    void Start()
    {
        GetComponent<UnityEngine.UI.Button>().onClick.AddListener(() =>
            SceneManager.LoadScene(nextSceneName));
    }
}