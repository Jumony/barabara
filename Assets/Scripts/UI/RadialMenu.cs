using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RadialMenu : MonoBehaviour
{
    public List<PlayerWeaponType> items = new List<PlayerWeaponType>();
    public GameObject buttonPrefab;
    public PlayerController playerController;
    [Tooltip("Should be a child or within the same Canvas")]
    public Transform centerPoint;
    public float radius = 100f;

    // Start is called before the first frame update
    void Start()
    {
        PopulateRadialMenu();
    }

    public void PopulateRadialMenu()
    {
        // Places weapons in the radial menu
        float angleStep = 360f / items.Count;
        for (int i = 0; i < items.Count; i++)
        {
            // Calculates where the buttons should be
            float angle = i * angleStep;
            float posX = centerPoint.position.x + Mathf.Cos(angle * Mathf.Deg2Rad) * radius;
            float posY = centerPoint.position.y + Mathf.Sin(angle * Mathf.Deg2Rad) * radius;

            // Creates the button and replaces the image of the BUTTON with the weapon
            GameObject button = Instantiate(buttonPrefab, new Vector2(posX, posY), Quaternion.identity, centerPoint);
            button.GetComponent<Image>().sprite = items[i].sprite;

            // Adds a listener to tell us when the buttons is clicked
            int index = i;
            button.GetComponent<Button>().onClick.AddListener(() => OnItemClicked(index));
        }
    }

    public void AddGunToRadialInventory(PlayerWeaponType playerWeaponType)
    {
        items.Add(playerWeaponType);
    }

    void OnItemClicked(int index)
    {
        
        Debug.Log("Clicked on Item: " + items[index].weaponID);

        // THE PLUS 1 REPRESENTS THE FIST WHICH WE ARE HIDING FROM THE PLAYER
        playerController.SwitchWeapon(index + 1);
    }
}
