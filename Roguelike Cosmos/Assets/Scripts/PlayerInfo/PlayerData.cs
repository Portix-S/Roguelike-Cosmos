using Unity.VisualScripting;
using UnityEngine;

namespace Player
{

    [CreateAssetMenu(fileName = "Player", menuName = "Scriptable Objects/Player")]

    public class PlayerData : ScriptableObject
    {
        public PlayerModifiers[] modifier;

        [Header("life and Defense")]
        public int baseHealthPoints;//base + constituicao
        public int baseArmor;
        public int baseMagicResistence;
        public int baseDodge;//base + DEX

        [Header("Attack and Damage")]
        public float baseAttackSpeed;
        public float baseAttackDamage;
        public float baseAttackRange;
        public float baseCoolDown;//modificarodr de tempo de recarga das skills


        [Header("Other")]
        public float baseMoveSpeed;//base + DEX


        ////////////////////////////////////////////////////////////////////////// valores baseados nos modificadores

        public float HealthPoints
        {
            get
            {
                float hp = baseHealthPoints;
                foreach (PlayerModifiers v in modifier)
                {
                    if (v.stat == PlayerModifier.Constitution)
                        hp += v.value * 1.5f;
                }
                return hp;
            }
        }

        public float Armor
        {
            get
            {
                float hp = baseArmor;
                foreach (PlayerModifiers v in modifier)
                {
                    if (v.stat == PlayerModifier.Constitution)
                        hp += v.value * 0.2f;
                    if (v.stat == PlayerModifier.Agility)
                        hp += v.value * 0.1f;
                }
                return hp;
            }
        }

        public float MagicResistence
        {
            get
            {
                float hp = baseMagicResistence;
                foreach (PlayerModifiers v in modifier)
                {
                    if (v.stat == PlayerModifier.Constitution)
                        hp += v.value * 0.1f;
                    if (v.stat == PlayerModifier.Wisdom)
                        hp += v.value * 0.2f;
                    if (v.stat == PlayerModifier.Intelligence)
                        hp += v.value * 0.1f;


                }
                return hp;
            }
        }
        public float Dodge
        {
            get
            {
                float dg = baseDodge;
                foreach (PlayerModifiers v in modifier)
                {
                    if (v.stat == PlayerModifier.Agility)
                        dg += v.value * 0.1f;
                }
                return dg;
            }
            
        }

        public float AttackSpeed
        {
            get
            {
                float dg = baseAttackSpeed;
                foreach (PlayerModifiers v in modifier)
                {
                    if (v.stat == PlayerModifier.Agility)
                        dg += v.value * 0.1f;
                }
                return dg;
            }

        }

        public float AttackDamage
        {
            get
            {
                float d = baseAttackDamage;
                foreach (PlayerModifiers v in modifier)
                {
                    if (v.stat == PlayerModifier.Agility)
                        d += v.value * 0.75f;
                    if (v.stat == PlayerModifier.Strength)
                        d += v.value * 1.5f;
                }
                return d;
            }
        }

        public float CoolDownReduct
        {
            get
            {
                float d = baseCoolDown;
                foreach (PlayerModifiers v in modifier)
                {
                    if (v.stat == PlayerModifier.Intelligence)
                        d += v.value* 0.2f;
                    if (v.stat == PlayerModifier.Wisdom)
                        d += v.value* 0.1f;
                }
                return d;
            }
        }

        public float MoveSpeed
        {
            get
            {
                float d = baseMoveSpeed;
                foreach (PlayerModifiers v in modifier)
                {
                    if (v.stat == PlayerModifier.Agility)
                        d += v.value * 0.1f;
                }
                return d;
            }
        }

        public ItemInventory[] inventory;
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

