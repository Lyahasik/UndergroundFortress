using System.Collections.Generic;
using UnityEngine;

using UndergroundFortress.Gameplay.Stats;

namespace UndergroundFortress.Gameplay.StaticData
{
    [CreateAssetMenu(fileName = "EquipmentData", menuName = "Static data/Equipment")]
    public class EquipmentStaticData : ScriptableObject
    {
        public int id;
        
        [Space]
        public int maxLevel;
        public string name;
        public Sprite icon;
        
        [Space]
        public StatType typeStat;
        public List<QualityValue> qualityValues;
    }
}