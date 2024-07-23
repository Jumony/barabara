using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GamblerHouseBehaviour : MonoBehaviour
{
    public BoxCollider2D boxCollider;
    public GameObject GamblersHouseExterior;

    private bool playerInBuilding = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Something Entered");
        if (collision.CompareTag("Player"))
        {
            playerInBuilding = !playerInBuilding;
            GamblersHouseExterior.SetActive(!GamblersHouseExterior.activeSelf);
        }
    }
}
