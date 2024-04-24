using System;
using System.Collections.Generic;
using System.Linq;

using UndergroundFortress.Constants;
using UndergroundFortress.Core.Services.StaticData;
using UndergroundFortress.Extensions;
using UndergroundFortress.Gameplay.Inventory.Services;
using UndergroundFortress.Gameplay.Items.Equipment;
using UndergroundFortress.Gameplay.Items.Resource;
using UndergroundFortress.Gameplay.StaticData;
using UndergroundFortress.Gameplay.Stats;
using Random = UnityEngine.Random;

namespace UndergroundFortress.Gameplay.Items.Services
{
    public class ItemsGeneratorService : IItemsGeneratorService
    {
        private readonly IStaticDataService _staticDataService;
        private readonly IInventoryService _inventoryService;

        public ItemsGeneratorService(IStaticDataService staticDataService,
            IInventoryService inventoryService)
        {
            _staticDataService = staticDataService;
            _inventoryService = inventoryService;
        }

        public ResourceData GenerateResource()
        {
            List<ResourceStaticData> resources = _staticDataService.ForResources();

            int id = Random.Range(0, resources.Count);
            return CreateResource(resources[id]);
        }

        public ResourceData GenerateResource(ResourceStaticData resourceStaticData) => 
            CreateResource(resourceStaticData);

        public ResourceData GenerateResourceById(int id)
        {
            List<ResourceStaticData> resources = _staticDataService.ForResources();

            return CreateResource(resources.Find(resource => resource.id == id));
        }

        public void GenerateResourcesById(int id, int number)
        {
            ResourceStaticData resource = _staticDataService.ForResources().Find(resource => resource.id == id);

            CreateResources(resource, number);
        }

        public ResourceData TryGenerateResourceById(int id)
        {
            List<ResourceStaticData> resources = _staticDataService.ForResources();
            var resource = resources.Find(resource => resource.id == id);

            if (!TryGenerateByQuality(resource.quality))
                return null;
            
            return CreateResource(resource);
        }

        private bool TryGenerateByQuality(QualityType qualityType)
        {
            var qualities = _staticDataService.ForQualities().qualitiesData;
            int totalWeight = qualities.Sum(data => data.weight);

            int accident = Random.Range(0, totalWeight);
            foreach (QualityData qualityData in qualities)
            {
                accident -= qualityData.weight;
                if (accident < 0)
                    return qualityType == qualityData.type;
            }
            
            return false;
        }

        private ResourceData CreateResource(ResourceStaticData resourceStaticData)
        {
            if (_inventoryService.IsBagFullForResource(resourceStaticData.type, resourceStaticData.id))
                return null;

            ResourceData resourceData;
            if (resourceStaticData is ConsumableStaticData data)
            {
                resourceData = new ConsumableResourceData(
                    data.id,
                    data.name,
                    data.type,
                    data.quality,
                    data.consumableType);
            }
            else
            {
                resourceData = new ResourceData(
                    resourceStaticData.id,
                    resourceStaticData.name,
                    resourceStaticData.type,
                    resourceStaticData.quality);
            }

            _inventoryService.AddItem(resourceData);

            return resourceData;
        }

        private void CreateResources(ResourceStaticData resourceStaticData, int number)
        {
            ResourceData resourceData;
            if (resourceStaticData is ConsumableStaticData data)
            {
                resourceData = new ConsumableResourceData(
                    data.id,
                    data.name,
                    data.type,
                    data.quality,
                    data.consumableType);
            }
            else
            {
                resourceData = new ResourceData(
                    resourceStaticData.id,
                    resourceStaticData.name,
                    resourceStaticData.type,
                    resourceStaticData.quality);
            }
            
            _inventoryService.AddItems(resourceData, number);
        }

