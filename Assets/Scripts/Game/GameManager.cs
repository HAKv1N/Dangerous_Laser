using UnityEngine;

public class GameManager : MonoBehaviour
{
    private SpawnEnemy spawnEnemy;
    [HideInInspector] public int _destroyedEnemies;
    private bool isTeleported;

    private void Start()
    {
        spawnEnemy = FindFirstObjectByType<SpawnEnemy>();

        isTeleported = false;
    }

    private void Update()
    {
        if (!isTeleported && _destroyedEnemies >= spawnEnemy._maxEnemies)
        {
            TeleportToSecretRoom();
        }
    }

    private void TeleportToSecretRoom()
    {
        GameObject secretRoom = GameObject.FindGameObjectWithTag("SecretRoom");
        transform.localPosition = secretRoom.transform.position;
        
        isTeleported = true;
    }
}