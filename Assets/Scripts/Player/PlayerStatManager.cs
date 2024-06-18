using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatManager : MonoBehaviour
{
    public float maxHealth = 100;
    public float currentHealth;

    private void Start()
    {
        currentHealth = maxHealth;    
    }

    public void TakeDamage(float damage)
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
