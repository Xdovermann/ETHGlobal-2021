using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int currentHealth;
    public int maxHealth;
    private Target target;

    private bool hasDied = false;

    private void Awake()
    {
        currentHealth = maxHealth;
        target = GetComponent<Target>();
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;

        if(currentHealth <= 0 && !hasDied)
        {
            EnemyDied();
        }
    }

    public void EnemyDied()
    {
        hasDied = true;
        target.RemoveTarget();
    }
}