        public EquipmentData GenerateEquipment(int id,
            int currentLevel = int.MaxValue,
            StatType setStatType = StatType.Empty,
            QualityType qualityType = QualityType.Empty)
        {
            if (_inventoryService.IsBagFull())
                return null;

            EquipmentStaticData equipmentStaticData = _staticDataService.GetEquipmentById(id);

            currentLevel = Math.Clamp(currentLevel, equipmentStaticData.minLevel, equipmentStaticData.maxLevel);

            if (qualityType == QualityType.Empty)
                qualityType = qualityType.Random(_staticDataService.ForQualities().qualitiesData);

            List<StatItemData> mainStats = GetMainStats(
                equipmentStaticData.qualityValues,
                qualityType,
                equipmentStaticData.typeStat,
                equipmentStaticData.statValuePerLevel * currentLevel,
                setStatType);

            int numberAdditionalStats = (int)qualityType - (int)QualityType.Grey;
            numberAdditionalStats = Math.Clamp(numberAdditionalStats, 0, ConstantValues.MAX_NUMBER_ADDITIONAL_STATS);
            List<StatItemData> additionalStats = GetAdditionalStats(numberAdditionalStats);

            bool isSet = setStatType != StatType.Empty;

            EquipmentData equipmentData = new EquipmentData(equipmentStaticData.id,
                equipmentStaticData.name,
                equipmentStaticData.type,
                qualityType,
                currentLevel,
                isSet,
                mainStats,
                additionalStats,
                new List<StoneItemData>());

            _inventoryService.AddItem(equipmentData);

            return equipmentData;
        }

        public void GenerateEquipments(int id,
            int number,
            int currentLevel = Int32.MaxValue,
            StatType setStatType = StatType.Empty,
            QualityType qualityType = QualityType.Empty)
        {
            for (int i = 0; i < number; i++) 
                GenerateEquipment(id, currentLevel, setStatType, qualityType);
        }

        private List<QualityValue> GetStaticQualityValues(StatType type) => 
            _staticDataService.ForStats().Find(v => v.type == type).qualityValues;

        private List<StatItemData> GetAdditionalStats(int number)
        {
            List<StatItemData> stats = new List<StatItemData>();

            if (number == 0)
                return stats;

            while (number > 0)
            {
                int idStat = Random.Range(0, _staticDataService.ForStats().Count);
                
                StatStaticData staticStatData = _staticDataService.ForStats()[idStat];
                if (!staticStatData.type.IsEquipmentAdditional()
                    || IsStatAlreadyAvailable(stats, staticStatData.type))
                    continue;
                
                StatType typeStat = staticStatData.type;
                QualityType qualityStat = QualityType.Empty.Random(_staticDataService.ForQualities().qualitiesData);

                float qualityValue = GetQualityValue(staticStatData.qualityValues, qualityStat, typeStat);
                stats.Add(new (typeStat, qualityStat, qualityValue));
                
                number--;
            }

            return stats;
        }

        private float GetQualityValue(List<QualityValue> qualityValues, QualityType qualityStat, StatType statType)
        {
            QualityValue qualityValuesStat = 
                qualityValues.Find(v => v.qualityType == qualityStat);
            
            float qualityValue = statType.IsInteger() 
                ? Random.Range((int) qualityValuesStat.minValue, (int) qualityValuesStat.maxValue + 1)
                : Random.Range(qualityValuesStat.minValue, qualityValuesStat.maxValue);
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

        private List<StatItemData> GetMainStats(List<QualityValue> equipQualityValues,
            QualityType qualityEquipment, 
            StatType mainType, 
            float baseValue,
            StatType additionalMainType = StatType.Empty)
        {
            float qualityValue = baseValue + GetQualityValue(equipQualityValues, qualityEquipment, mainType);
            List<StatItemData> stats = new List<StatItemData>
            {
                new (mainType, qualityEquipment, qualityValue)
            };
            
            if (additionalMainType != StatType.Empty)
            {
                qualityValue = GetQualityValue(
                    GetStaticQualityValues(additionalMainType),
                    qualityEquipment,
                    additionalMainType);
                if (additionalMainType == StatType.Dodge)
                    qualityValue /= 2f;
                
                stats.Add(new StatItemData(additionalMainType, qualityEquipment, qualityValue));
            }

            return stats;
        }

        private bool IsStatAlreadyAvailable(List<StatItemData> stats, StatType type) => 
            stats.Any(data => data.Type == type);
    }
}