using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerProgression
{
    public bool hasUnlockedRevolver;
    public bool hasUnlockedShotgun;
    public bool hasGambled;

    public PlayerProgression()
    {
        hasUnlockedRevolver = false;
        hasUnlockedShotgun = false;
        hasGambled = false;
    }
}
