using UndergroundFortress.Gameplay.Inventory.Services;
using UndergroundFortress.Gameplay.Items.Services;

namespace UndergroundFortress.Core.Services.Scene
{
    public interface ISceneProviderService : IService
    {
        public void LoadMainScene();
        public void LoadLevel(IItemsGeneratorService itemsGeneratorService, 
            IInventoryService inventoryService,
            int idDungeon,
            int idLevel);
    }
}