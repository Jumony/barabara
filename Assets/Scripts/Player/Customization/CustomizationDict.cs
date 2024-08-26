using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomizationDictionary : MonoBehaviour
{
    public List<CustomizationEntry> customizationEntries;

    // When referencing any customization option, use the dictionary. NOT THE LIST
    // The dictionary to allow for quick references to the customization entries
    private Dictionary<CustomizationEnum.Category, CustomizationSO> customizationDictionary;

    private void Awake()
    {
        customizationDictionary = new Dictionary<CustomizationEnum.Category, CustomizationSO>();

        foreach (CustomizationEntry entry in customizationEntries)
        {
            if (!customizationDictionary.ContainsKey(entry.category))
            {
                customizationDictionary.Add(entry.category, entry.customizationOption);
            }
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
