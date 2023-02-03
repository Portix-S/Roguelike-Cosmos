using UnityEngine;

namespace Player
{
    [CreateAssetMenu(fileName = "Player", menuName = "Scriptable Objects/Player")]
    public class PlayerData : ScriptableObject
    {
        public float attackDamage;
        public int healthPoints;
    }
}

