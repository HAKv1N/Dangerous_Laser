using System.Collections.Generic;
using UnityEngine;

public class ChangeDifficulty : MonoBehaviour
{
    public static void UpdateAllEnemiesDifficulty()
    {
        EnemyFunctional[] enemyScripts = FindObjectsByType<EnemyFunctional>(FindObjectsSortMode.None);

        foreach (EnemyFunctional enemy in enemyScripts)
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
                }
            }
        }
    }

    private static void SetEnemyStats(EnemyInfo enemyInfo, int ammo, float damage, float fireRate, float rangeAttack, float reloadRate, bool isAgressive)
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