using System.Collections.Generic;
using UnityEngine;

using UndergroundFortress.Gameplay.Stats;

namespace UndergroundFortress.Gameplay.StaticData
{
    [CreateAssetMenu(fileName = "StatData", menuName = "Static data/Stat")]
    public class StatStaticData : ScriptableObject
    {
        public string keyName;
        public StatType type;
        public Sprite icon;
        public List<QualityValue> qualityValues;
    }
}