using UnityEngine;

using UndergroundFortress.Scripts.Gameplay.Stats;

namespace UndergroundFortress.Scripts.Gameplay.StaticData
{
    [CreateAssetMenu(fileName = "EnemyData", menuName = "Static data/Enemy")]
    public class EnemyStaticData : ScriptableObject
    {
        public MainStats mainStats;
    }
}