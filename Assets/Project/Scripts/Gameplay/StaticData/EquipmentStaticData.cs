using System.Collections.Generic;
using UnityEngine;

using UndergroundFortress.Constants;
using UndergroundFortress.Gameplay.Items;
using UndergroundFortress.Gameplay.Stats;

namespace UndergroundFortress.Gameplay.StaticData
{
    public class EquipmentStaticData : ItemStaticData
    {
        public ItemType type;
        
        [Space]
        public int maxLevel;
        public string name;
        public Sprite icon;
        public int maxNumberForCell = ConstantValues.MIN_NUMBER_ITEM_FOR_CELL;
        
        [Space]
        public StatType typeStat;
        public List<QualityValue> qualityValues;
    }
}