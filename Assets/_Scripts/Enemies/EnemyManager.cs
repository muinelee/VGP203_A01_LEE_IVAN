using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public event Action OnEnemyDefeated;
    public event Action OnAllEnemiesDefeated;
    public int RemainingEnemies => enemyList.Count;
    private List<Enemy> enemyList = new List<Enemy>();

    private MenuManager mm;

    // Start is called before the first frame update
    void Start()
    {
        mm = FindObjectOfType<MenuManager>();

        enemyList.AddRange(FindObjectsOfType<Enemy>());
        foreach (var enemy in enemyList)
        {
            enemy.OnDeath += HandleEnemyDeath;
        }
        mm.UpdateEnemies(enemyList.Count);
    }

    private void HandleEnemyDeath(Enemy enemy)
    {
        enemyList.Remove(enemy);
        mm.UpdateEnemies(enemyList.Count);
        if (enemyList.Count == 0)
        {
            OnAllEnemiesDefeated?.Invoke();
        }
    }
}
