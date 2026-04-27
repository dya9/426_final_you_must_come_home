using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement; // Required for switching scenes

public class Dialogue : MonoBehaviour
{
    [Header("UI References")]
    public GameObject dialogueCanvas;
    public TextMeshProUGUI dialogueText;
    public Image panelImage;
    public Button nextArrowButton; 
    public Button nextSceneButton; // Drag your new "Next Scene" button here

    [Header("Colors")]
    public Color rehemColor = new Color(1f, 0f, 0f, 0.6f);
    public Color entityColor = new Color(0f, 0f, 0f, 0.8f);

    private int index = 0;
    
    private string[] dialogueLines = new string[]
    {
        "Rehem: AAAAAHHHHHHH!!!",
        "Rehem: WHO ARE YOU?!?!",
        "Entity: I am a clone of a clone of a clone of clone",
        "Entity: I not the source, nor am I the bone",
        "Entity: If you wish to defeat me ",
        "Entity: YOU MUST COME HOME"
    };

    void Start()
    {
        index = 0;
        
        // Hide the next scene button at the start
        if(nextSceneButton != null) nextSceneButton.gameObject.SetActive(false);

        ShowLine();

        if (nextArrowButton != null)
            nextArrowButton.onClick.AddListener(AdvanceDialogue);

        // Set up the next scene button click
        if (nextSceneButton != null)
            nextSceneButton.onClick.AddListener(LoadWinScene);
    }

    void Update()
    {
        // Only allow space/R if we haven't reached the end yet
        if (dialogueCanvas.activeSelf && index < dialogueLines.Length - 1)
        {
            if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.R))
            {
                AdvanceDialogue();
            }
        }
    }

    public void AdvanceDialogue()
    {
        index++;
        if (index < dialogueLines.Length)
        {
            ShowLine();
        }
    }

    void ShowLine()
    {
        dialogueCanvas.SetActive(true);
        string currentLine = dialogueLines[index];
        dialogueText.text = currentLine;

        // Change colors
        if (currentLine.StartsWith("Rehem")) panelImage.color = rehemColor;
        else if (currentLine.StartsWith("Entity")) panelImage.color = entityColor;

        // Check if this is the LAST line
        if (index == dialogueLines.Length - 1)
        {
            // Hide the arrow, show the "Next Scene" button
            if (nextArrowButton != null) nextArrowButton.gameObject.SetActive(false);
            if (nextSceneButton != null) nextSceneButton.gameObject.SetActive(true);
        }
    }

    void LoadWinScene()
    {
        
        SceneManager.LoadScene("WinScene");
    }
}