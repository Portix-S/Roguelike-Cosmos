using System.Collections.Generic;
using UnityEngine;
using Player;

namespace RewardType
{
    [CreateAssetMenu(fileName = "RewardType", menuName = "Scriptable Objects/RewardType")]
    public class RewardTypeData : ScriptableObject
    {
        [SerializeField]
        private PlayerData pData;

        [Header("Text")]
        public string title;
        public List<string> descriptions;

        //[Header("INCREASE STATS")]
        //public pData.PlayerModifiers[] modifier;


    }
}

