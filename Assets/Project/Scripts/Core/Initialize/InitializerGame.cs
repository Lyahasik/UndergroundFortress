using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

using UndergroundFortress.Core.Localization;
using UndergroundFortress.Core.Publish;
using UndergroundFortress.Core.Services;
using UndergroundFortress.Core.Services.Ads;
using UndergroundFortress.Core.Services.Analytics;
using UndergroundFortress.Core.Services.Characters;
using UndergroundFortress.Core.Services.Factories.Gameplay;
using UndergroundFortress.Core.Services.Factories.UI;
using UndergroundFortress.Core.Services.GameStateMachine;
using UndergroundFortress.Core.Services.GameStateMachine.States;
using UndergroundFortress.Core.Services.Progress;
using UndergroundFortress.Core.Services.Publish.Purchases;
using UndergroundFortress.Core.Services.Rewards;
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
        [SerializeField] private PublishHandler publishHandlerPrefab;
        [SerializeField] private LoadingCurtain curtainPrefab;
        [SerializeField] private UpdateHandler updateHandlerPrefab;
        [SerializeField] private StatsRestorationHandler statsRestorationHandlerPrefab;

        private ServicesContainer _servicesContainer;

        private IEnumerator Start()
        {
            var localizationService = new LocalizationService();
            
            yield return localizationService.Initialize();
            
            RegisterServices(localizationService);
            _servicesContainer.Single<IGameStateMachine>().Enter<LoadProgressState>();
        }

        private void RegisterServices(LocalizationService localizationService)
        {
            _servicesContainer = new ServicesContainer();
            
            _servicesContainer.Register<ILocalizationService>(localizationService);

            UpdateHandler updateHandler = CreateUpdateHandler();

            RegisterStaticDataService();
            RegisterProcessingAnalyticsService();
            RegisterProcessingAdsService();
            RegisterProcessingPurchasesService();

            PublishHandler publishHandler = CreatePublishHandler();

            GameStateMachine gameStateMachine = new GameStateMachine();
            
            RegisterProgressProviderService(gameStateMachine, publishHandler, updateHandler);
            publishHandler.Initialize(_servicesContainer.Single<IProgressProviderService>(),
                _servicesContainer.Single<IProcessingAdsService>(),
                _servicesContainer.Single<IProcessingPurchasesService>());
            RegisterStatsRestorationService();
            RegisterProcessingPlayerStatsService();
            RegisterPlayerUpdateLevelService();
            
            RegisterAccumulationRewardsService(updateHandler);

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
                    _servicesContainer.Single<ILocalizationService>(),
                    _servicesContainer.Single<IProcessingAnalyticsService>(),
                    _servicesContainer.Single<IProcessingAdsService>(),
                    _servicesContainer.Single<IProcessingPurchasesService>(),
                    _servicesContainer.Single<IProgressProviderService>(),
                    _servicesContainer.Single<IProcessingPlayerStatsService>(),
                    _servicesContainer.Single<IPlayerUpdateLevelService>(),
                    _servicesContainer.Single<IPlayerDressingService>(),
                    _servicesContainer.Single<IStatsRestorationService>(),
                    _servicesContainer.Single<IAccumulationRewardsService>()));
            _servicesContainer.Single<IProgressProviderService>().SceneProviderService
                = _servicesContainer.Single<ISceneProviderService>();
            
            LoadingCurtain curtain = CreateLoadingCurtain();
            CreateStatsRestorationHandler();
            GameData gameData = GameDataCreate(curtain, _servicesContainer);

            gameStateMachine.Initialize(
                _servicesContainer.Single<IProgressProviderService>(),
                gameData.CoroutinesContainer,
                gameData.Curtain);
            _servicesContainer.Register<IGameStateMachine>(gameStateMachine);
            
            DontDestroyOnLoad(gameData);
        }

        private void RegisterProcessingAnalyticsService()
        {
            var service = new ProcessingAnalyticsService();
            _servicesContainer.Register<IProcessingAnalyticsService>(service);
            service.Initialize();
        }

        private void RegisterProcessingAdsService()
        {
            var service = new ProcessingAdsService(_servicesContainer.Single<IProcessingAnalyticsService>());
            _servicesContainer.Register<IProcessingAdsService>(service);
            service.Initialize();
        }

        private void RegisterProcessingPurchasesService()
        {
            var service = new ProcessingPurchasesService(_servicesContainer.Single<IProcessingAnalyticsService>());
            _servicesContainer.Register<IProcessingPurchasesService>(service);
            service.Initialize();
        }

        private void RegisterAccumulationRewardsService(UpdateHandler updateHandler)
        {
            AccumulationRewardsService service = new AccumulationRewardsService(
                _servicesContainer.Single<IStaticDataService>(),
                _servicesContainer.Single<IProgressProviderService>());
            _servicesContainer.Register<IAccumulationRewardsService>(service);
            updateHandler.AddUpdatedObject(service);
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

        private void RegisterProgressProviderService(GameStateMachine gameStateMachine,
            PublishHandler publishHandler,
            UpdateHandler updateHandler)
        {
            var service = new ProgressProviderService(
                publishHandler,
                _servicesContainer.Single<IStaticDataService>(),
                _servicesContainer.Single<IProcessingAnalyticsService>(),
                gameStateMachine);
            
            service.Initialize(updateHandler);
            
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

        private PublishHandler CreatePublishHandler()
        {
            PublishHandler publishHandler = Instantiate(publishHandlerPrefab);
            publishHandler.Construct(publishHandlerPrefab.name);
            
            return publishHandler;
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
