using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DialogueLines
{
    public string speakerName;
    public Sprite speakerIcon;
    [TextArea(3,10)]
    public string text;
}
