using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatManager : MonoBehaviour
{
    public int maxHealth = 100;
    public int currentHealth;

    private void Start()
    {
        currentHealth = maxHealth;    
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        Debug.Log("Took damage: Health is now " + currentHealth);
        if (currentHealth <= 0)
        {
            GameManager.Instance.GameOver();
            Debug.Log("GameOver: Player health reached 0");
        }
    }
}
