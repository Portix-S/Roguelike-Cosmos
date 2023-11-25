using System.Collections.Generic;
using UnityEngine;
using RewardType;

[CreateAssetMenu(fileName = "RewardPool", menuName = "Scriptable Objects/RewardPool")]
public class RewardPool : ScriptableObject
{
    public Reward[] pool;

    [System.Serializable]
    public class Reward
    {
        public RewardTypeData r;
        public int count;
    }

    public RewardTypeData GetRandomReward()
    {
        int index;
        int safeExit = 0;

        while (true)
        {
            index = Random.Range(0, pool.Length);
            Debug.Log(index);
            if ((pool[index].count > 0) || (safeExit == 3)) break;
            safeExit += 1;
        }

        pool[index].count -= 1;
        return pool[index].r;
    }
}