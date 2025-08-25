using System.Collections;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private SpawnEnemy spawnEnemy;
    [HideInInspector] public int _destroyedEnemies;
    private bool isTeleported;
    private GameObject secretRoom;

    private void Start()
    {
        secretRoom = GameObject.FindGameObjectWithTag("SecretRoom");
        spawnEnemy = FindFirstObjectByType<SpawnEnemy>();

        isTeleported = false;
    }

    private void FixedUpdate()
    {
        if (!isTeleported && _destroyedEnemies >= spawnEnemy._maxEnemies)
        {
            TeleportToSecretRoom();
        }
    }

    private void TeleportToSecretRoom()
    {
        isTeleported = true;

        gameObject.transform.position = secretRoom.transform.position;
    }
}