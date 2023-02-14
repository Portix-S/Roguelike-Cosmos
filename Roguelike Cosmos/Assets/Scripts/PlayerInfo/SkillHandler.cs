using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SkillHandler
{
    [CreateAssetMenu(fileName = "SkillHandler", menuName = "Scriptable Objects/SkillHandler")]
    public class SkillHandler : ScriptableObject
    {
        public ScriptableObject skill_1;
        public ScriptableObject skill_2;
        public ScriptableObject skill_3;
        public ScriptableObject dash;
        public ScriptableObject ultimate;
        public ScriptableObject passive;
    }
}