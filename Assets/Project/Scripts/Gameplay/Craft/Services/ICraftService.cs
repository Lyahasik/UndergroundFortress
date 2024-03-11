using UndergroundFortress.Core.Services;
using UndergroundFortress.Gameplay.Items;
using UndergroundFortress.Gameplay.Items.Equipment;
using UndergroundFortress.Gameplay.Items.Resource;
using UndergroundFortress.UI.Craft.Recipe;

namespace UndergroundFortress.Gameplay.Craft.Services
{
    public interface ICraftService : IService
    {
        public EquipmentData TryCreateEquipment(int itemId,
            int currentLevel,
            int moneyPrice,
            ListPrice listPrice,
            ItemData crystal = null,
            bool isEnoughResources = true);
        public ResourceData TryCreateResource(int itemId,
            int moneyPrice,
            ListPrice listPrice,
            bool isEnoughResources = true);
    }
}