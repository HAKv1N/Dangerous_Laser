using System.Collections.Generic;
using UnityEngine;

public class ChangeDifficulty : MonoBehaviour
{
    [SerializeField] private List<GameObject> enemies = new List<GameObject>();

    private void Start()
    {
        EnemyFunctional[] enemyScripts = FindObjectsByType<EnemyFunctional>(FindObjectsSortMode.None);

        foreach (EnemyFunctional enemy in enemyScripts)
        {
            enemies.Add(enemy.gameObject);
        }

        ApplyDifficulty(GameSettings.DifficultyLevel);
    }

    private void ApplyDifficulty(int difficulty)
    {
        switch (difficulty)
        {
            case -1:
                ApplyEnemySettings(0, 0, 0, 5, 0, false);
                break;

            case 0:
                ApplyEnemySettings(5, 1, 0.5f, 5, 5, true);
                break;

            case 1:
                ApplyEnemySettings(10, 2.5f, 0.4f, 10, 4.2f, true);
                break;

            case 2:
                ApplyEnemySettings(20, 5, 0.3f, 15, 3.9f, true);
                break;

            case 3:
                ApplyEnemySettings(25, 8, 0.15f, 20, 3.6f, true);
                break;
        }
    }

    private void ApplyEnemySettings(int ammo, float damage, float fireRate, float rangeAttack, float reloadRate, bool isAgressive)
    {
        for (int i = 0; i < enemies.Count; i++)
        {
            EnemyInfo enemyInfo = enemies[i].GetComponent<EnemyInfo>();
            if (enemyInfo != null)
            {
                enemyInfo._currentAmmo = ammo;
                enemyInfo._damage = damage;
                enemyInfo._fireRate = fireRate;
                enemyInfo._rangeAttack = rangeAttack;
                enemyInfo._reloadRate = reloadRate;
                enemyInfo._agressive = isAgressive;
            }
        }
    }
}

public static class GameSettings
{
    public static int DifficultyLevel { get; set; } = 0;
}