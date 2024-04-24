using UnityEngine;

using UndergroundFortress.Core.Services;
using UndergroundFortress.Core.Services.Ads;
using UndergroundFortress.Core.Services.Bonuses;
using UndergroundFortress.Core.Services.Factories.Gameplay;
using UndergroundFortress.Core.Services.Factories.UI;
using UndergroundFortress.Core.Services.Progress;
using UndergroundFortress.Core.Services.Scene;
using UndergroundFortress.Core.Services.StaticData;
using UndergroundFortress.Gameplay.Character;
using UndergroundFortress.Gameplay.Character.Services;
using UndergroundFortress.Gameplay.Craft.Services;
using UndergroundFortress.Gameplay.Dungeons.Services;
using UndergroundFortress.Gameplay.Inventory.Services;
using UndergroundFortress.Gameplay.Inventory.Wallet.Services;
using UndergroundFortress.Gameplay.Items.Services;
using UndergroundFortress.Gameplay.Player.Level.Services;
using UndergroundFortress.Gameplay.Skills.Services;
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
        private IStatsRestorationService _statsRestorationService;

        private ServicesContainer _gameplayServicesContainer;

        private PlayerData _playerData;
        private CharacterStats _enemyStats;

        public void Construct(ISceneProviderService sceneProviderService,
            IStaticDataService staticDataService,
            IGameplayFactory gameplayFactory,
            IUIFactory uiFactory,
            IProcessingPlayerStatsService processingPlayerStatsService,
            IStatsRestorationService statsRestorationService)
        {
            _sceneProviderService = sceneProviderService;
            _staticDataService = staticDataService;
            _gameplayFactory = gameplayFactory;
            _uiFactory = uiFactory;
            _processingPlayerStatsService = processingPlayerStatsService;
            _statsRestorationService = statsRestorationService;
        }

        public void Initialize(IProgressProviderService progressProviderService,
            IProcessingAdsService processingAdsService,
            IItemsGeneratorService itemsGeneratorService,
            IInventoryService inventoryService,
            IWalletOperationService walletOperationService,
            IPlayerUpdateLevelService playerUpdateLevelService,
            ISkillsUpgradeService skillsUpgradeService,
            IProcessingBonusesService processingBonusesService,
            IActivationRecipesService activationRecipesService,
            int dungeonId, int levelId)
        {
            RegisterGameplayServices(
                itemsGeneratorService,
                walletOperationService,
                _statsRestorationService,
                playerUpdateLevelService,
                progressProviderService,
                skillsUpgradeService,
                processingBonusesService,
                activationRecipesService);
                
            DungeonBackground dungeonBackground = _uiFactory.CreateDungeonBackground();
            HudView hudView = CreateHUD();

            CreateGameplay(
                dungeonBackground,
                hudView,
                progressProviderService,
                processingAdsService,
                inventoryService,
                _statsRestorationService,
                dungeonId,
                levelId);
        }

        private void OnDestroy()
        {
            ClearGameplayServices();
        }

        private void RegisterGameplayServices(IItemsGeneratorService itemsGeneratorService,
            IWalletOperationService walletOperationService,
            IStatsRestorationService statsRestorationService,
            IPlayerUpdateLevelService playerUpdateLevelService,
            IProgressProviderService progressProviderService,
            ISkillsUpgradeService skillsUpgradeService,
            IProcessingBonusesService processingBonusesService,
            IActivationRecipesService activationRecipesService)
        {
            _gameplayServicesContainer = new ServicesContainer();
            
            _gameplayServicesContainer.Register<ICheckerCurrentStatsService>(
                new CheckerCurrentStatsService());
            _gameplayServicesContainer.Register<IStatsWasteService>(
                new StatsWasteService());
            _gameplayServicesContainer.Register<IAttackService>(
                new AttackService(
                    _gameplayServicesContainer.Single<IStatsWasteService>(),
                    statsRestorationService,
                    skillsUpgradeService,
                    processingBonusesService));

            RegisterProgressDungeonService(
                itemsGeneratorService,
                walletOperationService,
                statsRestorationService,
                playerUpdateLevelService,
                progressProviderService,
                activationRecipesService);
        }

        private void RegisterProgressDungeonService(IItemsGeneratorService itemsGeneratorService,
            IWalletOperationService walletOperationService,
            IStatsRestorationService statsRestorationService,
            IPlayerUpdateLevelService playerUpdateLevelService,
            IProgressProviderService progressProviderService,
            IActivationRecipesService activationRecipesService)
        {
            var service = new ProgressDungeonService(
                _staticDataService,
                progressProviderService,
                _gameplayFactory,
                _gameplayServicesContainer.Single<IAttackService>(),
                statsRestorationService,
                _gameplayServicesContainer.Single<ICheckerCurrentStatsService>(),
                playerUpdateLevelService,
                itemsGeneratorService,
                activationRecipesService,
                walletOperationService);
            _statsRestorationService.ProgressDungeonService = service;
            
            _gameplayServicesContainer.Register<IProgressDungeonService>(service);
        }

        private void CreateGameplay(DungeonBackground dungeonBackground,
            HudView hudView,
            IProgressProviderService progressProviderService,
            IProcessingAdsService processingAdsService,
            IInventoryService inventoryService,
            IStatsRestorationService statsRestorationService,
            int dungeonId, int levelId)
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
                dungeonBackground,
                hudView,
                _playerData,
                dungeonId,
                levelId);
            progressDungeonService.StartBattle();
            
            hudView.Initialize(
                _staticDataService,
                inventoryService,
                _gameplayServicesContainer.Single<IProgressDungeonService>(),
                processingAdsService,
                progressProviderService,
                statsRestorationService,
                _gameplayServicesContainer.Single<IAttackService>(),
                _playerData);
        }

        private HudView CreateHUD()
        {
            HudView hudView = _uiFactory.CreateHUD();
            hudView.Construct(_sceneProviderService);

            return hudView;
        }

        private void ClearGameplayServices()
        {
            _statsRestorationService.ProgressDungeonService = null;
            _gameplayServicesContainer.Clear();
            
            _gameplayServicesContainer = null;
        }
    }
}