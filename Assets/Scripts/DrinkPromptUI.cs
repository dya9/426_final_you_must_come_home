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
 
        // Drink button: consume and destroy
        drinkButton.onClick.AddListener(() => currentDrink?.Consume());
 
        // FIX: Leave button now calls Leave() on the drink, which handles
        // its own destruction — the UI alone no longer decides the drink's fate
        leaveButton.onClick.AddListener(() => currentDrink?.Leave());
    }
 
    public void HidePrompt()
    {
        promptPanel.SetActive(false);
        currentDrink = null;
    }
}