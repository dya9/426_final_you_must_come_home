using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using TMPro;

public class Dialogue : MonoBehaviour
{

    //reference to TMPro component
    public TextMeshProUGUI textComponent;

    //Collection of dialogue we want to display
    public string[] lines;

    //Type out each character at a specific speed
    public float textSpeed;

    //private indexer of where we are in convo
    private int index;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //Initializee string
        textComponent.text = string.Empty;
        //Start typing
        StartDialogue();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (textComponent.text == lines[index])
            {
                NextLine();
            }
            else
            {
                //Fill out current line when click
                StopAllCoroutines();
                textComponent.text = lines[index];
            }
        }
    }

    void StartDialogue()
    {
        index = 0;
        StartCoroutine(TypeLine());
    }

    //Type out each character one by one
    IEnumerator TypeLine()
    {
        foreach (char item in lines[index].ToCharArray())
        {
            textComponent.text += item;
            yield return new WaitForSeconds(textSpeed);
        }
    }

    //Move to next dialogue
    void NextLine()
    {
        if (index < lines.Length - 1)
        {
            index++;
            textComponent.text = string.Empty;
            StartCoroutine(TypeLine());
        }
        else
        {
            gameObject.SetActive(false);
        }
    }
}