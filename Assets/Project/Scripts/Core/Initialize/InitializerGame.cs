using Unity.VisualScripting;
using UnityEngine;

using UndergroundFortress.Core.Services;
using UndergroundFortress.Core.Services.Ads;
using UndergroundFortress.Core.Services.Characters;
using UndergroundFortress.Core.Services.Factories.Gameplay;
using UndergroundFortress.Core.Services.Factories.UI;
using UndergroundFortress.Core.Services.GameStateMachine;
using UndergroundFortress.Core.Services.GameStateMachine.States;
using UndergroundFortress.Core.Services.Progress;
using UndergroundFortress.Core.Services.Scene;
using UndergroundFortress.Core.Services.StaticData;
using UndergroundFortress.Core.Update;
using UndergroundFortress.Gameplay.Character.Services;
using UndergroundFortress.Gameplay.Player.Level.Services;
using UndergroundFortress.Gameplay.Stats.Services;
using UndergroundFortress.UI.Loading;

namespace UndergroundFortress.Core.Initialize
{
    public class InitializerGame : MonoBehaviour
    {
        [SerializeField] private LoadingCurtain curtainPrefab;
        [SerializeField] private UpdateHandler updateHandlerPrefab;
        [SerializeField] private StatsRestorationHandler statsRestorationHandlerPrefab;

        private ServicesContainer _servicesContainer;
        
        private void Awake()
        {
            RegisterServices();
            _servicesContainer.Single<IGameStateMachine>().Enter<LoadProgressState>();
        }

        private void RegisterServices()
        {
            _servicesContainer = new ServicesContainer();

            UpdateHandler updateHandler = CreateUpdateHandler();

            RegisterStaticDataService();
            _servicesContainer.Register<IProcessingAdsService>(new ProcessingAdsService());

            GameStateMachine gameStateMachine = new GameStateMachine();
            
            RegisterProgressProviderService(gameStateMachine, updateHandler);
            RegisterStatsRestorationService();
            RegisterProcessingPlayerStatsService();
            RegisterPlayerUpdateLevelService();
            
            _servicesContainer.Register<IPlayerDressingService>(
                new PlayerDressingService(_servicesContainer.Single<IProcessingPlayerStatsService>()));
            
            _servicesContainer.Register<IUIFactory>(
                new UIFactory(
                    _servicesContainer.Single<IStaticDataService>()));
            _servicesContainer.Register<IGameplayFactory>(
                new GameplayFactory(
                    _servicesContainer.Single<IStaticDataService>()));

            _servicesContainer.Register<ISceneProviderService>(
                new SceneProviderService(
                    gameStateMachine,
                    updateHandler,
                    _servicesContainer.Single<IUIFactory>(),
                    _servicesContainer.Single<IGameplayFactory>(),
                    _servicesContainer.Single<IStaticDataService>(),
                    _servicesContainer.Single<IProcessingAdsService>(),
                    _servicesContainer.Single<IProgressProviderService>(),
                    _servicesContainer.Single<IProcessingPlayerStatsService>(),
                    _servicesContainer.Single<IPlayerUpdateLevelService>(),
                    _servicesContainer.Single<IPlayerDressingService>(),
                    _servicesContainer.Single<IStatsRestorationService>()));
            
            LoadingCurtain curtain = CreateLoadingCurtain();
            CreateStatsRestorationHandler();
            GameData gameData = GameDataCreate(curtain, _servicesContainer);

            gameStateMachine.Initialize(
                _servicesContainer.Single<IProgressProviderService>(),
                gameData.CoroutinesContainer,
                gameData.Curtain);
            _servicesContainer.Single<ISceneProviderService>().LoadMainScene();
            _servicesContainer.Register<IGameStateMachine>(gameStateMachine);
            
            DontDestroyOnLoad(gameData);
        }

        private UpdateHandler CreateUpdateHandler() => 
            Instantiate(updateHandlerPrefab);

        private void RegisterStaticDataService()
        {
            StaticDataService service = new StaticDataService();
            service.Load();
            _servicesContainer.Register<IStaticDataService>(service);
        }

        private void RegisterProcessingPlayerStatsService()
        {
            var service = new ProcessingPlayerStatsService(
                _servicesContainer.Single<IStaticDataService>(),
                _servicesContainer.Single<IProgressProviderService>(),
                _servicesContainer.Single<IStatsRestorationService>());
            service.Initialize();
            
            _servicesContainer.Register<IProcessingPlayerStatsService>(service);
        }

        private void RegisterStatsRestorationService()
        {
            StatsRestorationService statsRestorationService = new StatsRestorationService();
            statsRestorationService.Initialize();
            _servicesContainer.Register<IStatsRestorationService>(statsRestorationService);
        }

        private void RegisterProgressProviderService(GameStateMachine gameStateMachine, UpdateHandler updateHandler)
        {
            var service = new ProgressProviderService(
                _servicesContainer.Single<IStaticDataService>(),
                gameStateMachine);
            
            service.Initialization(updateHandler);
            
            _servicesContainer.Register<IProgressProviderService>(service);
        }

        private void RegisterPlayerUpdateLevelService()
        {
            var service = new PlayerUpdateLevelService(
                _servicesContainer.Single<IStaticDataService>(),
                _servicesContainer.Single<IProgressProviderService>());
            service.Initialize();
            
            _servicesContainer.Register<IPlayerUpdateLevelService>(service);
        }

        private LoadingCurtain CreateLoadingCurtain()
        {
            LoadingCurtain curtain = Instantiate(curtainPrefab);
            curtain.Construct(curtainPrefab.name,
                _servicesContainer.Single<IStaticDataService>().ForUI());
            
            return curtain;
        }

        private void CreateStatsRestorationHandler()
        {
            StatsRestorationHandler statsRestorationHandler = Instantiate(statsRestorationHandlerPrefab);
            statsRestorationHandler.Construct(_servicesContainer.Single<IStatsRestorationService>());
        }

        private GameData GameDataCreate(LoadingCurtain curtain, ServicesContainer servicesContainer)
        {
            GameData gameData = new GameObject().AddComponent<GameData>();
            gameData.name = nameof(GameData);
            
            Coroutines.CoroutinesContainer coroutinesContainer = gameData.AddComponent<Coroutines.CoroutinesContainer>();
            gameData.Construct(curtain, coroutinesContainer, servicesContainer);
            
            return gameData;
        }
    }
}
