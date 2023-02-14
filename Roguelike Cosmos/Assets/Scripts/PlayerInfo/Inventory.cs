using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Inventory
{
    [CreateAssetMenu(fileName = "Inventory", menuName = "Scriptable Objects/Inventory")]
    public class Inventory : ScriptableObject
    {
        public ScriptableObject slot_1;
        public ScriptableObject slot_2;
        public ScriptableObject slot_3;
        public ScriptableObject slot_4;
        public ScriptableObject slot_5;
        public ScriptableObject slot_6;
    }
}