using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomizationManager : MonoBehaviour
{
    public CustomizationDict customizationDictScript;
    public GameObject shirtObject;
    public GameObject bodyObject;
    public GameObject hatObject;

    private Dictionary<CustomizationEnum.Category, int> currentIndices;
    private Dictionary<CustomizationEnum.Category, List<CustomizationSO>> customizationDict;
    private SpriteRenderer spriteRenderer;

    // Start is called before the first frame update
    void Start()
    {
        currentIndices = new Dictionary<CustomizationEnum.Category, int>();
        customizationDict = customizationDictScript.GetCustomizationDictionary();

        foreach (CustomizationEnum.Category category in currentIndices.Keys)
        {
            currentIndices[category] = 0;
        }

    }

    private void ApplyCustomization(CustomizationEnum.Category category)
    {
        if (customizationDict.ContainsKey(category) && currentIndices.ContainsKey(category))
        {
            List<CustomizationSO> options = customizationDict[category];
            int currentIndex = currentIndices[category];

            CustomizationSO currentOption = options[currentIndex];

            switch (category)
            {
                case CustomizationEnum.Category.Shirt:
                    spriteRenderer = shirtObject.GetComponent<SpriteRenderer>();
                    spriteRenderer.sprite = currentOption.sprite;

                    shirtObject.transform.position = currentOption.positionOffset;
                    Debug.Log("shirt");
                    break;

                case CustomizationEnum.Category.Body:
                    spriteRenderer = bodyObject.GetComponent<SpriteRenderer>();
                    spriteRenderer.sprite = currentOption.sprite;

                    bodyObject.transform.position = currentOption.positionOffset;
                    Debug.Log("Body");
                    break;

                case CustomizationEnum.Category.Hat:
                    spriteRenderer = hatObject.GetComponent<SpriteRenderer>();
                    spriteRenderer.sprite = currentOption.sprite;

                    hatObject.transform.position = currentOption.positionOffset;
                    Debug.Log("Hat");
                    break;

                default:
                    Debug.LogError("Cannot Find Valid Category for Customization Option\nConsider Adding a new Customization Category and adding it to this script");
                    break;
            }
        }

        else
        {
            Debug.LogError("Category not found in customization dictionary or current indices");
        }
    }
}
