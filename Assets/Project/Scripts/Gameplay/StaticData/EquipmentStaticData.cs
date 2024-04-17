using System.Collections.Generic;
using UnityEngine;

using UndergroundFortress.Constants;
using UndergroundFortress.Gameplay.Stats;

namespace UndergroundFortress.Gameplay.StaticData
{
    public class EquipmentStaticData : ItemStaticData
    {
        [Space]
        public int maxLevel;
        public string name;
        public int maxNumberForCell = ConstantValues.MIN_NUMBER_ITEM_FOR_CELL;
        
        [Space]
        public StatType typeStat;
        public List<QualityValue> qualityValues;

        private void Awake()
        {
            CreateQualities();
        }

        private void CreateQualities()
        {
            qualityValues = new List<QualityValue>
            {
                new(QualityType.Grey, 0.1f, 5f),
                new(QualityType.Green, 5f, 10f),
                new(QualityType.Blue, 10f, 15f),
                new(QualityType.Purple, 15f, 20f),
                new(QualityType.Yellow, 20f, 25f),
                new(QualityType.Red, 25f, 30f),
                new(QualityType.White, 30f, 35f)
            };
        }
    }
}