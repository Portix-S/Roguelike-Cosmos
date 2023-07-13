using UnityEngine;

namespace Player
{
    [CreateAssetMenu(fileName = "Player", menuName = "Scriptable Objects/Player")]
    public class PlayerData : ScriptableObject
    {
        [Header("Player Status")]
        public int healthPoints;
        public int armor;
        public int magicResistence;
        public int inteligence;
        public int agility;
        public float attackDamage;
        public float moveSpeed;
        public float lucky;

        [Header("Player State")]
        public ScriptableObject stateHandler;

        [Header("Player Skills")]
        public ScriptableObject skillHandler;

        [Header("Player Inventory")]
        public ScriptableObject inventory;
    }
}

