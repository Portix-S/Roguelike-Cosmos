using Unity.VisualScripting;
using UnityEngine;

namespace Player
{

    [CreateAssetMenu(fileName = "Player", menuName = "Scriptable Objects/Player")]

    public class PlayerData : ScriptableObject
    {
        [Header("life and Defense")]
        public int BaseHealthPoints;//base + constituicao
        public int armor;
        public int magicResistence;
        public int dodge;//base + DEX

        [Header("Attack and Damage")]
        public float attackSpeed;
        public float baseAttackDamage;
        public float attackRange;

        public float MagicDamage;//
        public float skillCoolDown;//modificarodr de tempo de recarga das skills
        

        [Header("Other")]
        public float moveSpeed;//base + DEX
        public float lucky;

        public int inteligence;


        public float AttackDamage
        {
            get
            {
                float d = 0f;
                foreach (PlayerModifiers v in modifier)
                {
                    if (v.stat == PlayerModifier.Constitution)
                        d += v.value * 0.7f;
                    if (v.stat == PlayerModifier.Strength)
                        d += v.value * 0.7f;
                    if (v.stat == PlayerModifier.Intelligence)
                        d += v.value * 0.7f;
                }
                return d;
            }
        }

        public PlayerStatisticValue[] statistics;

        public ItemInventory[] inventory;

        public PlayerModifiers[] modifier;



    }

    //moficador das estatisticas do player
    public enum PlayerStatistic
    {

    }

    [System.Serializable]
    public class PlayerStatisticValue
    {
        public PlayerStatistic stat;
        public int value;
    }

    //modificador simplificado de estatisticas do player, vulgo igual dnd 10 de strenght vira dano etc
    public enum PlayerModifier
    {
        Strength, Constitution, Agility, Intelligence, Wisdom,
    }

    [System.Serializable]
    public class PlayerModifiers
    {
        public PlayerModifier stat;
        public int value;
    }

    [System.Serializable]
    public class ItemInventory
    {
        public Item item;
    }
}

