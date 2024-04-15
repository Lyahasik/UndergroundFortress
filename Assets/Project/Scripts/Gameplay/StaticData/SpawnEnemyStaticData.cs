using System;

namespace UndergroundFortress.Gameplay.StaticData
{
    [Serializable]
    public class SpawnEnemyStaticData
    {
        public int probabilityWeight;
        public EnemyStaticData enemyStaticData;
    }
}