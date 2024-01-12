using System.Collections.Generic;
using UnityEngine;

using UndergroundFortress.Core.Services;
using UndergroundFortress.Core.Services.Factories.Gameplay;
using UndergroundFortress.Core.Services.Factories.UI;
using UndergroundFortress.Core.Services.Scene;
using UndergroundFortress.Core.Services.StaticData;
using UndergroundFortress.Gameplay.Character;
using UndergroundFortress.Gameplay.Items.Equipment;
using UndergroundFortress.Gameplay.StaticData;
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
        private CharacterStats _playerStats;

        private ServicesContainer _gameplayServicesContainer;

        private CharacterData _playerData;
        private CharacterData _enemyData;
        private CharacterStats _enemyStats;

        private void OnDestroy()
        {
            ClearGameplayServices();
        }

        public void Construct(ISceneProviderService sceneProviderService,
            IStaticDataService staticDataService,
            IGameplayFactory gameplayFactory,
            IUIFactory uiFactory,
            CharacterStats playerStats)
        {
            _sceneProviderService = sceneProviderService;
            _staticDataService = staticDataService;
            _gameplayFactory = gameplayFactory;
            _uiFactory = uiFactory;
            _playerStats = playerStats;
        }

        public void Initialize()
        {
            RegisterGameplayServices();
                
            HudView hudView = CreateHUD();

            CreateGameplay(hudView);
        }

        private void CreateGameplay(HudView hudView)
        {
            Canvas gameplayCanvas = _gameplayFactory.CreateGameplayCanvas();
            
            _playerData = _gameplayFactory.CreatePlayer(gameplayCanvas.transform);
            _playerData.Construct(_playerStats, new List<EquipmentData>());
            
            CharacterStaticData enemyStaticData = _staticDataService.ForEnemy();
            _enemyStats = new CharacterStats();
            _enemyStats.Initialize(enemyStaticData);
            _enemyData = _gameplayFactory.CreateEnemy(gameplayCanvas.transform);
            _enemyData.Construct(_enemyStats, new List<EquipmentData>());
            
            _gameplayServicesContainer.Single<IStatsRestorationService>().AddStats(_playerStats);
            _gameplayServicesContainer.Single<IStatsRestorationService>().AddStats(_enemyStats);
            
            hudView.playerStatsView.Construct(_playerStats);
            hudView.enemyStatsView.Construct(_enemyStats);

            AttackArea attackArea = _gameplayFactory.CreateAttackArea(gameplayCanvas.transform);
            attackArea.Construct(
                _playerData,
                _enemyData,
                _gameplayServicesContainer.Single<ICheckerCurrentStatsService>(),
                _gameplayServicesContainer.Single<IAttackService>());
        }

        private HudView CreateHUD()
        {
            HudView hudView = _uiFactory.CreateHUD();
            hudView.Construct(_sceneProviderService);
            hudView.Initialize();

            return hudView;
        }

        private void RegisterGameplayServices()
        {
            _gameplayServicesContainer = new ServicesContainer();
            
            _gameplayServicesContainer.Register<ICheckerCurrentStatsService>(
                new CheckerCurrentStatsService());
            _gameplayServicesContainer.Register<IStatsWasteService>(
                new StatsWasteService());
            _gameplayServicesContainer.Register<IAttackService>(
                new AttackService(
                    _gameplayServicesContainer.Single<IStatsWasteService>()));

            RegisterStatsRestorationService();
        }

        private void RegisterStatsRestorationService()
        {
            StatsRestorationService statsRestorationService = new GameObject(nameof(statsRestorationService))
                .AddComponent<StatsRestorationService>();
            statsRestorationService.Initialize();
            _gameplayServicesContainer.Register<IStatsRestorationService>(statsRestorationService);
        }

        private void ClearGameplayServices()
        {
            _gameplayServicesContainer.Clear();
            
            _gameplayServicesContainer = null;
        }
    }
}