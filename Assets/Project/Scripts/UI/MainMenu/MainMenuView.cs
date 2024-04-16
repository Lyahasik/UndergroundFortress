using System.Collections.Generic;
using UnityEngine;

using UndergroundFortress.Core.Services.Progress;
using UndergroundFortress.Core.Services.Scene;
using UndergroundFortress.Core.Services.StaticData;
using UndergroundFortress.Gameplay.Character;
using UndergroundFortress.Gameplay.Craft.Services;
using UndergroundFortress.Gameplay.Items.Services;
using UndergroundFortress.Gameplay.Skills.Services;
using UndergroundFortress.Gameplay.StaticData;
using UndergroundFortress.Gameplay.Stats;
using UndergroundFortress.UI.Craft;
using UndergroundFortress.UI.Inventory;
using UndergroundFortress.UI.Shop;
using UndergroundFortress.UI.Skills;

namespace UndergroundFortress.UI.MainMenu
{
    public class MainMenuView : MonoBehaviour
    {
        [SerializeField] private LevelNumberView levelNumberView;
        [SerializeField] private ExperienceBarView experienceBarView;
        [SerializeField] private CurrentStatFillView playerHealthFill;
        
        [Space]
        [SerializeField] private WalletView walletView;
        [SerializeField] private AmountSpaceBag amountSpaceBag;

        private ISceneProviderService _sceneProviderService;
        private IItemsGeneratorService _itemsGeneratorService;
        private IActivationRecipesService _activationRecipesService;
        private ISkillsUpgradeService _skillsUpgradeService;

        private List<IWindow> _windows;
        
        public CurrentStatFillView PlayerHealthFill => playerHealthFill;

        public void Construct(ISceneProviderService sceneProviderService,
            IItemsGeneratorService itemsGeneratorService,
            IActivationRecipesService activationRecipesService,
            ISkillsUpgradeService skillsUpgradeService)
        {
            _sceneProviderService = sceneProviderService;
            _itemsGeneratorService = itemsGeneratorService;
            _activationRecipesService = activationRecipesService;
            _skillsUpgradeService = skillsUpgradeService;
        }

        public void Initialize(
            HomeView homeView,
            SkillsView skillsView,
            CraftView craftView,
            InventoryView inventoryView,
            ShopView shopView,
            StartLevelView startLevelView,
            IStaticDataService staticDataService,
            IProgressProviderService progressProviderService)
        {
            _windows = new List<IWindow>();
            
            _windows.Add(homeView);
            _windows.Add(skillsView);
            _windows.Add(craftView);
            _windows.Add(inventoryView);
            _windows.Add(shopView);
            _windows.Add(startLevelView);

            levelNumberView.Initialize(progressProviderService);
            experienceBarView.Construct(staticDataService, progressProviderService);
            experienceBarView.Initialize();
            walletView.Initialize(progressProviderService);
            
            amountSpaceBag.Register(progressProviderService);
        }
        
        //TODO temporary
        public void CreateResource(int id)
        {
            for (int i = 0; i < 10; i++)
            {
                _itemsGeneratorService.GenerateResourceById(id);
            }
        }
        
        //TODO temporary
        private static int _idRecipeEquip;
        public void CreateRecipeEquip()
        {
            _activationRecipesService.ActivateRecipe(_idRecipeEquip++);
        }
        
        //TODO temporary
        private static int _idRecipeResource = 1000;

        public void CreateRecipeResource()
        {
            _activationRecipesService.ActivateRecipe(_idRecipeResource++);
        }
        
        //TODO temporary
        public void UpgradeProgressSkill()
        {
            _skillsUpgradeService.UpdateProgressSkill(SkillsType.Dodge, StatType.Dodge);
            _skillsUpgradeService.UpdateProgressSkill(SkillsType.Dodge, StatType.Accuracy);
            _skillsUpgradeService.UpdateProgressSkill(SkillsType.Crit, StatType.Crit);
            _skillsUpgradeService.UpdateProgressSkill(SkillsType.Crit, StatType.Parry);
            _skillsUpgradeService.UpdateProgressSkill(SkillsType.Block, StatType.Block);
            _skillsUpgradeService.UpdateProgressSkill(SkillsType.Block, StatType.BreakThrough);
            _skillsUpgradeService.UpdateProgressSkill(SkillsType.Stun, StatType.Stun);
            _skillsUpgradeService.UpdateProgressSkill(SkillsType.Stun, StatType.Strength);
        }

        public void ActivateWindow(int idWindow)
        {
            WindowType windowType = (WindowType) idWindow;

            foreach (IWindow window in _windows) 
                window.ActivationUpdate(windowType);
        }
    }
}