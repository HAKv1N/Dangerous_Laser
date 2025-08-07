using System.Collections.Generic;
using UnityEngine;

public class SpawnPlayer : MonoBehaviour
{
    [Header("Player")]
    [SerializeField] private GameObject player;
    
    private List<Transform> spawns = new List<Transform>();

    private void Start()
    {
        Spawn();
    }

    private void Spawn()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            spawns.Add(transform.GetChild(i));
        }

        Transform randomSpawn = spawns[Random.Range(0, spawns.Count)];

        Instantiate(player, randomSpawn.position, Quaternion.identity);
    }
}