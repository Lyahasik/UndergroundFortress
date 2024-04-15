using System;
using System.Collections.Generic;

namespace UndergroundFortress.Gameplay.StaticData
{
    [Serializable]
    public class DungeonLevelStaticData
    {
        public int id;
        public int numberStages;
        public List<SpawnEnemyStaticData> enemies;
    }
}