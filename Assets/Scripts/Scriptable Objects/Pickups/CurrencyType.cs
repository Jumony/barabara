using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewCurrencyType", menuName = "Currency")]
public class CurrencyType : ScriptableObject
{
    public string currencyName;
    public Sprite currencyIcon;
}
