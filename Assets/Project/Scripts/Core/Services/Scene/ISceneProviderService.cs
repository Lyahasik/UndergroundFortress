using UndergroundFortress.Core.Services.Bonuses;
using UndergroundFortress.Gameplay.Craft.Services;
using UndergroundFortress.Gameplay.Inventory.Services;
using UndergroundFortress.Gameplay.Items.Services;
using UndergroundFortress.Gameplay.Skills.Services;
using UndergroundFortress.Gameplay.Tutorial.Services;

namespace UndergroundFortress.Core.Services.Scene
{
    public interface ISceneProviderService : IService
    {
        public void LoadMainScene();
        public void LoadLevel(IItemsGeneratorService itemsGeneratorService, 
            IInventoryService inventoryService,
            ISkillsUpgradeService skillsUpgradeService,
            IProcessingBonusesService processingBonusesService,
            IActivationRecipesService activationRecipesService,
            IProgressTutorialService progressTutorialService,
            int idDungeon,
            int idLevel);
    }
}