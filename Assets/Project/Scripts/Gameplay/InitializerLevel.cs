using UnityEngine;

using UndergroundFortress.Core.Services;
using UndergroundFortress.Core.Services.Factories.Gameplay;
using UndergroundFortress.Core.Services.Factories.UI;
using UndergroundFortress.Core.Services.Progress;
using UndergroundFortress.Core.Services.Scene;
using UndergroundFortress.Core.Services.StaticData;
using UndergroundFortress.Gameplay.Character;
using UndergroundFortress.Gameplay.Character.Services;
using UndergroundFortress.Gameplay.Dungeons.Services;
using UndergroundFortress.Gameplay.Player.Level.Services;
using UndergroundFortress.Gameplay.Stats.Services;
using UndergroundFortress.UI.Hud;

namespace UndergroundFortress.Gameplay
{
    public class InitializerLevel : MonoBehaviour
    {
        private ISceneProviderService _sceneProviderService;
        private IStaticDataService _staticDataService;
        private IGameplayFactory _gameplayFactory;
        private IUIFactory _uiFactory;
        private IProcessingPlayerStatsService _processingPlayerStatsService;

        private ServicesContainer _gameplayServicesContainer;

        private PlayerData _playerData;
        private CharacterStats _enemyStats;

        private void OnDestroy()
        {
            ClearGameplayServices();
        }

        public void Construct(ISceneProviderService sceneProviderService,
            IStaticDataService staticDataService,
            IGameplayFactory gameplayFactory,
            IUIFactory uiFactory,
            IProcessingPlayerStatsService processingPlayerStatsService)
        {
            _sceneProviderService = sceneProviderService;
            _staticDataService = staticDataService;
            _gameplayFactory = gameplayFactory;
            _uiFactory = uiFactory;
            _processingPlayerStatsService = processingPlayerStatsService;
        }

        public void Initialize(IProgressProviderService progressProviderService,
            IStatsRestorationService statsRestorationService,
            IPlayerUpdateLevelService playerUpdateLevelService,
            int dungeonId, int levelId)
        {
            RegisterGameplayServices(statsRestorationService, playerUpdateLevelService, progressProviderService);
                
            HudView hudView = CreateHUD(progressProviderService);

            CreateGameplay(hudView, dungeonId, levelId);
        }

        private void RegisterGameplayServices(IStatsRestorationService statsRestorationService,
            IPlayerUpdateLevelService playerUpdateLevelService,
            IProgressProviderService progressProviderService)
        {
            _gameplayServicesContainer = new ServicesContainer();
            
            _gameplayServicesContainer.Register<ICheckerCurrentStatsService>(
                new CheckerCurrentStatsService());
            _gameplayServicesContainer.Register<IStatsWasteService>(
                new StatsWasteService());
            _gameplayServicesContainer.Register<IAttackService>(
                new AttackService(
                    _gameplayServicesContainer.Single<IStatsWasteService>()));

            _gameplayServicesContainer.Register<IProgressDungeonService>(
                new ProgressDungeonService(
                    _staticDataService,
                    progressProviderService,
                    _gameplayFactory, 
                    _gameplayServicesContainer.Single<IAttackService>(),
                    statsRestorationService,
                    _gameplayServicesContainer.Single<ICheckerCurrentStatsService>(),
                    playerUpdateLevelService));

        }

        private void CreateGameplay(HudView hudView, int dungeonId, int levelId)
        {
            Canvas gameplayCanvas = _gameplayFactory.CreateGameplayCanvas();
            
            _playerData = _gameplayFactory.CreatePlayer(gameplayCanvas.transform);
            _playerData.Construct(
                _processingPlayerStatsService.PlayerStats,
                hudView.PlayerHealthFill,
                hudView.PlayerStaminaFill);
            _playerData.Initialize();

            var progressDungeonService = _gameplayServicesContainer.Single<IProgressDungeonService>();
            progressDungeonService.Initialize(gameplayCanvas,
                hudView,
                _playerData,
                dungeonId,
                levelId);
            progressDungeonService.StartBattle();
        }

        private HudView CreateHUD(IProgressProviderService progressProviderService)
        {
            HudView hudView = _uiFactory.CreateHUD();
            hudView.Construct(_sceneProviderService);
            hudView.Initialize(_staticDataService, 
                _gameplayServicesContainer.Single<IProgressDungeonService>(),
                progressProviderService);

            return hudView;
        }

        private void ClearGameplayServices()
        {
            _gameplayServicesContainer.Clear();
            
            _gameplayServicesContainer = null;
        }
    }
}