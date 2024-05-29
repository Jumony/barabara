using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTriggers : MonoBehaviour
{
    private CurrencyManager currencyManager;
    public CurrencyType coin;
    private void Start()
    {
        currencyManager = CurrencyManager.Instance;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Coin"))
        {
            currencyManager.AddCurrency(coin, 1);
            Debug.Log(currencyManager.GetBalance(coin));
            collision.gameObject.SetActive(false);
        }

    }
}
