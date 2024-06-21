using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponPickup : MonoBehaviour
{
    public GameObject weaponPrefab;
    public PlayerWeaponType weaponType;
    private PlayerController playerController;
    private CurrencyManager currencyManager;
    public CurrencyType currencyType;
    public RadialMenu radialMenu;
    private bool playerInRange;

    private void Start()
    {
        currencyManager = CurrencyManager.Instance;
        playerController = GameObject.Find("Player").GetComponent<PlayerController>();
    }

    private void Update()
    {
        if (playerInRange && Input.GetKeyDown(KeyCode.E) && currencyManager.GetBalance(currencyType) >= weaponType.price)
        {
            // Checks to see if player already has weapon
            foreach (GameObject weapon in playerController.weapons)
            {
                IGunBehaviour weaponBehaviour = weapon.GetComponent<IGunBehaviour>();
                if (weaponBehaviour != null && weaponBehaviour.GetWeaponType().weaponID == weaponType.weaponID)
                {
                    Debug.Log("You already have this in your inventory. Ignoring");
                    return;
                }
            }
            HandleTransaction();
            HandleAchievement();

            radialMenu.AddGunToRadialInventory(weaponType);
            radialMenu.PopulateRadialMenu();

            playerController.PickUpWeapon(weaponPrefab);
            Destroy(gameObject);
        }
    }

    private void HandleTransaction()
    {
        Debug.Log("Enough Money. Purchasing");
        currencyManager.SpendCurrency(currencyType, weaponType.price);
    }

    private void HandleAchievement()
    {
        if (weaponPrefab.gameObject.name == "Gun")
            GameManager.Instance.playerProgression.hasUnlockedRevolver = true;
        if (weaponPrefab.gameObject.name == "Shotgun")
            GameManager.Instance.playerProgression.hasUnlockedShotgun = true;
        GameManager.Instance.SaveProgress();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name == "Interact Trigger")
        {
            playerInRange = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.name == "Interact Trigger")
        {
            playerInRange = false;
        }
    }
}
