﻿using System.Collections.Generic;
using UnityEngine;

namespace UndergroundFortress.Gameplay.StaticData
{
    [CreateAssetMenu(fileName = "Dungeon_Data", menuName = "Static data/Dungeon")]
    public class DungeonStaticData : ScriptableObject
    {
        public int id;
        public string name;

        [Space]
        public List<DungeonLevelStaticData> levels;
    }
}