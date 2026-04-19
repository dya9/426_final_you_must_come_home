// using UnityEngine;
// using TMPro; // Needed for TextMeshPro

// public class DialogueTrigger : MonoBehaviour
// {
//     public GameObject dialogueCanvas;
//     public TextMeshProUGUI dialogueText;

//     void OnMouseDown()
//     {
//         // runs when Rehem clicks on Marnie's collider
//         dialogueCanvas.SetActive(true);
//         dialogueText.text = "Marnie: Hey Rehem, did you finish the asset import?";
        
        
//         Cursor.lockState = CursorLockMode.None;
//         Cursor.visible = true;
//     }
// }

// using UnityEngine;
// using UnityEngine.UI;
// using TMPro;

// public class DialogueManager : MonoBehaviour
// {
//     public GameObject dialogueCanvas;
//     public TextMeshProUGUI dialogueText;
//     public Image panelImage; 

//     public void ShowMarnieDialogue()
//     {
//         dialogueCanvas.SetActive(true);
//         panelImage.color = new Color(0, 0, 1, 0.5f); // Blue (R, G, B, Alpha)
//         dialogueText.text = "Marnie: This blue box represents my cool personality.";
//     }

//     public void ShowRehemDialogue()
//     {
//         dialogueCanvas.SetActive(true);
//         panelImage.color = new Color(1, 0, 0, 0.5f); // Red (R, G, B, Alpha)
//         dialogueText.text = "Rehem: And this red box means I'm the protagonist!";
//     }
// }

using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialogueManager : MonoBehaviour
{
    public GameObject dialogueCanvas;
    public TextMeshProUGUI dialogueText;
    public Image panelImage; 

    // This is the "Engine" that runs when you click the 3D object
    private void OnMouseDown()
    {
        ShowMarnieDialogue();
    }

    public void ShowMarnieDialogue()
    {
        dialogueCanvas.SetActive(true);
        // Note: I added 'f' after the numbers to ensure they are treated as floats
        panelImage.color = new Color(0f, 0f, 1f, 0.5f); 
        dialogueText.text = "Marnie: This blue box represents my cool personality.";
    }

    public void ShowRehemDialogue()
    {
        dialogueCanvas.SetActive(true);
        panelImage.color = new Color(1f, 0f, 0f, 0.5f); 
        dialogueText.text = "Rehem: And this red box means I'm the protagonist!";
    }
}