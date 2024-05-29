using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponPickup : MonoBehaviour
{
    public GameObject weaponPrefab;
    private PlayerController playerController;

    private void Start()
    {
        playerController = GameObject.Find("Player").GetComponent<PlayerController>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerController.PickUpWeapon(weaponPrefab);
            Destroy(gameObject);
        }
    }
}
