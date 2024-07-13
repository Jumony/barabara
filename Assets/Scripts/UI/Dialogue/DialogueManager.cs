using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    public GameObject dialoguePrefab;
    public TextMeshProUGUI characterNameText;
    public TextMeshProUGUI dialogueText;
    public Image speakerImage;

    private Queue<DialogueLines> dialogueLines;
    private bool isDialogueActive = false;

    private void Start()
    {
        dialogueLines = new Queue<DialogueLines>();
        dialoguePrefab.SetActive(false);
    }

    private void Update()
    {
        if (isDialogueActive && Input.GetKeyDown(KeyCode.Space))
        {
            DisplayNextLine();
        }
    }

    // This is meant to be called
    public void StartDialogue(Dialogue dialogue)
    {
        dialoguePrefab.SetActive(true);
        dialogueLines.Clear();
        DialogueLines testLine;
        testLine = dialogue.lines[1];

        foreach (DialogueLines line in dialogue.lines)
        {
            dialogueLines.Enqueue(line);
        }

        isDialogueActive = true;
        DisplayNextLine();
    }

    public void DisplayNextLine()
    {
        if (dialogueLines.Count == 0)
        {
            EndDialogue();
            return;
        }

        DialogueLines line = dialogueLines.Dequeue();
        characterNameText.text = line.speakerName;
        StopAllCoroutines();
        StartCoroutine(TypeSentence(line.text));
    }

    IEnumerator TypeSentence(string sentence)
    {
        dialogueText.text = "";
        foreach (char letter in sentence.ToCharArray())
        {
            dialogueText.text += letter;
            yield return null;
        }
    }

    void EndDialogue()
    {
        dialoguePrefab.SetActive(false);
        isDialogueActive = false;
    }
}
