using System.Collections.Generic;
using UnityEngine;

using UndergroundFortress.Scripts.Gameplay.Stats;

namespace UndergroundFortress.Scripts.Gameplay.StaticData
{
    [CreateAssetMenu(fileName = "ItemData", menuName = "Static data/Item")]
    public class ItemStaticData : ScriptableObject
    {
        public List<StatData> stats;
    }
}