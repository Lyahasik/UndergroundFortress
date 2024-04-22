using System;
using System.Collections.Generic;
using UnityEngine;

using UndergroundFortress.Core.Services.Bonuses;
using UndergroundFortress.Core.Services.Factories.UI;
using UndergroundFortress.Core.Services.Progress;
using UndergroundFortress.Core.Services.StaticData;
using UndergroundFortress.Gameplay.Character;
using UndergroundFortress.Gameplay.Craft.Services;
using UndergroundFortress.Gameplay.Items.Services;
using UndergroundFortress.Gameplay.StaticData;
using UndergroundFortress.UI.Craft;
using UndergroundFortress.UI.Information.Services;
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

        [Space]
        [SerializeField] private BonusOfferButton offerButton;
        [SerializeField] private GameObject listBuffs;

        private IUIFactory _uiFactory;
        private IInformationService _informationService;
        private IItemsGeneratorService _itemsGeneratorService;
        private IActivationRecipesService _activationRecipesService;

        private List<IWindow> _windows;
        private WindowType _currentWindowType;

        private BonusData _bonusData;

        public CurrentStatFillView PlayerHealthFill => playerHealthFill;

        public void Construct(IUIFactory uiFactory,
            IInformationService informationService,
            IItemsGeneratorService itemsGeneratorService,
            IActivationRecipesService activationRecipesService)
        {
            _uiFactory = uiFactory;
            _informationService = informationService;
            _itemsGeneratorService = itemsGeneratorService;
            _activationRecipesService = activationRecipesService;
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
            
            offerButton.Initialize(ShowBonusOffer);
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

        public void ActivateWindow(int idWindow)
        {
            _currentWindowType = (WindowType) idWindow;

            foreach (IWindow window in _windows) 
                window.ActivationUpdate(_currentWindowType);
            
            if (offerButton.IsActive)
                offerButton.gameObject.SetActive(_currentWindowType == WindowType.StartLevel);
            if (_currentWindowType == WindowType.StartLevel
                && _bonusData != null)
            {
                offerButton.Activate(_bonusData.iconOffer, _bonusData.lifetimeOffer);
                _bonusData = null;
            }
            
            listBuffs.SetActive(_currentWindowType == WindowType.StartLevel);
        }

        public void ActivateBonusOfferButton(BonusData bonusData, Action onBonusActivate)
        {
            _informationService.UpdateBonusOffer(bonusData, onBonusActivate);
            
            if (_currentWindowType != WindowType.StartLevel)
            {
                _bonusData = bonusData;
                return;
            }
            
            offerButton.Activate(bonusData.iconOffer, bonusData.lifetimeOffer);
            _bonusData = null;
        }

        public void ShowBuff(IProcessingBonusesService processingBonusesService, BonusData bonusData)
        {
            BuffView buffView = _uiFactory.CreateBuff();
            buffView.Construct(processingBonusesService, bonusData);
            buffView.transform.SetParent(listBuffs.transform);
        }

        private void ShowBonusOffer()
        {
            offerButton.Deactivate();
            _informationService.ShowBonusOffer();
        }
    }
}