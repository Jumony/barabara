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

}
