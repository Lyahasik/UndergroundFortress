using UnityEngine;

using UndergroundFortress.Core.Services;
using UndergroundFortress.Core.Services.Factories.UI;
using UndergroundFortress.Core.Services.Progress;
using UndergroundFortress.Core.Services.Scene;
using UndergroundFortress.Core.Services.StaticData;
using UndergroundFortress.Gameplay.Character.Services;
using UndergroundFortress.Gameplay.Craft.Services;
using UndergroundFortress.Gameplay.Inventory.Services;
using UndergroundFortress.Gameplay.Inventory.Wallet.Services;
using UndergroundFortress.Gameplay.Items.Services;
using UndergroundFortress.UI.Craft;
using UndergroundFortress.UI.Information;
using UndergroundFortress.UI.Information.Services;
using UndergroundFortress.UI.Inventory;

namespace UndergroundFortress.UI.MainMenu
{
    public class InitializerMainMenu : MonoBehaviour
    {
        private IStaticDataService _staticDataService;
        private IUIFactory _uiFactory;
        private IProgressProviderService _progressProviderService;
        
        private ServicesContainer _mainMenuServicesContainer;

        private void OnDestroy()
        {
            ClearMainMenuServices();
        }

        public void Construct(IStaticDataService staticDataService,
            IUIFactory uiFactory,
            IProgressProviderService progressProviderService)
        {
            _staticDataService = staticDataService;
            _uiFactory = uiFactory;
            _progressProviderService = progressProviderService;
        }

        public void Initialize(IProgressProviderService progressProviderService,
            IProcessingPlayerStatsService processingPlayerStatsService,
            ISceneProviderService sceneProviderService) 
        {
            RegisterServices(progressProviderService);
            CreateMainMenu(progressProviderService, processingPlayerStatsService, sceneProviderService);
        }
        
        private void RegisterServices(IProgressProviderService progressProviderService)
        {
            _mainMenuServicesContainer = new ServicesContainer();
            
            _mainMenuServicesContainer.Register<IInformationService>(new InformationService());
            RegisterWalletOperationService(progressProviderService);

            RegisterActivationRecipesService(progressProviderService);
            RegisterInventoryService();

            _mainMenuServicesContainer.Register<ISwapCellsService>(
                new SwapCellsService(
                    progressProviderService,
                    _mainMenuServicesContainer.Single<IInventoryService>()));

            RegisterMovingService();

            _mainMenuServicesContainer.Register<IItemsGeneratorService>(
                new ItemsGeneratorService(
                    _staticDataService,
                    _mainMenuServicesContainer.Single<IInventoryService>()
                    ));
            
            _mainMenuServicesContainer.Register<ICraftService>(
                new CraftService(
                    _mainMenuServicesContainer.Single<IInventoryService>(),
                    _mainMenuServicesContainer.Single<IItemsGeneratorService>()));
        }

        private void RegisterWalletOperationService(IProgressProviderService progressProviderService)
        {
            var service = new WalletOperationService();
            service.Construct(progressProviderService);
            service.Initialize();
            
            _mainMenuServicesContainer.Register<IWalletOperationService>(service);
        }

        private void RegisterMovingService()
        {
            MovingItemService movingItemService = new MovingItemService(
                _mainMenuServicesContainer.Single<ISwapCellsService>());
            _mainMenuServicesContainer.Register<IMovingItemService>(movingItemService);
        }

        private void RegisterActivationRecipesService(IProgressProviderService progressProviderService)
        {
            ActivationRecipesService activationRecipesService = new ActivationRecipesService(
                _staticDataService,
                progressProviderService);
            activationRecipesService.Initialize();
            _mainMenuServicesContainer.Register<IActivationRecipesService>(activationRecipesService);
        }

        private void RegisterInventoryService()
        {
            InventoryService inventoryService = new InventoryService(
                _staticDataService,
                _progressProviderService,
                _mainMenuServicesContainer.Single<IInformationService>(),
                _mainMenuServicesContainer.Single<IWalletOperationService>());
            inventoryService.Initialize();
            _mainMenuServicesContainer.Register<IInventoryService>(inventoryService);
        }

        private void CreateMainMenu(IProgressProviderService progressProviderService,
            IProcessingPlayerStatsService processingPlayerStatsService,
            ISceneProviderService sceneProviderService)
        {
            InformationView information = _uiFactory.CreateInformation();
            information.Initialize(_staticDataService);
            _mainMenuServicesContainer.Single<IInformationService>().Initialize(information);
            
            HomeView home = _uiFactory.CreateHome();
            home.Construct(processingPlayerStatsService);
            home.Initialize(_staticDataService);

            CraftView craft = _uiFactory.CreateCraft();
            craft.Construct(
                _staticDataService,
                _progressProviderService,
                _mainMenuServicesContainer.Single<ICraftService>(),
                _mainMenuServicesContainer.Single<IInventoryService>(),
                _mainMenuServicesContainer.Single<IInformationService>());
            craft.Initialize(_mainMenuServicesContainer.Single<IActivationRecipesService>());

            InventoryView inventory = _uiFactory.CreateInventory();
            inventory.Construct(
                _staticDataService,
                _mainMenuServicesContainer.Single<IInventoryService>(),
                _mainMenuServicesContainer.Single<IMovingItemService>());
            inventory.Initialize();
            _mainMenuServicesContainer.Single<ISwapCellsService>().Initialize(inventory);

            IMovingItemService movingItemService = _mainMenuServicesContainer.Single<IMovingItemService>();
            movingItemService.Initialize(information.CellItemView);
            movingItemService.Subscribe(inventory.BagActiveArea);
            movingItemService.Subscribe(inventory.EquipmentActiveArea);

            MainMenuView mainMenu = _uiFactory.CreateMainMenu();
            mainMenu.Construct(sceneProviderService, 
                _mainMenuServicesContainer.Single<IItemsGeneratorService>(),
                _mainMenuServicesContainer.Single<IActivationRecipesService>());
            mainMenu.Initialize(home, craft, inventory, progressProviderService);
        }
        
        private void ClearMainMenuServices()
        {
            _mainMenuServicesContainer.Clear();
            
            _mainMenuServicesContainer = null;
        }
    }
}