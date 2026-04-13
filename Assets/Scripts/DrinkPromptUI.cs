using UnityEngine;
using UnityEngine.UI;
 
public class DrinkPromptUI : MonoBehaviour
{
    public static DrinkPromptUI Instance;
 
    public GameObject promptPanel;
    public Button drinkButton;
    public Button leaveButton;
 
    private energyDrink currentDrink;
 
    void Awake()
    {
        Instance = this;
        promptPanel.SetActive(false);
    }
 
    public void ShowPrompt(energyDrink drink)
    {
        currentDrink = drink;
        promptPanel.SetActive(true);

        drinkButton.onClick.RemoveAllListeners();
        leaveButton.onClick.RemoveAllListeners();

        drinkButton.onClick.AddListener(() => currentDrink?.Consume());
        
        leaveButton.onClick.AddListener(() => {
            HidePrompt(); // Just hide UI, drink stays in the world
        });
    }

    public void HidePrompt()
    {
        promptPanel.SetActive(false);
        currentDrink = null;
    }
}