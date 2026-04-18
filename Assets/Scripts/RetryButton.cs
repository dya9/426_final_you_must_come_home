using UnityEngine;
using UnityEngine.SceneManagement;
 
public class RetryButton : MonoBehaviour
{
    // Set this to the exact name of your main game scene in the Inspector
    public string gameSceneName = "Sophmore";
 
    public void OnRetryClicked()
    {
        SceneManager.LoadScene(gameSceneName);
    }
}
 
