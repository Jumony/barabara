using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    public GameObject dialoguePrefab;
    [Header("Dialogue Info")]
    public TextMeshProUGUI speakerName;
    public TextMeshProUGUI dialogueText;
    public Image speakerImage;

    [Header("Parameters")]
    [Tooltip("Smaller = Faster")]
    public float dialogueSpeed;

    private Queue<DialogueLines> dialogueLines;
    private bool isDialogueActive = false;

    private void Start()
    {
        dialogueLines = new Queue<DialogueLines>();
        dialoguePrefab.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && isDialogueActive)
        {
            DisplayNextLine();
        }
    }

    // This is meant to be called (by triggers, etc.)
    // Cleans up everything, fills queue with dialogue and starts with first line
    public void StartDialogue(Dialogue dialogue)
    {
        dialoguePrefab.SetActive(true);
        dialogueLines.Clear();

        speakerImage.sprite = dialogue.lines[0].speakerIcon;

        foreach (DialogueLines line in dialogue.lines)
        {
            dialogueLines.Enqueue(line);
        }

        isDialogueActive = true;
        DisplayNextLine();
    }

    // Loads sprite and text to be displayed
    public void DisplayNextLine()
    {
        // Ends dialogue if there are no more lines
        if (dialogueLines.Count == 0)
        {
            EndDialogue();
            return;
        }

        DialogueLines line = dialogueLines.Dequeue();

        speakerImage.sprite = line.speakerIcon;
        speakerName.text = line.speakerName;
        StopAllCoroutines(); // Only stops coroutines running in this instance of DialogueManager
        StartCoroutine(TypeSentence(line.text));
    }

    IEnumerator TypeSentence(string sentence)
    {
        dialogueText.text = "";
        foreach (char letter in sentence.ToCharArray())
        {
            dialogueText.text += letter;
            yield return new WaitForSeconds(dialogueSpeed);
        }
    }

    void EndDialogue()
    {
        dialoguePrefab.SetActive(false);
        isDialogueActive = false;
    }
}
