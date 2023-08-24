using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    [SerializeField]
    public List<List<GameObject>> waves = new List<List<GameObject>>(); 
    public int wCounter;
    public bool wsCompleted = false;
    public RewardManager rm;
    void Start()
    {
        waves.Add(new List<GameObject>());
        waves.Add(new List<GameObject>());
        waves.Add(new List<GameObject>());
        wCounter = 0;
        rm = gameObject.GetComponentInChildren<RewardManager>();
    }

    // Update is called once per frame
    void Update()
    {
        Check();
    }

    void Check()
    {
        if (wsCompleted) return;

        if(waves[wCounter].Count == 0)
        {
            if (waves.Count < waves[wCounter].Count - 1)
                wCounter += 1;
            else
            {
                wsCompleted = true;
                rm.ReleaseReward();
            }
        }
        else
        {
            for(int i = 0; i< waves[wCounter].Count; i++)
            {
                if (waves[wCounter][i] == null) waves[wCounter].RemoveAt(i);
            }
        }
    }
}
