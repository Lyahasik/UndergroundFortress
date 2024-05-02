using UnityEngine;

using UndergroundFortress.Core.Localization;
using UndergroundFortress.Core.Services;
using UndergroundFortress.Core.Services.Ads;
using UndergroundFortress.Core.Services.Bonuses;
using UndergroundFortress.Core.Services.Characters;
using UndergroundFortress.Core.Services.Factories.UI;
using UndergroundFortress.Core.Services.Progress;
using UndergroundFortress.Core.Services.Rewards;
using UndergroundFortress.Core.Services.Scene;
using UndergroundFortress.Core.Services.StaticData;
using UndergroundFortress.Core.Update;
using UndergroundFortress.Gameplay.Character.Services;
using UndergroundFortress.Gameplay.Craft.Services;
using UndergroundFortress.Gameplay.Inventory.Services;
using UndergroundFortress.Gameplay.Inventory.Wallet.Services;
using UndergroundFortress.Gameplay.Items.Services;
using UndergroundFortress.Gameplay.Shop;
using UndergroundFortress.Gameplay.Skills.Services;
using UndergroundFortress.Gameplay.Stats.Services;
using UndergroundFortress.Gameplay.Tutorial.Services;
using UndergroundFortress.UI.Craft;
using UndergroundFortress.UI.Information;
using UndergroundFortress.UI.Information.Services;
using UndergroundFortress.UI.Inventory;
using UndergroundFortress.UI.Shop;
using UndergroundFortress.UI.Skills;

namespace UndergroundFortress.UI.MainMenu
{
    public class InitializerMainMenu : MonoBehaviour
    {
        private IStaticDataService _staticDataService;
        private ILocalizationService _localizationService;
        private IProcessingAdsService _processingAdsService;
        private IUIFactory _uiFactory;
        private IProgressProviderService _progressProviderService;

        private ServicesContainer _mainMenuServicesContainer;

        private void OnDestroy()
        {
            ClearMainMenuServices();
        }

        public void Construct(IStaticDataService staticDataService, ILocalizationService localizationService,
            IProcessingAdsService processingAdsService,
            IUIFactory uiFactory,
            IProgressProviderService progressProviderService)
        {
            _staticDataService = staticDataService;
            _localizationService = localizationService;
            _processingAdsService = processingAdsService;
            _uiFactory = uiFactory;
            _progressProviderService = progressProviderService;
        }

        public void Initialize(UpdateHandler updateHandler,
            IProcessingPlayerStatsService processingPlayerStatsService,
            IPlayerDressingService playerDressingService,
            ISceneProviderService sceneProviderService,
            IStatsRestorationService statsRestorationService,
            IAccumulationRewardsService accumulationRewardsService) 
        {
            RegisterServices(updateHandler, playerDressingService, statsRestorationService);
            CreateMainMenu(
                processingPlayerStatsService,
                sceneProviderService,
                accumulationRewardsService);
        }
        
        private void RegisterServices(UpdateHandler updateHandler,
            IPlayerDressingService playerDressingService,
            IStatsRestorationService statsRestorationService)
        {
            _mainMenuServicesContainer = new ServicesContainer();
            
            _mainMenuServicesContainer.Register<IInformationService>(new InformationService());
            RegisterWalletOperationService();

            RegisterSkillsUpgradeService();
            
            RegisterActivationRecipesService();
            RegisterInventoryService();
            RegisterShoppingService();

            _mainMenuServicesContainer.Register<ISwapCellsService>(
                new SwapCellsService(
                    _progressProviderService,
                    _mainMenuServicesContainer.Single<IInventoryService>(),
                    playerDressingService));

            RegisterMovingService();

            _mainMenuServicesContainer.Register<IItemsGeneratorService>(
                new ItemsGeneratorService(
                    _staticDataService,
                    _mainMenuServicesContainer.Single<IInventoryService>()
                    ));
            
            RegisterDailyRewardsService();

            RegisterProcessingBonusesService(updateHandler);
            statsRestorationService.ProcessingBonusesService = _mainMenuServicesContainer.Single<IProcessingBonusesService>();

            _mainMenuServicesContainer.Register<ICraftService>(
                new CraftService(
                    _mainMenuServicesContainer.Single<IInventoryService>(),
                    _mainMenuServicesContainer.Single<IItemsGeneratorService>()));
            
            _mainMenuServicesContainer.Register<IProgressTutorialService>(new ProgressTutorialService(
                _localizationService,
                _progressProviderService,
                _mainMenuServicesContainer.Single<IItemsGeneratorService>()));
        }

