// using UnityEngine;
// using UnityEngine.SceneManagement;

// public class SceneLoader : MonoBehaviour
// {
//     public void StartGame()
//     {
//         // loads the next scene after MainMenu in Build Settings
//         SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
//     }

//     public void LoadNextScene()
//     {
//         SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
//     }

//     public void LoadSceneByName(string sceneName)
//     {
//         SceneManager.LoadScene(sceneName);
//     }
// }
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    // Call this from your Main Menu button to go specifically to the cutscene
    public void StartGame()
    {
        SceneManager.LoadScene("cutscene1");
    }

    public void LoadNextScene()
    {
        // Keeps the index logic if you prefer, but "cutscene1" must be index 1
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void LoadSceneByName(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
}