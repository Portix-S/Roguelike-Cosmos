using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Item", menuName = "Scriptable Objects/Item")]
public class Item : ScriptableObject
{
    [Header("Item Status modifier")]
    public int healthPoints;
    public int armor;
    public int magicResistence;
    public int inteligence;
    public int agility;
    public float attackDamage;
    public float moveSpeed;
    public float lucky;

    [Header("Item State")]
    public ScriptableObject state;

    [Header("Item Passive skill")]
    public ScriptableObject passive;

    [Header("Item Active Skill")]
    public ScriptableObject skill;

}
