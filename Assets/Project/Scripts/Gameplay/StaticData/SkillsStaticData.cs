using System.Collections.Generic;
using UnityEngine;

namespace UndergroundFortress.Gameplay.StaticData
{
    [CreateAssetMenu(fileName = "SkillsData", menuName = "Static data/Player/Skills")]
    public class SkillsStaticData : ScriptableObject
    {
        public SkillsType type;
        public List<SkillData> skillsData;
    }
}