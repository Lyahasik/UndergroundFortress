using UndergroundFortress.Core.Services;
using UndergroundFortress.Gameplay.Items.Equipment;
using UndergroundFortress.Gameplay.StaticData;
using UndergroundFortress.Gameplay.Stats;

namespace UndergroundFortress.Gameplay.Craft.Services
{
    public interface ICraftService : IService
    {
        public EquipmentData CreateEquipment(EquipmentStaticData equipmentStaticData,
            int currentLevel,
            StatType additionalMainType = StatType.Empty);
    }
}