using System;

using UndergroundFortress.Core.Services;
using UndergroundFortress.Gameplay.Shop;
using UndergroundFortress.Gameplay.Skills.Services;
using UndergroundFortress.UI.Craft;
using UndergroundFortress.UI.Inventory;
using UndergroundFortress.UI.MainMenu;
using UndergroundFortress.UI.Shop;
using UndergroundFortress.UI.Skills;

namespace UndergroundFortress.Gameplay.Tutorial.Services
{
    public interface IProgressTutorialService : IService
    {
        public void Initialize(MainMenuView mainMenuView,
            CraftView craftView,
            InventoryView inventoryView,
            ShopView shopView,
            IShoppingService shoppingService,
            ISkillsUpgradeService skillsUpgradeService,
            SkillsView skillsView,
            TutorialView tutorialView);
        public bool TryActivateStage(TutorialStageType stageType, Action onClose = null);
        public void DeactivateStage();
        public void SuccessStage(TutorialStageType stageType);
        public bool IdSuccessStage(TutorialStageType stageType);
        public void SuccessStep();
        public void OnClose();
    }
}