using System;
using UnityEngine;
using UnityEngine.SceneManagement;

using UndergroundFortress.Constants;
using UndergroundFortress.Core.Services.Ads;
using UndergroundFortress.Core.Services.Characters;
using UndergroundFortress.Core.Services.Factories.Gameplay;
using UndergroundFortress.Core.Services.Factories.UI;
using UndergroundFortress.Core.Services.GameStateMachine;
using UndergroundFortress.Core.Services.GameStateMachine.States;
using UndergroundFortress.Core.Services.Progress;
using UndergroundFortress.Core.Services.StaticData;
using UndergroundFortress.Gameplay;
using UndergroundFortress.Gameplay.Character.Services;
using UndergroundFortress.Gameplay.Inventory.Wallet.Services;
using UndergroundFortress.Gameplay.Items.Services;
using UndergroundFortress.Gameplay.Player.Level.Services;
using UndergroundFortress.Gameplay.Stats.Services;
using UndergroundFortress.UI.MainMenu;

namespace UndergroundFortress.Core.Services.Scene
{
    public class SceneProviderService : ISceneProviderService
    {
        private readonly IGameStateMachine _gameStateMachine;
        private readonly IUIFactory _uiFactory;
        private readonly IGameplayFactory _gameplayFactory;
        private readonly IStaticDataService _staticDataService;
        private readonly IProcessingAdsService _processingAdsService;
        private readonly IProgressProviderService _progressProviderService;
        private readonly IProcessingPlayerStatsService _processingPlayerStatsService;
        private readonly IPlayerUpdateLevelService _playerUpdateLevelService;
        private readonly IPlayerDressingService _playerDressingService;
        private readonly IStatsRestorationService _statsRestorationService;
        
        private IItemsGeneratorService _itemsGeneratorService;
        private IWalletOperationService _walletOperationService;

        private string _nameNewActiveScene;

        private bool _isMainMenuInit;

        private int _currentDungeonId;
        private int _currentLevelId;

        public SceneProviderService(IGameStateMachine gameStateMachine,
            IUIFactory uiFactory,
            IGameplayFactory gameplayFactory,
            IStaticDataService staticDataService,
            IProcessingAdsService processingAdsService,
            IProgressProviderService progressProviderService,
            IProcessingPlayerStatsService processingPlayerStatsService,
            IPlayerUpdateLevelService playerUpdateLevelService,
            IPlayerDressingService playerDressingService,
            IStatsRestorationService statsRestorationService)
        {
            _gameStateMachine = gameStateMachine;
            _uiFactory = uiFactory;
            _gameplayFactory = gameplayFactory;
            _staticDataService = staticDataService;
            _processingAdsService = processingAdsService;
            _progressProviderService = progressProviderService;
            _processingPlayerStatsService = processingPlayerStatsService;
            _playerUpdateLevelService = playerUpdateLevelService;
            _playerDressingService = playerDressingService;
            _statsRestorationService = statsRestorationService;
        }

        public void LoadMainScene()
        {
            Debug.Log("Current active scene : " + SceneManager.GetActiveScene().name);

            if (_isMainMenuInit)
            {
                _gameStateMachine.Enter<LoadSceneState>();
                
                PrepareMainMenu();
                return;
            }

            LoadScene(ConstantValues.SCENE_NAME_MAIN_MENU, PrepareMainMenuScene);
        }

        public void LoadLevel(IItemsGeneratorService itemsGeneratorService,
            IWalletOperationService walletOperationService,
            int idDungeon, int idLevel)
        {
            _itemsGeneratorService = itemsGeneratorService;
            _walletOperationService = walletOperationService;
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
                initializerMainMenu.Construct(_staticDataService, _uiFactory, _progressProviderService);
                initializerMainMenu.Initialize(_processingPlayerStatsService, _playerDressingService, this);
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
                _gameplayFactory,
                _uiFactory,
                _processingPlayerStatsService);
            initializerLevel.Initialize(
                _progressProviderService,
                _processingAdsService,
                _itemsGeneratorService,
                _walletOperationService,
                _statsRestorationService,
                _playerUpdateLevelService,
                _currentDungeonId,
                _currentLevelId);

            Debug.Log("Level scene loaded.");
            _gameStateMachine.Enter<GameplayState>();
        }
    }
}