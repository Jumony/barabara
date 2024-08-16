using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Customization", menuName = "Customization Option")]
public class CustomizationSO : ScriptableObject
{
    public enum Category { Hat, Shirt, Body };

    public Category category;
    public string optionName;
    public Sprite sprite;
    public Vector3 positionOffset;
}
