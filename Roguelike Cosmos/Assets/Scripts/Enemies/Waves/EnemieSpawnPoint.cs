using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemieSpawnPoint : MonoBehaviour
{
    public List<GameObject> enemies;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.H)) Spawn();
    }

    public GameObject Spawn() 
    {
        if (enemies.Count == 0) return null;

        int index = Random.Range(0, enemies.Count);

        GameObject enemie = Instantiate(enemies[index], transform.position, Quaternion.identity);

        return enemie;
    }
}
