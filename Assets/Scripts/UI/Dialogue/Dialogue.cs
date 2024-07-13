using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewDialogue", menuName = "Dialogue/Dialogue")]
public class Dialogue : ScriptableObject
{
    // The reason we are using a list of DialogueLines rather than
    //  just having the lines, sprites, etc. is because these
    //  dialogues have a lot of information (images, names, text)
    //  so we need to have an organized way to traverse these things.
    // This method allows the queue to easily obtian information
    //  while traversing the array.
    public DialogueLines[] lines;
}
