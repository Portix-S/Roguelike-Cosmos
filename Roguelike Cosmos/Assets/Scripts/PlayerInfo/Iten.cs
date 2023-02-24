using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Iten
{
    [CreateAssetMenu(fileName = "Iten", menuName = "Scriptable Objects/Iten")]
    public class Iten : ScriptableObject
    {
        [Header("Iten Status modifier")]
        public int healthPoints;
        public int armor;
        public int magicResistence;
        public int inteligence;
        public int agility;
        public float attackDamage;
        public float moveSpeed;
        public float lucky;

        [Header("Iten State")]
        public ScriptableObject state;

        [Header("Iten Passive skill")]
        public ScriptableObject passive;

        [Header("Iten Active Skill")]
        public ScriptableObject skill;

    }
}