        private void RegisterDailyRewardsService()
        {
            var service = new DailyRewardsService(
                _staticDataService,
                _progressProviderService,
                _mainMenuServicesContainer.Single<IItemsGeneratorService>(),
                _mainMenuServicesContainer.Single<IWalletOperationService>(),
                _mainMenuServicesContainer.Single<IInformationService>()
            );
            _mainMenuServicesContainer.Register<IDailyRewardsService>(service);
        }

        private void RegisterProcessingBonusesService(UpdateHandler updateHandler)
        {
            ProcessingBonusesService processingBonusesService = new ProcessingBonusesService(
                _staticDataService,
                _progressProviderService,
                _mainMenuServicesContainer.Single<IItemsGeneratorService>());
            updateHandler.AddUpdatedObject(processingBonusesService);
            _mainMenuServicesContainer.Register<IProcessingBonusesService>(processingBonusesService);
        }

        private void RegisterSkillsUpgradeService()
        {
            var service = new SkillsUpgradeService(_staticDataService, _progressProviderService);
            service.Initialize();
            
            _mainMenuServicesContainer.Register<ISkillsUpgradeService>(service);
        }

        private void RegisterWalletOperationService()
        {
            var service = new WalletOperationService();
            service.Construct(
                _localizationService,
                _progressProviderService,
                _mainMenuServicesContainer.Single<IInformationService>());
            service.Initialize();
            
            _mainMenuServicesContainer.Register<IWalletOperationService>(service);
        }

        private void RegisterMovingService()
        {
            MovingItemService movingItemService = new MovingItemService(
                _mainMenuServicesContainer.Single<ISwapCellsService>());
            _mainMenuServicesContainer.Register<IMovingItemService>(movingItemService);
        }

        private void RegisterActivationRecipesService()
        {
            ActivationRecipesService activationRecipesService = new ActivationRecipesService(
                _staticDataService,
                _progressProviderService);
            activationRecipesService.Initialize();
            _mainMenuServicesContainer.Register<IActivationRecipesService>(activationRecipesService);
        }

        private void RegisterInventoryService()
        {
            InventoryService inventoryService = new InventoryService(
                _staticDataService,
                _localizationService,
                _progressProviderService,
                _mainMenuServicesContainer.Single<IInformationService>(),
                _mainMenuServicesContainer.Single<IWalletOperationService>());
            inventoryService.Initialize();
            _mainMenuServicesContainer.Register<IInventoryService>(inventoryService);
        }

        private void RegisterShoppingService()
        {
            ShoppingService shoppingService = new ShoppingService(
                _staticDataService,
                _progressProviderService,
                _mainMenuServicesContainer.Single<IWalletOperationService>(),
                _mainMenuServicesContainer.Single<IInventoryService>(),
                _mainMenuServicesContainer.Single<IInformationService>());
            _mainMenuServicesContainer.Register<IShoppingService>(shoppingService);
        }

