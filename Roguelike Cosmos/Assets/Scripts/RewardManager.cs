using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RewardManager : MonoBehaviour
{
    private Vector3 position;

    [SerializeField]
    public List<GameObject> rewards;
    public List<float> prob;
    public void ReleaseReward()
    {
        if (prob.Count > rewards.Count) 
        { 
            while(prob.Count > rewards.Count)
            {
                prob.RemoveAt(0);
            }
        } 
        else if(prob.Count > rewards.Count)
        {
            while (prob.Count > rewards.Count)
            {
                prob.Add(prob[0]);
            }
        }

        prob.Sort();
        float sum = 0;
        foreach(float value in prob)
        {
            sum += value;
            Debug.Log(value);
        }
           



        float randValue = Random.Range(0, sum);

        sum = 0;
        for(int i = 0; i<prob.Count; i++)
        {
            sum += prob[i];
            Debug.Log("Soma: "+sum);

            Debug.Log("Rand: " + randValue);
            if (randValue <= sum)
            {
                if(rewards[i] != null)
                    Instantiate(rewards[i], position, Quaternion.identity);
                break;
            }
        }

    }

    public void SetRewardLocation(Vector3 local)
    {
        position = local;
    }
}
