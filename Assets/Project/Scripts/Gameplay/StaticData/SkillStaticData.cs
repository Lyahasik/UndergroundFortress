using UnityEngine;

using UndergroundFortress.Gameplay.Stats;

namespace UndergroundFortress.Gameplay.StaticData
{
    [CreateAssetMenu(fileName = "SkillData", menuName = "Static data/Player/Skill")]
    public class SkillStaticData : ScriptableObject
    {
        public StatType statType;
        public float value;
        public string name;
        public string description;
        public Sprite icon;
    }
}