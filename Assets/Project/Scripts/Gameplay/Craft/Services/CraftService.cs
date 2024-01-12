using System;
using System.Collections.Generic;

using UndergroundFortress.Constants;
using UndergroundFortress.Core.Services.StaticData;
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

        public CraftService(IStaticDataService staticDataService)
        {
            _staticDataService = staticDataService;
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
            
            return new EquipmentData(currentLevel,
                qualityEquipment, 
                equipmentStaticData.name, 
                equipmentStaticData.icon,
                mainStats,
                additionalStats,
                new List<StoneItemData>(),
                isSet);
        }

        private List<StatItemData> GetMainStats(List<QualityValue> equipQualityValues,
            QualityType qualityEquipment, 
            StatType mainType, 
            StatType additionalMainType = StatType.Empty)
        {
            List<StatItemData> stats = new List<StatItemData>
            {
                new (mainType, 
                    null,
                    GetQualityValue(equipQualityValues, qualityEquipment))
            };
            
            if (additionalMainType != StatType.Empty)
                stats.Add(new (additionalMainType, 
                    null,
                    GetQualityValue(GetStaticQualityValues(additionalMainType), qualityEquipment)));

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
                
                stats.Add(new (typeStat, null, GetQualityValue(staticStatData.qualityValues, qualityStat)));
            }

            return stats;
        }

        private List<QualityValue> GetStaticQualityValues(StatType type) => 
            _staticDataService.ForStats().Find(v => v.type == type).qualityValues;

        private static QualityType GetRangeQualityType(QualityType minType, QualityType maxType) => 
            (QualityType) Random.Range((int) minType, (int) maxType + 1);

        private float GetQualityValue(List<QualityValue> qualityValues, QualityType qualityStat)
        {
            QualityValue qualityValuesStat = 
                qualityValues.Find(v => v.qualityType == qualityStat);
            
            return Random.Range(qualityValuesStat.minValue, qualityValuesStat.maxValue);
        }
    }
}