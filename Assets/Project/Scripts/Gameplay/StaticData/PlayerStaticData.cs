using UnityEngine;

using UndergroundFortress.Scripts.Gameplay.Stats;

namespace UndergroundFortress.Scripts.Gameplay.StaticData
{
    [CreateAssetMenu(fileName = "PlayerData", menuName = "Static data/Player")]
    public class PlayerStaticData : ScriptableObject
    {
        public MainStats mainStats;
    }
}