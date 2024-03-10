using UndergroundFortress.Core.Services;
using UndergroundFortress.Gameplay.Items;
using UndergroundFortress.Gameplay.StaticData;
using UndergroundFortress.UI.Craft.Recipe;

namespace UndergroundFortress.Gameplay.Craft.Services
{
    public interface ICraftService : IService
    {
        public void TryCreateEquipment(EquipmentStaticData equipmentStaticData,
            int currentLevel,
            int moneyPrice,
            ListPrice listPrice,
            ItemData crystal = null);
        public void TryCreateResource(ResourceStaticData resourceStaticData, int moneyPrice, ListPrice listPrice);
    }
}