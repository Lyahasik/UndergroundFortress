using System;
using System.Collections.Generic;

using UndergroundFortress.Core.Localization;
using UndergroundFortress.Core.Progress;
using UndergroundFortress.Core.Services.Progress;
using UndergroundFortress.Gameplay.Items.Services;
using UndergroundFortress.Gameplay.Shop;
using UndergroundFortress.Gameplay.Skills.Services;
using UndergroundFortress.Gameplay.Stats;
using UndergroundFortress.UI.Craft;
using UndergroundFortress.UI.Inventory;
using UndergroundFortress.UI.MainMenu;
using UndergroundFortress.UI.Shop;
using UndergroundFortress.UI.Skills;

namespace UndergroundFortress.Gameplay.Tutorial.Services
{
    public class ProgressTutorialService : IProgressTutorialService, IWritingProgress
    {
        private readonly ILocalizationService _localizationService;
        private readonly IProgressProviderService _progressProviderService;
        private readonly IItemsGeneratorService _itemsGeneratorService;

        private TutorialView _tutorialView;

        private MainMenuView _mainMenuView;
        private CraftView _craftView;
        private InventoryView _inventoryView;
        private ShopView _shopView;
        private IShoppingService _shoppingService;
        private ISkillsUpgradeService _skillsUpgradeService;
        private SkillsView _skillsView;
        private HashSet<int> _tutorialSteps;

        private TutorialStageType _currentStageType;
        private Action _onClose;

        public ProgressTutorialService(ILocalizationService localizationService,
            IProgressProviderService progressProviderService,
            IItemsGeneratorService itemsGeneratorService)
        {
            _localizationService = localizationService;
            _progressProviderService = progressProviderService;
            _itemsGeneratorService = itemsGeneratorService;
        }

        public void Initialize(MainMenuView mainMenuView,
            CraftView craftView,
            InventoryView inventoryView,
            ShopView shopView,
            IShoppingService shoppingService,
            ISkillsUpgradeService skillsUpgradeService,
            SkillsView skillsView,
            TutorialView tutorialView)
        {
            _mainMenuView = mainMenuView;
            _craftView = craftView;
            _inventoryView = inventoryView;
            _shopView = shopView;
            _shoppingService = shoppingService;
            _skillsUpgradeService = skillsUpgradeService;
            _skillsView = skillsView;
            _tutorialView = tutorialView;
            _tutorialView.Construct(_localizationService, this);
            
            Register(_progressProviderService);
        }

        public void Register(IProgressProviderService progressProviderService)
        {
            progressProviderService.Register(this);
        }

        public void LoadProgress(ProgressData progress)
        {
            _tutorialSteps = progress.TutorialStages;
            
            TryActivateStage(TutorialStageType.FirstStart);
        }

        public void UpdateProgress(ProgressData progress) {}

        public void WriteProgress()
        {
            _progressProviderService.SaveProgress();
        }

        public bool TryActivateStage(TutorialStageType stageType, Action onClose = null)
        {
            if (_tutorialSteps.Contains((int) stageType)
                || _currentStageType == stageType)
                return false;

            _currentStageType = stageType;
            _onClose = onClose;

            switch (stageType)
            {
                case TutorialStageType.FirstStart:
                    _mainMenuView.ActivateTutorial(this);
                    _inventoryView.ActivateTutorial(this);
                    _itemsGeneratorService.GenerateEquipment(22, 1, qualityType: QualityType.Grey);
                    break;
                case TutorialStageType.FirstCreateEquipment:
                    _mainMenuView.ActivateTutorial(this);
                    _craftView.ActivateTutorial(this);
                    break;
                case TutorialStageType.FirstShopping:
                    _mainMenuView.ActivateTutorial(this);
                    _craftView.ActivateTutorial(this);
                    _shopView.ActivateTutorial(this);
                    _shoppingService.ActivateTutorial(this);
                    _itemsGeneratorService.GenerateResourceById(1006);
                    break;
                case TutorialStageType.SecondCreateEquipment:
                    _craftView.ActivateTutorial(this);
                    break;
                case TutorialStageType.FirstEquipmentPotion:
                    _inventoryView.ActivateTutorial(this);
                    _mainMenuView.ActivateTutorial(this);
                    _itemsGeneratorService.GenerateResourcesById(1003, 10);
                    break;
                case TutorialStageType.UpgradeSkills:
                    _mainMenuView.ActivateTutorial(this);
                    _skillsUpgradeService.ActivateTutorial(this);
                    _skillsView.ActivateTutorial(this);
                    break;
                case TutorialStageType.SuccessDungeon1:
                    _itemsGeneratorService.GenerateEquipment(8, 1, StatType.Dodge, QualityType.Grey);
                    _itemsGeneratorService.GenerateEquipment(8, 1, StatType.Dodge, QualityType.Grey);
                    _itemsGeneratorService.GenerateEquipment(14, 1, StatType.Stun, QualityType.Grey);
                    _itemsGeneratorService.GenerateEquipment(25, 1, StatType.Crit, QualityType.Grey);
                    _itemsGeneratorService.GenerateEquipment(20, 1, StatType.Block, QualityType.Grey);
                    break;
                case TutorialStageType.SuccessDungeon2:
                    _itemsGeneratorService.GenerateResourcesById(1006, 5);
                    break;
            }
            
            _tutorialView.ActivateStage(stageType);
            SuccessStage(stageType);
            
            return true;
        }

        public void DeactivateStage()
        {
            _inventoryView.DeactivateTutorial();
            _shopView.DeactivateTutorial();
            _shoppingService.DeactivateTutorial();
            _skillsUpgradeService.DeactivateTutorial();
            _skillsView.DeactivateTutorial();
        }

        public void SuccessStage(TutorialStageType stageType)
        {
            _tutorialSteps.Add((int)stageType);
            WriteProgress();
        }

        public bool IdSuccessStage(TutorialStageType stageType) => 
            _tutorialSteps.Contains((int)stageType);

        public void SuccessStep()
        {
            _tutorialView.SuccessStep();
            
            OnClose();
        }

        public void OnClose()
        {
            if (_onClose != null)
            {
                _onClose?.Invoke();
                _onClose = null;
            }
        }
    }
}