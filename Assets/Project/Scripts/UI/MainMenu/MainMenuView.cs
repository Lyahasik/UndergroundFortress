using System;
using System.Collections.Generic;
using UnityEngine;

using UndergroundFortress.Core.Localization;
using UndergroundFortress.Core.Services.Bonuses;
using UndergroundFortress.Core.Services.Factories.UI;
using UndergroundFortress.Core.Services.Progress;
using UndergroundFortress.Core.Services.StaticData;
using UndergroundFortress.Core.Progress;
using UndergroundFortress.Core.Services.Ads;
using UndergroundFortress.Gameplay.Character;
using UndergroundFortress.Gameplay.StaticData;
using UndergroundFortress.Gameplay.Tutorial.Services;
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
        [SerializeField] private AccumulatedRewardButton accumulatedRewardButton;
        [SerializeField] private GameObject listBuffs;

        [Space]
        [SerializeField] private List<UnlockByTutorialStage> unlocksByTutorial;

        private IUIFactory _uiFactory;
        private IProcessingAdsService _processingAdsService;
        private ILocalizationService _localizationService;
        private IInformationService _informationService;

        private List<IWindow> _windows;
        private WindowType _currentWindowType;

        private BonusData _bonusData;
        private RewardsData _rewardsData;

        private ProgressTutorialService _progressTutorialService;

        public CurrentStatFillView PlayerHealthFill => playerHealthFill;

        public void Construct(IUIFactory uiFactory,
            IProcessingAdsService processingAdsService,
            ILocalizationService localizationService,
            IInformationService informationService)
        {
            _uiFactory = uiFactory;
            _processingAdsService = processingAdsService;
            _localizationService = localizationService;
            _informationService = informationService;
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
            
            offerButton.Construct(_localizationService);
            offerButton.Initialize(ShowBonusOffer);
            accumulatedRewardButton.Initialize(ShowAccumulatedReward);
            
            unlocksByTutorial.ForEach(data => data.Initialize(progressProviderService));
        }

        public void ActivateWindow(int idWindow)
        {
            _currentWindowType = (WindowType) idWindow;

            foreach (IWindow window in _windows) 
                window.ActivationUpdate(_currentWindowType);
            
            if (offerButton.IsActive)
                offerButton.gameObject.SetActive(_currentWindowType == WindowType.StartLevel);
            if (accumulatedRewardButton.IsActive)
                accumulatedRewardButton.gameObject.SetActive(_currentWindowType == WindowType.StartLevel);
            if (_currentWindowType == WindowType.StartLevel)
            {
                if (_bonusData != null)
                {
                    offerButton.Activate(_bonusData.iconOffer, _bonusData.lifetimeOffer);
                    _bonusData = null;
                }
                
                if (_rewardsData != null)
                {
                    accumulatedRewardButton.Activate();
                    _rewardsData = null;
                }
            }
            
            listBuffs.SetActive(_currentWindowType == WindowType.StartLevel);
            _processingAdsService.ShowAdsInterstitial();
            
            CheckTutorial();
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
            buffView.Construct(_localizationService, processingBonusesService, bonusData);
            buffView.transform.SetParent(listBuffs.transform);
        }

        public void ActivateAccumulatedRewardButton(RewardsData rewardsData)
        {
            _rewardsData = rewardsData;
            
            _informationService.UpdateAccumulatedReward(_rewardsData);
            
            if (_currentWindowType != WindowType.StartLevel
                && _currentWindowType != WindowType.Empty)
                return;
            
            accumulatedRewardButton.Activate();
            _rewardsData = null;
        }

        public void ActivateTutorial(ProgressTutorialService progressTutorialService)
        {
            _progressTutorialService = progressTutorialService;
        }

        private void ShowBonusOffer()
        {
            offerButton.Deactivate();
            _informationService.ShowBonusOffer();
        }

        private void ShowAccumulatedReward()
        {
            accumulatedRewardButton.Deactivate();
            _informationService.ShowAccumulatedReward();
        }

        private void CheckTutorial()
        {
            _progressTutorialService?.SuccessStep();
        }
    }
}