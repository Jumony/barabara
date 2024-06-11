using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponPickup : MonoBehaviour, IInteractable
{
    public GameObject weaponPrefab;
    private PlayerController playerController;
    private bool playerInRange;

    private void Start()
    {
        playerController = GameObject.Find("Player").GetComponent<PlayerController>();
    }

    private void Update()
    {
        if (playerInRange && Input.GetKeyDown(KeyCode.E))
        {
            playerController.PickUpWeapon(weaponPrefab);
            Destroy(gameObject);
        }
    }

    public void Interact()
    {
        return;
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
