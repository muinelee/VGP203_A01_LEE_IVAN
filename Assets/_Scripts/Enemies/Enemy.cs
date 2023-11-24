using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour, IDamageable
{
    public event Action<Enemy> OnDeath;

    [SerializeField] private float maxHealth = 3.0f;
    private float currentHealth;
    private EnemyAnimationHandler eah;

    private void Start()
    {
        currentHealth = maxHealth;
        eah = GetComponentInChildren<EnemyAnimationHandler>();
        if(eah == null)
        {
            Debug.LogError("No EnemyAnimationHandler component found on this GameObject.");
        }
    }

    public void Damage(float damageAmount)
    {
        currentHealth -= damageAmount;
        eah.PlayHitAnimation();
        Debug.Log("Enemy damaged for " + damageAmount + " damage. Current health: " + currentHealth + ".");
        if (currentHealth <= 0.0f)
        {
            OnDeath?.Invoke(this);
            Destroy(gameObject);
        }
    }
}
