using UndergroundFortress.Core.Services;
using UndergroundFortress.Gameplay.Items.Equipment;
using UndergroundFortress.Gameplay.Items.Resource;
using UndergroundFortress.Gameplay.StaticData;
using UndergroundFortress.Gameplay.Stats;

namespace UndergroundFortress.Gameplay.Items.Services
{
    public interface IItemsGeneratorService : IService
    {
        public ResourceData GenerateResource();
        public ResourceData GenerateResourceById(int id);
        public ResourceData TryGenerateResourceById(int id);
        public ResourceData GenerateResource(ResourceStaticData resourceStaticData);
        
        public EquipmentData GenerateEquipment(int id,
            int currentLevel = int.MaxValue,
            StatType setStatType = StatType.Empty);
    }
}