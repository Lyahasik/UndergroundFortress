using System.Collections.Generic;
using UnityEngine;

namespace UndergroundFortress.Gameplay.StaticData
{
    [CreateAssetMenu(fileName = "SkillsData", menuName = "Static data/Skills")]
    public class SkillsStaticData : ScriptableObject
    {
        public SkillsType type;
        public List<SkillData> skillsData;
    }
}