using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CurrencyManager : MonoBehaviour
{
    public static CurrencyManager Instance;
    private Dictionary<CurrencyType, int> currencyBalances;
    public TextMeshProUGUI coinText;
    public CurrencyType coin;

    private int coinBalance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            currencyBalances = new Dictionary<CurrencyType, int>();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        coinBalance = PlayerPrefs.GetInt("Coin", 0);

        if (!currencyBalances.ContainsKey(coin))
        {
            currencyBalances[coin] = coinBalance;
        }

        coinText.text = coinBalance.ToString();
        LoadCurrency();
    }

    public int GetBalance(CurrencyType currencyType)
    {
        return currencyBalances[currencyType];
    }

    public void AddCurrency(CurrencyType currencyType, int amount)
    {
        if (!currencyBalances.ContainsKey(currencyType))
        {
            currencyBalances[currencyType] = coinBalance;
        }
        currencyBalances[currencyType] += amount;



       //Debug.Log("New balance is: " + currencyBalances[currencyType]);
        coinText.text = currencyBalances[currencyType].ToString();
        SaveCurrency();
    }

    public void SpendCurrency(CurrencyType currencyType, int amount)
    {
        if (currencyBalances.ContainsKey(currencyType) && currencyBalances[currencyType] >= amount)
        {
            currencyBalances[currencyType] -= amount;
        }
        else
        {
            Debug.LogWarning("Not enough currency");
        }

        coinText.text = currencyBalances[currencyType].ToString();
        SaveCurrency();
    }

    private void SaveCurrency()
    {
        foreach (var currency in currencyBalances)
        {
            PlayerPrefs.SetInt(currency.Key.currencyName, currency.Value);
        }
        PlayerPrefs.Save();
    }

    private void LoadCurrency()
    {
        // Load all CurrencyType objects from the Resources/Currency folder
        CurrencyType[] currencies = Resources.LoadAll<CurrencyType>("Currency");

        // Check if any CurrencyType objects were found
        if (currencies.Length == 0)
        {
            Debug.LogWarning("No currency types found in Resources/Currency");
        }
        else
        {
            //Debug.Log($"Found {currencies.Length} currency types in Resources/Currency");
        }

        // Iterate over each CurrencyType object and load its balance from PlayerPrefs
        foreach (CurrencyType currencyType in currencies)
        {

            int balance = PlayerPrefs.GetInt(currencyType.currencyName, 0);
            currencyBalances[currencyType] = balance;
            //Debug.Log($"Loaded currency type: {currencyType.currencyName} with balance: {balance}");
        }
    }

    private void OnApplicationQuit()
    {
        SaveCurrency();
    }
}
