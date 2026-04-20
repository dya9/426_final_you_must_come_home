using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialogueTrigger : MonoBehaviour
{
    [Header("UI References")]
    public GameObject dialogueCanvas;
    public TextMeshProUGUI dialogueText;
    public Image panelImage;
    public Button nextArrowButton; // Drag your Arrow Button here

    [Header("Colors")]
    public Color marnieColor = new Color(0f, 0.5f, 1f, 0.6f);  // Blue
    public Color rehemColor = new Color(1f, 0f, 0f, 0.6f);   // Red
    public Color momColor = new Color(1f, 0.4f, 0.7f, 0.6f); // Pink
    public Color entityColor = new Color(0f, 0f, 0f, 0.8f);  // Black

    private int index = 0;
    
    private string[] dialogueLines = new string[]
    {
        "Rehem: Marnie I’m sooo exhausted I’ve never been so sleep deprived.",
        "Marnie: Yeah you’ve been up for like 3 days straight right??",
        "Rehem: God, yeah, you’re right, and it doesn’t help that my parents are mad at me for staying on campus for so long.",
        "Marnie: That’s so crazy to me. You’re a senior in college and they’re treating you like a kid.",
        "Rehem: RIGHT. This 426 final project is frustrating enough, especially with group members that don’t do jack. Anyways, thank you for letting me crash at your place, I really appreciate it.",
        "Marnie: I’m sorry your group is so chuzzy, and stop thanking me for letting you spend the night, it’s the least I can do. Grab yourself some wine, there’s a special bottle on the shelf that I haven’t opened yet.",
        "Rehem: Thanks Marns you’re the best!",
        "Rehem: This tastes so good.",
        "Marnie: Save some for me!",
        "Rehem: (Phone Rings) Hello, who is this?",
        "Entity: It wondered why it was alone in the world, why it was different from everyone else. But what it didn’t know was that it had a hole in its soul, one passed down from its flawed creation, its flawed mother. The hole never mended.",
        "Rehem: Hello, I think you have the wrong number bucko.",
        "Mom: REHEM HELP ME PLEASE!",
        "Entity: YOU MUST COME HOME.",
        "Rehem: MOM, IS THAT YOU?? WHAT’S GOING ON. HELLO WHOSE THERE?",
        "Marnie: What’s going on, is your mom okay??",
        "Rehem: I have to go.",
        "Marnie: What’s wrong, is your mom okay?? Should I call the police?",
        "Rehem: NO, I’ll deal with it.",
        "Marnie: You’re scaring me please be safe."
    };

    void Start()
    {
        // This shows the first line of dialogue as soon as the game opens
        index = 0;
        ShowLine();

        // Optional: Programmatically tell the arrow button to run AdvanceDialogue
        if (nextArrowButton != null)
        {
            nextArrowButton.onClick.AddListener(AdvanceDialogue);
        }
    }

    void Update()
    {
        if (dialogueCanvas.activeSelf && (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.R)))
        {
            AdvanceDialogue();
        }
    }

    public void AdvanceDialogue()
    {
        index++;
        if (index < dialogueLines.Length)
        {
            ShowLine();
        }
        else
        {
            dialogueCanvas.SetActive(false); 
        }
    }

    void ShowLine()
    {
        dialogueCanvas.SetActive(true);
        string currentLine = dialogueLines[index];
        dialogueText.text = currentLine;

        if (currentLine.StartsWith("Marnie")) panelImage.color = marnieColor;
        else if (currentLine.StartsWith("Rehem")) panelImage.color = rehemColor;
        else if (currentLine.StartsWith("Mom")) panelImage.color = momColor;
        else if (currentLine.StartsWith("Entity")) panelImage.color = entityColor;
    }
}