using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
 
public class Dialogue : MonoBehaviour
{
    public static Dialogue Instance;
 
    [Header("UI References")]
    public GameObject dialoguePanel;
    public TextMeshProUGUI speakerNameText;
    public TextMeshProUGUI dialogueText;
    public Button nextButton;
    public GameObject continueIndicator;
 
    [Header("Typewriter Settings")]
    public float typingSpeed = 0.04f;
 
    [Header("Dialogue Lines")]
    public DialogueLine[] lines;
 
    private int currentLine   = 0;
    private bool isTyping     = false;
    private bool dialogueDone = false;
    private Coroutine typingCoroutine;
 
    void Awake()
    {
        Instance = this;
    }
 
    void Start()
    {
        // Panel visibility is controlled manually in the Editor
        // and by StartDialogue() / EndDialogue() at runtime.
        // We do NOT hide it here so you can see it in the scene.
 
        if (continueIndicator != null)
            continueIndicator.SetActive(false);
 
        if (nextButton != null)
            nextButton.onClick.AddListener(OnNextPressed);
 
        // Auto-start the dialogue when the scene loads
        if (lines.Length > 0)
            ShowLine(currentLine);
    }
 
    // ── Public ────────────────────────────────────────────────────────────────
 
    public void StartDialogue()
    {
        currentLine  = 0;
        dialogueDone = false;
 
        if (dialoguePanel != null)
            dialoguePanel.SetActive(true);
 
        if (continueIndicator != null)
            continueIndicator.SetActive(false);
 
        ShowLine(currentLine);
    }
 
    public void OnNextPressed()
    {
        if (dialogueDone)
        {
            EndDialogue();
            return;
        }
 
        // If still typing — skip to full line instantly
        if (isTyping)
        {
            SkipTyping();
            return;
        }
 
        // Move to next line
        currentLine++;
        if (currentLine >= lines.Length)
        {
            dialogueDone = true;
            if (continueIndicator != null)
                continueIndicator.SetActive(true);
            return;
        }
 
        ShowLine(currentLine);
    }
 
    // ── Private ───────────────────────────────────────────────────────────────
 
    void ShowLine(int index)
    {
        if (continueIndicator != null)
            continueIndicator.SetActive(false);
 
        if (speakerNameText != null)
            speakerNameText.text = lines[index].speakerName;
 
        if (typingCoroutine != null)
            StopCoroutine(typingCoroutine);
 
        typingCoroutine = StartCoroutine(TypeLine(lines[index].text));
    }
 
    IEnumerator TypeLine(string line)
    {
        isTyping = true;
        dialogueText.text = "";
 
        foreach (char c in line)
        {
            dialogueText.text += c;
            yield return new WaitForSeconds(typingSpeed);
        }
 
        isTyping = false;
 
        if (continueIndicator != null)
            continueIndicator.SetActive(true);
    }
 
    void SkipTyping()
    {
        if (typingCoroutine != null)
            StopCoroutine(typingCoroutine);
 
        dialogueText.text = lines[currentLine].text;
        isTyping = false;
 
        if (continueIndicator != null)
            continueIndicator.SetActive(true);
    }
 
    void EndDialogue()
    {
        // Panel stays visible — just clears the text
        // If you want it to hide on finish, change this to:
        // dialoguePanel.SetActive(false);
        dialogueText.text = "";
        if (speakerNameText != null)
            speakerNameText.text = "";
        if (continueIndicator != null)
            continueIndicator.SetActive(false);
 
        currentLine  = 0;
        dialogueDone = false;
    }
}
 
[System.Serializable]
public class DialogueLine
{
    public string speakerName;
    [TextArea(2, 5)]
    public string text;
}