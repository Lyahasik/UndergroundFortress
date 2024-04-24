using System.Collections.Generic;
using UnityEngine;

using UndergroundFortress.Constants;
using UndergroundFortress.Gameplay.Stats;

namespace UndergroundFortress.Gameplay.StaticData
{
    public class EquipmentStaticData : ItemStaticData
    {
        [Space]
        public int minLevel;
        public int maxLevel;
        public int statValuePerLevel;
        public string name;
        public int maxNumberForCell = ConstantValues.MIN_NUMBER_ITEM_FOR_CELL;
        
        [Space]
        public StatType typeStat;
        public List<QualityValue> qualityValues;
    }
}