        private void CreateMainMenu(IProcessingPlayerStatsService processingPlayerStatsService,
            ISceneProviderService sceneProviderService,
            IAccumulationRewardsService accumulationRewardsService)
        {
            HomeView home = _uiFactory.CreateHome();
            home.Construct(processingPlayerStatsService);
            home.Initialize(_staticDataService, _localizationService);
            
            SkillsView skills = _uiFactory.CreateSkills();
            skills.Construct(
                _mainMenuServicesContainer.Single<ISkillsUpgradeService>(),
                _progressProviderService);
            skills.Initialize(
                _staticDataService, 
                _processingAdsService,
                _mainMenuServicesContainer.Single<IInformationService>());

            CraftView craft = _uiFactory.CreateCraft();
            craft.Construct(
                _staticDataService,
                _progressProviderService,
                _mainMenuServicesContainer.Single<ICraftService>(),
                _mainMenuServicesContainer.Single<IInventoryService>(),
                _mainMenuServicesContainer.Single<IInformationService>());
            craft.Initialize(
                _localizationService,
                _mainMenuServicesContainer.Single<IActivationRecipesService>(),
                _mainMenuServicesContainer.Single<IProgressTutorialService>());

            InventoryView inventory = _uiFactory.CreateInventory();
            inventory.Construct(
                _staticDataService,
                _mainMenuServicesContainer.Single<IInventoryService>(),
                _mainMenuServicesContainer.Single<IMovingItemService>());
            inventory.Initialize();
            _mainMenuServicesContainer.Single<ISwapCellsService>().Initialize(inventory);

            ShopView shop = _uiFactory.CreateShop();
            shop.Construct(_mainMenuServicesContainer.Single<IInventoryService>());
            shop.Initialize(_staticDataService, _mainMenuServicesContainer.Single<IShoppingService>());

            StartLevelView startLevel = _uiFactory.CreateStartLevel();
            startLevel.Construct(
                sceneProviderService, 
                _mainMenuServicesContainer.Single<IItemsGeneratorService>(),
                _mainMenuServicesContainer.Single<IInventoryService>(),
                _mainMenuServicesContainer.Single<ISkillsUpgradeService>(),
                _mainMenuServicesContainer.Single<IProcessingBonusesService>(),
                _mainMenuServicesContainer.Single<IActivationRecipesService>(),
                _mainMenuServicesContainer.Single<IProgressTutorialService>());
            startLevel.Initialize(_staticDataService, _localizationService, _progressProviderService);

            MainMenuView mainMenu = _uiFactory.CreateMainMenu();
            mainMenu.Construct(
                _uiFactory,
                _processingAdsService,
                _localizationService,
                _mainMenuServicesContainer.Single<IInformationService>());
            mainMenu.Initialize(home, skills, craft, inventory, shop, startLevel, _staticDataService, _progressProviderService);
            
            mainMenu.PlayerHealthFill.Subscribe(processingPlayerStatsService.PlayerStats);
            
            InformationView information = _uiFactory.CreateInformation();
            information.Initialize(_staticDataService,
                _localizationService,
                _processingAdsService,
                _progressProviderService,
                _mainMenuServicesContainer.Single<ISkillsUpgradeService>(), 
                _mainMenuServicesContainer.Single<IItemsGeneratorService>(),
                _mainMenuServicesContainer.Single<IInventoryService>(),
                _mainMenuServicesContainer.Single<IShoppingService>(),
                accumulationRewardsService,
                _mainMenuServicesContainer.Single<IDailyRewardsService>());
            _mainMenuServicesContainer.Single<IInformationService>().Initialize(information);
            _mainMenuServicesContainer.Single<IDailyRewardsService>().Initialize();
            
            accumulationRewardsService.Initialize(
                _mainMenuServicesContainer.Single<IInventoryService>(),
                mainMenu);
            
            IMovingItemService movingItemService = _mainMenuServicesContainer.Single<IMovingItemService>();
            movingItemService.Initialize(information.CellItemView);
            movingItemService.Subscribe(inventory.BagActiveArea);
            movingItemService.Subscribe(inventory.EquipmentActiveArea);

            TutorialView tutorialView = _uiFactory.CreateTutorial();
            _mainMenuServicesContainer.Single<IProgressTutorialService>().Initialize(
                mainMenu,
                craft,
                inventory,
                shop,
                _mainMenuServicesContainer.Single<IShoppingService>(),
                _mainMenuServicesContainer.Single<ISkillsUpgradeService>(),
                skills,
                tutorialView);

            _mainMenuServicesContainer.Single<IProcessingBonusesService>().Initialize(mainMenu);
        }
        
        private void ClearMainMenuServices()
        {
            _mainMenuServicesContainer.Clear();
            
            _mainMenuServicesContainer = null;
        }
    }
}