using System;
using UnityEngine;
using UnityEngine.SceneManagement;

using UndergroundFortress.Constants;
using UndergroundFortress.Core.Localization;
using UndergroundFortress.Core.Services.Ads;
using UndergroundFortress.Core.Services.Analytics;
using UndergroundFortress.Core.Services.Bonuses;
using UndergroundFortress.Core.Services.Characters;
using UndergroundFortress.Core.Services.Factories.Gameplay;
using UndergroundFortress.Core.Services.Factories.UI;
using UndergroundFortress.Core.Services.GameStateMachine;
using UndergroundFortress.Core.Services.GameStateMachine.States;
using UndergroundFortress.Core.Services.Progress;
using UndergroundFortress.Core.Services.Publish.Purchases;
using UndergroundFortress.Core.Services.Rewards;
using UndergroundFortress.Core.Services.StaticData;
using UndergroundFortress.Core.Update;
using UndergroundFortress.Gameplay;
using UndergroundFortress.Gameplay.Character.Services;
using UndergroundFortress.Gameplay.Craft.Services;
using UndergroundFortress.Gameplay.Inventory.Services;
using UndergroundFortress.Gameplay.Items.Services;
using UndergroundFortress.Gameplay.Player.Level.Services;
using UndergroundFortress.Gameplay.Skills.Services;
using UndergroundFortress.Gameplay.Stats.Services;
using UndergroundFortress.Gameplay.Tutorial.Services;
using UndergroundFortress.UI.MainMenu;

namespace UndergroundFortress.Core.Services.Scene
{
    public class SceneProviderService : ISceneProviderService
    {
        private readonly IGameStateMachine _gameStateMachine;
        private readonly UpdateHandler _updateHandler;
        private readonly IUIFactory _uiFactory;
        private readonly IGameplayFactory _gameplayFactory;
        private readonly IStaticDataService _staticDataService;
        private readonly ILocalizationService _localizationService;
        private readonly IProcessingAnalyticsService _processingAnalyticsService;
        private readonly IProcessingAdsService _processingAdsService;
        private readonly IProcessingPurchasesService _processingPurchasesService;
        private readonly IProgressProviderService _progressProviderService;
        private readonly IProcessingPlayerStatsService _processingPlayerStatsService;
        private readonly IPlayerUpdateLevelService _playerUpdateLevelService;
        private readonly IPlayerDressingService _playerDressingService;
        private readonly IStatsRestorationService _statsRestorationService;
        private readonly IAccumulationRewardsService _accumulationRewardsService;

        private IItemsGeneratorService _itemsGeneratorService;
        private IInventoryService _inventoryService;
        private ISkillsUpgradeService _skillsUpgradeService;
        private IProcessingBonusesService _processingBonusesService;
        private IActivationRecipesService _activationRecipesService;
        private IProgressTutorialService _progressTutorialService;

        private string _nameNewActiveScene;

        private bool _isMainMenuInit;

        private int _currentDungeonId;
        private int _currentLevelId;

        public SceneProviderService(IGameStateMachine gameStateMachine,
            UpdateHandler updateHandler,
            IUIFactory uiFactory,
            IGameplayFactory gameplayFactory,
            IStaticDataService staticDataService,
            ILocalizationService localizationService,
            IProcessingAnalyticsService processingAnalyticsService,
            IProcessingAdsService processingAdsService,
            IProcessingPurchasesService processingPurchasesService,
            IProgressProviderService progressProviderService,
            IProcessingPlayerStatsService processingPlayerStatsService,
            IPlayerUpdateLevelService playerUpdateLevelService,
            IPlayerDressingService playerDressingService,
            IStatsRestorationService statsRestorationService,
            IAccumulationRewardsService accumulationRewardsService)
        {
            _gameStateMachine = gameStateMachine;
            _updateHandler = updateHandler;
            _uiFactory = uiFactory;
            _gameplayFactory = gameplayFactory;
            _staticDataService = staticDataService;
            _localizationService = localizationService;
            _processingAnalyticsService = processingAnalyticsService;
            _processingAdsService = processingAdsService;
            _processingPurchasesService = processingPurchasesService;
            _progressProviderService = progressProviderService;
            _processingPlayerStatsService = processingPlayerStatsService;
            _playerUpdateLevelService = playerUpdateLevelService;
            _playerDressingService = playerDressingService;
            _statsRestorationService = statsRestorationService;
            _accumulationRewardsService = accumulationRewardsService;
            
            Debug.Log($"[{ GetType() }] initialize");
        }

