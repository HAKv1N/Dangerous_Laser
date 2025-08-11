using System.Collections.Generic;
using UnityEngine;

public class SpawnEnemy : MonoBehaviour
{
    [Header("Objects")]
    [SerializeField] private GameObject enemyPrefab;

    private List<Transform> spawns = new List<Transform>();
    public int _maxEnemies = 15;
    private int _currentEnemies;

    private void Start()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            spawns.Add(transform.GetChild(i).transform);
        }

        MixxingSpawns();

        foreach (var spawn in spawns)
        {
            if (_currentEnemies >= _maxEnemies) return;

            GameObject newEnemy = Instantiate(enemyPrefab, spawn.position, Quaternion.identity, spawn);
            ApplyDifficultyToEnemy(newEnemy);
            _currentEnemies++;
        }
    }

    private void MixxingSpawns()
    {
        for (int i = spawns.Count - 1; i > 0; i--)
        {
            int randomIndex = Random.Range(0, i + 1);

            Transform temp = spawns[i];
            spawns[i] = spawns[randomIndex];
            spawns[randomIndex] = temp;
        }
    }

    private void ApplyDifficultyToEnemy(GameObject enemy)
    {
        EnemyInfo enemyInfo = enemy.GetComponent<EnemyInfo>();

        if (enemyInfo != null)
        {
            switch (GameSettings.DifficultyLevel)
            {
                case -1:
                    SetEnemyStats(enemyInfo, 0, 0, 0, 5, 0, false);
                    break;
                case 0:
                    SetEnemyStats(enemyInfo, 5, 1, 0.5f, 5, 5, true);
                    break;
                case 1:
                    SetEnemyStats(enemyInfo, 10, 2.5f, 0.4f, 10, 4.2f, true);
                    break;
                case 2:
                    SetEnemyStats(enemyInfo, 20, 5, 0.3f, 15, 3.9f, true);
                    break;
                case 3:
                    SetEnemyStats(enemyInfo, 25, 8, 0.15f, 20, 3.6f, true);
                    break;
                case 4:
                    SetEnemyStats(enemyInfo, 30, 10, 0.1f, 25, 3, true);
                    break;
            }
        }
    }

    private void SetEnemyStats(EnemyInfo enemyInfo, int ammo, float damage, float fireRate, float rangeAttack, float reloadRate, bool isAgressive)
    {
        enemyInfo._maxAmmo = ammo;
        enemyInfo._currentAmmo = ammo;
        enemyInfo._damage = damage;
        enemyInfo._fireRate = fireRate;
        enemyInfo._rangeAttack = rangeAttack;
        enemyInfo._reloadRate = reloadRate;
        enemyInfo._agressive = isAgressive;
    }
}

public static class GameSettings
{
    public static int DifficultyLevel;
}