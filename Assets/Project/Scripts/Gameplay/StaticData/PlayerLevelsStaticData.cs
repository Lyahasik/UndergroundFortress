using System.Collections.Generic;
using UnityEngine;

namespace UndergroundFortress.Gameplay.StaticData
{
    [CreateAssetMenu(fileName = "PlayerLevelsData", menuName = "Static data/Player/LevelsData")]
    public class PlayerLevelsStaticData : ScriptableObject
    {
        public List<PlayerLevelStaticData> levelsData;
    }
}