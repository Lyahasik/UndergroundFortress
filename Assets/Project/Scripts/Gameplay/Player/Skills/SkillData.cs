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
        public SkillStaticData data;

        [Space]
        public List<SkillLevelData> levelsData;
    }
}