using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    public enum WaveState { 
        SPAWNING, // Spawnando inimigos da wave
        WAITING, // Aguardando o player matar todos
        DELAYING, // Atraso antes da pr�xima wave
        COMPLETED, // Wave foi conclu�da
        ENDED // Acabaram as waves
    };

    [SerializeField]
    public List<Transform> positions = new List<Transform>();

    public RewardManager rm;

    public float TimeBetweenWaves;

    public Wave[] waves;

    public int currentWave;

    public WaveState currentState;
    bool hasRewarded;
    [SerializeField] GameObject endDoor;
    void Start()
    {
        rm = gameObject.GetComponentInChildren<RewardManager>();
        //currentState = WaveState.SPAWNING;
        //currentWave = 0;
    }

    public void Restart()
    {
        currentState = WaveState.SPAWNING;
        currentWave = 0;
        hasRewarded = false;
    }

    public void KillAllEnemies()
    {
        foreach (GameObject enemy in waves[currentWave].spawnedEnemies)
        {
            Destroy(enemy);
        }
        waves[currentWave].spawnedEnemies.Clear();
        this.enabled = false;
    }

    void Update()
    {
        //Debug.Log("State: " + currentState);
        if (currentState == WaveState.ENDED) 
        {
            endDoor.GetComponent<Animator>().enabled = true;
            return;
        }
        if (currentState == WaveState.COMPLETED)
        {
            if (currentWave == waves.Length - 1 && !hasRewarded)
            {
                hasRewarded = true;
                currentState = WaveState.ENDED;
                rm.ReleaseReward();
                return;
            }

            currentWave += 1;
            currentState = WaveState.SPAWNING;
        }
        else if (currentState == WaveState.WAITING)
        {
            for(int i = 0; i < waves[currentWave].spawnedEnemies.Count; i++)
            {
                if (!waves[currentWave].spawnedEnemies[i])
                    waves[currentWave].spawnedEnemies.RemoveAt(i);
            }
            
            if (waves[currentWave].spawnedEnemies.Count == 0)
            {
                if (!hasRewarded)
                {
                    hasRewarded = true;
                    rm.ReleaseReward();
                }
                StartCoroutine(Wait());
            }
        }
        else if (currentState == WaveState.SPAWNING)
        {
            int posIndex = 0;
            // Spawnar os inimigos
            for (int i = 0; i < waves[currentWave].waveEnemy.Length; i++)
            {
                // Para cada tipo de inimigo
                for (int j = 0; j < waves[currentWave].waveEnemy[i].count; j++)
                {

                    GameObject enemy = Instantiate(waves[currentWave].waveEnemy[i].enemy,
                        positions[posIndex].position, Quaternion.identity);
                    waves[currentWave].spawnedEnemies.Add(enemy);

                    posIndex += 1;
                    if (posIndex == positions.Count)
                        posIndex = 0;
                }

            }
            currentState = WaveState.WAITING;
        }
    }


    [System.Serializable]
    public class Wave
    {
        public WaveEnemy[] waveEnemy; // lista de inimigos da wave
        public List<GameObject> spawnedEnemies = new List<GameObject>(); // inimigos em campo
    }

    [System.Serializable]
    public class WaveEnemy
    {
        public GameObject enemy; // prefab
        public int count; // quantidade a ser spawnada
    }

    IEnumerator Wait()
    {
        currentState = WaveState.DELAYING;
        yield return new WaitForSeconds(TimeBetweenWaves);
        hasRewarded = false;
        currentState = WaveState.COMPLETED;
    }
}
