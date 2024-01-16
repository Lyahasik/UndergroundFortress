using System;
using System.Collections.Generic;

using UndergroundFortress.Constants;
using UndergroundFortress.Core.Services.StaticData;
using UndergroundFortress.Gameplay.Inventory.Services;
using UndergroundFortress.Gameplay.Items;
using UndergroundFortress.Gameplay.Items.Equipment;
using UndergroundFortress.Gameplay.StaticData;
using UndergroundFortress.Gameplay.Stats;

using Random = UnityEngine.Random;

namespace UndergroundFortress.Gameplay.Craft.Services
{
    public class CraftService : ICraftService
    {
        private readonly IStaticDataService _staticDataService;
        private readonly IInventoryService _inventoryService;

        public CraftService(IStaticDataService staticDataService,
            IInventoryService inventoryService)
        {
            _staticDataService = staticDataService;
            _inventoryService = inventoryService;
        }

        public EquipmentData CreateEquipment(EquipmentStaticData equipmentStaticData,
            int currentLevel,
            StatType additionalMainType = StatType.Empty)
        {
            currentLevel = Math.Clamp(currentLevel, 0, equipmentStaticData.maxLevel);

            QualityType qualityEquipment = GetRangeQualityType(QualityType.Gray, QualityType.White);

            List<StatItemData> mainStats = GetMainStats(
                equipmentStaticData.qualityValues,
                qualityEquipment,
                equipmentStaticData.typeStat,
                additionalMainType);

            int numberAdditionalStats = (int)qualityEquipment - (int)QualityType.Gray;
            numberAdditionalStats = Math.Clamp(numberAdditionalStats, 0, ConstantValues.MAX_NUMBER_ADDITIONAL_STATS);
            List<StatItemData> additionalStats = GetAdditionalStats(numberAdditionalStats);
            
            bool isSet = additionalMainType == StatType.Empty ? false : true;

            EquipmentData equipmentData = new EquipmentData(equipmentStaticData.id,
                equipmentStaticData.type,
                currentLevel,
                isSet,
                qualityEquipment,
                equipmentStaticData.name,
                equipmentStaticData.icon,
                equipmentStaticData.maxNumberForCell,
                mainStats,
                additionalStats,
                new List<StoneItemData>());
            
            _inventoryService.AddItem(equipmentData);
            
            return equipmentData;
        }

        private List<StatItemData> GetMainStats(List<QualityValue> equipQualityValues,
            QualityType qualityEquipment, 
            StatType mainType, 
            StatType additionalMainType = StatType.Empty)
        {
            float qualityValue = GetQualityValue(equipQualityValues, qualityEquipment, additionalMainType);
            List<StatItemData> stats = new List<StatItemData>
            {
                new (mainType, null, qualityValue)
            };
            
            if (additionalMainType != StatType.Empty)
            {
                qualityValue = GetQualityValue(
                    GetStaticQualityValues(additionalMainType),
                    qualityEquipment,
                    additionalMainType);
                stats.Add(new(additionalMainType, null, qualityValue));
            }

            return stats;
        }

        private List<StatItemData> GetAdditionalStats(int number)
        {
            List<StatItemData> stats = new List<StatItemData>();

            if (number == 0)
                return stats;

            for (int i = number; i > 0; i--)
            {
                int idStat = Random.Range(0, _staticDataService.ForStats().Count);
                
                StatStaticData staticStatData = _staticDataService.ForStats()[idStat];
                StatType typeStat = staticStatData.type;
                QualityType qualityStat = GetRangeQualityType(QualityType.Gray, QualityType.White);

                float qualityValue = GetQualityValue(staticStatData.qualityValues, qualityStat, typeStat);
                stats.Add(new (typeStat, null, qualityValue));
            }

            return stats;
        }

        private List<QualityValue> GetStaticQualityValues(StatType type) => 
            _staticDataService.ForStats().Find(v => v.type == type).qualityValues;

        private static QualityType GetRangeQualityType(QualityType minType, QualityType maxType) => 
            (QualityType) Random.Range((int) minType, (int) maxType + 1);

        private float GetQualityValue(List<QualityValue> qualityValues, QualityType qualityStat, StatType statType)
        {
            QualityValue qualityValuesStat = 
                qualityValues.Find(v => v.qualityType == qualityStat);
            
            float qualityValue = Random.Range(qualityValuesStat.minValue, qualityValuesStat.maxValue);
            qualityValue = RoundByType(qualityValue, statType);
            
            return qualityValue;
        }

        private float RoundByType(float value, in StatType type)
        {
            switch (type)
            {
                case StatType.Health:
                case StatType.Stamina:
                case StatType.Damage:
                case StatType.Defense:
                    value = MathF.Round(value);
                    break;
                default:
                    value = MathF.Round(value, ConstantValues.DIGITS_STAT_VALUE);
                    break;
            }
            
            return value;
        }
    }
}