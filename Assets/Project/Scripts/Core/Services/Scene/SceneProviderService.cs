using System;
using UnityEngine;
using UnityEngine.SceneManagement;

using UndergroundFortress.Scripts.Constants;
using UndergroundFortress.Scripts.Core.Services.Factories.Gameplay;
using UndergroundFortress.Scripts.Core.Services.Factories.UI;
using UndergroundFortress.Scripts.Core.Services.GameStateMachine;
using UndergroundFortress.Scripts.Core.Services.GameStateMachine.States;
using UndergroundFortress.Scripts.Core.Services.Progress;
using UndergroundFortress.Scripts.Core.Services.StaticData;
using UndergroundFortress.Scripts.Gameplay;
using UndergroundFortress.Scripts.UI.MainMenu;

namespace UndergroundFortress.Scripts.Core.Services.Scene
{
    public class SceneProviderService : ISceneProviderService
    {
        private readonly IGameStateMachine _gameStateMachine;
        private readonly IUIFactory _uiFactory;
        private readonly IGameplayFactory _gameplayFactory;
        private readonly IStaticDataService _staticDataService;
        private readonly IProgressProviderService _progressProviderService;

        private ServicesContainer _match3ServicesContainer;

        private string _nameNewActiveScene;

        private bool _isMainMenuInit;

        public SceneProviderService(IGameStateMachine gameStateMachine,
            IUIFactory uiFactory,
            IGameplayFactory gameplayFactory,
            IStaticDataService staticDataService,
            IProgressProviderService progressProviderService)
        {
            _gameStateMachine = gameStateMachine;
            _uiFactory = uiFactory;
            _gameplayFactory = gameplayFactory;
            _staticDataService = staticDataService;
            _progressProviderService = progressProviderService;
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

        public void LoadLevel(string sceneName)
        {
            Debug.Log("Current active scene : " + SceneManager.GetActiveScene().name);
            _gameStateMachine.Enter<LoadSceneState>();
            
            LoadScene(sceneName, PrepareLevelScene);
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

            MainMenuView mainMenu = _uiFactory.CreateMainMenu();
            mainMenu.Construct(this);
            mainMenu.Initialize();
            
            Debug.Log("Main scene loaded.");
            _gameStateMachine.Enter<MainMenuState>();
        }

        private void PrepareLevelScene(AsyncOperation obj)
        {
            SceneManager.SetActiveScene(SceneManager.GetSceneByName(_nameNewActiveScene));
            Debug.Log("New active scene : " + SceneManager.GetActiveScene().name);

            InitializerLevel  initializerLevel = new GameObject().AddComponent<InitializerLevel>();
            initializerLevel.name = nameof(InitializerLevel);
            initializerLevel.Construct(this,
                _gameplayFactory,
                _uiFactory,
                _staticDataService.ForLevel(),
                _progressProviderService.CharacterCharacteristics);
            initializerLevel.Initialize();

            Debug.Log("Level scene loaded.");
            _gameStateMachine.Enter<GameplayState>();
        }
    }
}