        public void LoadMainScene()
        {
            Debug.Log("Current active scene : " + SceneManager.GetActiveScene().name);

            if (_isMainMenuInit)
            {
                _gameStateMachine.Enter<LoadSceneState>();
                
                PrepareMainMenu();
                _progressTutorialService.SuccessStep();
                return;
            }

            LoadScene(ConstantValues.SCENE_NAME_MAIN_MENU, PrepareMainMenuScene);
        }

        public void LoadLevel(IItemsGeneratorService itemsGeneratorService,
            IInventoryService inventoryService,
            ISkillsUpgradeService skillsUpgradeService,
            IProcessingBonusesService processingBonusesService,
            IActivationRecipesService activationRecipesService,
            IProgressTutorialService progressTutorialService,
            int idDungeon, int idLevel)
        {
            _itemsGeneratorService = itemsGeneratorService;
            _inventoryService = inventoryService;
            _skillsUpgradeService = skillsUpgradeService;
            _processingBonusesService = processingBonusesService;
            _activationRecipesService = activationRecipesService;
            _progressTutorialService = progressTutorialService;
            _currentDungeonId = idDungeon;
            _currentLevelId = idLevel;
            
            Debug.Log("Current active scene : " + SceneManager.GetActiveScene().name);
            _gameStateMachine.Enter<LoadSceneState>();
            
            LoadScene(ConstantValues.SCENE_NAME_LEVEL, PrepareLevelScene);
        }

        private void LoadScene(string sceneName, Action<AsyncOperation> prepareScene)
        {
            _nameNewActiveScene = sceneName;
            SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive).completed += prepareScene;
        }

        private void PrepareMainMenuScene(AsyncOperation obj)
        {
            PrepareMainMenu();
            
            _isMainMenuInit = true;
        }

        private void PrepareMainMenu()
        {
            string oldSceneName = SceneManager.GetActiveScene().name;
            SceneManager.SetActiveScene(SceneManager.GetSceneByName(_nameNewActiveScene));
            SceneManager.UnloadSceneAsync(oldSceneName);
            Debug.Log("New active scene : " + SceneManager.GetActiveScene().name);

            if (!_isMainMenuInit)
            {
                InitializerMainMenu initializerMainMenu = new GameObject().AddComponent<InitializerMainMenu>();
                initializerMainMenu.name = nameof(InitializerMainMenu);
                initializerMainMenu.Construct(
                    _staticDataService,
                    _localizationService,
                    _processingAnalyticsService,
                    _processingAdsService,
                    _processingPurchasesService,
                    _uiFactory,
                    _progressProviderService);
                initializerMainMenu.Initialize(
                    _updateHandler,
                    _processingPlayerStatsService,
                    _playerDressingService, 
                    this,
                    _statsRestorationService,
                    _accumulationRewardsService);
            }
            
            Debug.Log("Main scene loaded.");
            _gameStateMachine.Enter<MainMenuState>();
        }

        private void PrepareLevelScene(AsyncOperation obj)
        {
            SceneManager.SetActiveScene(SceneManager.GetSceneByName(_nameNewActiveScene));
            Debug.Log("New active scene : " + SceneManager.GetActiveScene().name);

            InitializerLevel initializerLevel = new GameObject().AddComponent<InitializerLevel>();
            initializerLevel.name = nameof(InitializerLevel);
            initializerLevel.Construct(this,
                _staticDataService,
                _localizationService,
                _processingAnalyticsService,
                _gameplayFactory,
                _uiFactory,
                _processingPlayerStatsService,
                _statsRestorationService);
            initializerLevel.Initialize(
                _progressProviderService,
                _processingAdsService,
                _itemsGeneratorService,
                _inventoryService,
                _inventoryService.WalletOperationService,
                _playerUpdateLevelService,
                _skillsUpgradeService,
                _processingBonusesService,
                _activationRecipesService,
                _progressTutorialService,
                _currentDungeonId,
                _currentLevelId);

            Debug.Log("Level scene loaded.");
            _gameStateMachine.Enter<GameplayState>();
        }
    }
}