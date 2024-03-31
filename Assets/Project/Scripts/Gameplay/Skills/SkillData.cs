using System;
using System.Collections.Generic;
using UnityEngine;

using UndergroundFortress.Gameplay.Stats;

namespace UndergroundFortress.Gameplay.StaticData
{
    [Serializable]
    public class SkillData
    {
        public int id;
        public StatType statType;
        public float value;
        public string name;
        public string description;
        public Sprite icon;

        [Space]
        public List<SkillLevelData> levelsData;
    }
}