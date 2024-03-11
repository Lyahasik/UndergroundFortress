using UndergroundFortress.Core.Services;
using UndergroundFortress.Gameplay.Items;
using UndergroundFortress.Gameplay.Items.Equipment;
using UndergroundFortress.Gameplay.Items.Resource;
using UndergroundFortress.Gameplay.StaticData;
using UndergroundFortress.UI.Craft.Recipe;

namespace UndergroundFortress.Gameplay.Craft.Services
{
    public interface ICraftService : IService
    {
        public EquipmentData TryCreateEquipment(EquipmentStaticData equipmentStaticData,
            int currentLevel,
            int moneyPrice,
            ListPrice listPrice,
            ItemData crystal = null);
        public ResourceData TryCreateResource(ResourceStaticData resourceStaticData, int moneyPrice, ListPrice listPrice);
    }
}