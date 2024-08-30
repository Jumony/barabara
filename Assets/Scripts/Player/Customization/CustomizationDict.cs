using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomizationDict : MonoBehaviour
{
    public List<CustomizationEntry> customizationEntries;

    // When referencing any customization option, use the dictionary. NOT THE LIST
    // The dictionary to allow for quick references to the customization entries
    private Dictionary<CustomizationEnum.Category, List<CustomizationSO>> customizationDictionary;

    private void Awake()
    {
        customizationDictionary = new Dictionary<CustomizationEnum.Category, List<CustomizationSO>>();

        foreach (CustomizationEntry entry in customizationEntries)
        {
            if (!customizationDictionary.ContainsKey(entry.category))
            {
                customizationDictionary[entry.category] = new List<CustomizationSO>();
            }
            customizationDictionary[entry.category].Add(entry.customizationOption);
        }
    }
    
    public Dictionary<CustomizationEnum.Category, List<CustomizationSO>> GetCustomizationDictionary()
    {
        return customizationDictionary;
    }
}
