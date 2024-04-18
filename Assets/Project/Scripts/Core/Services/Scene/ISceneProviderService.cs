using UndergroundFortress.Gameplay.Inventory.Services;
using UndergroundFortress.Gameplay.Items.Services;
using UndergroundFortress.Gameplay.Skills.Services;

namespace UndergroundFortress.Core.Services.Scene
{
    public interface ISceneProviderService : IService
    {
        public void LoadMainScene();
        public void LoadLevel(IItemsGeneratorService itemsGeneratorService, 
            IInventoryService inventoryService,
            ISkillsUpgradeService skillsUpgradeService,
            int idDungeon,
            int idLevel);
    }
}