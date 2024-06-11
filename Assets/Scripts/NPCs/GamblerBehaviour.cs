using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GamblerBehaviour : MonoBehaviour
{
    public List<GameObject> GambleableItems;
    private PlayerController playerController;
    private CurrencyManager currencyManager;
    public CurrencyType currencyType;
    private bool inRange;

    // Start is called before the first frame update
    void Start()
    {
        currencyManager = CurrencyManager.Instance;
        playerController = GameObject.Find("Player").GetComponent<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (inRange && Input.GetKeyDown(KeyCode.E) && currencyManager.GetBalance(currencyType) >= 5)
        {
            currencyManager.SpendCurrency(currencyType, 5);
            Gamble();
        }

        if (Input.GetKeyDown(KeyCode.P))
        {
            currencyManager.AddCurrency(currencyType, 100);
        }
    }

    void Gamble()
    {
        // Choose a random weapon instance from the list of gambleable items
        GameObject randomWeaponInstance = GambleableItems[Random.Range(0, GambleableItems.Count)];

        // Check if the random weapon instance is already in the player's inventory
        foreach (GameObject weaponInstance in playerController.weapons)
        {
            string randomWeaponInstanceName = randomWeaponInstance.name + "(Clone)";
            if (weaponInstance.name == randomWeaponInstanceName)
            {
                Debug.Log("Found duplicate item, refunding some money");
                currencyManager.AddCurrency(currencyType, 2);
                return;
            }
        }

        Debug.Log("Found a unique weapon, adding it to inventory");
        playerController.PickUpWeapon(randomWeaponInstance);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name == "Player")
        {
            inRange = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.name == "Player")
        {
            inRange = false;
        }
    }
}
