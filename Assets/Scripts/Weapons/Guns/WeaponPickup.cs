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
            HandleTransaction();
            HandleAchievement();
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
