using UndergroundFortress.Core.Services;
using UndergroundFortress.Gameplay.StaticData;
using UndergroundFortress.Gameplay.Stats;

namespace UndergroundFortress.Gameplay.Craft.Services
{
    public interface ICraftService : IService
    {
        public void TryCreateEquipment(EquipmentStaticData equipmentStaticData,
            int currentLevel,
            StatType additionalMainType = StatType.Empty);
        public void TryCreateResource(ResourceStaticData resourceStaticData);
    }
}