using UnityEngine;

using UndergroundFortress.Core.Services;
using UndergroundFortress.Core.Services.Factories.UI;
using UndergroundFortress.Core.Services.Progress;
using UndergroundFortress.Core.Services.Scene;
using UndergroundFortress.Core.Services.StaticData;
using UndergroundFortress.Gameplay.Craft.Services;
using UndergroundFortress.Gameplay.Inventory.Services;
using UndergroundFortress.Gameplay.Items.Services;
using UndergroundFortress.UI.Craft;
using UndergroundFortress.UI.Information;
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

        public void Initialize(ISceneProviderService sceneProviderService)
        {
            RegisterServices();
            CreateMainMenu(sceneProviderService);
        }
        
        private void RegisterServices()
        {
            _mainMenuServicesContainer = new ServicesContainer();

            CreateInventory();
            
            _mainMenuServicesContainer.Register<ICraftService>(
                new CraftService(
                    _staticDataService,
                    _mainMenuServicesContainer.Single<IInventoryService>()
                    ));
            
            _mainMenuServicesContainer.Register<IItemsGeneratorService>(
                new ItemsGeneratorService(
                    _staticDataService,
                    _mainMenuServicesContainer.Single<IInventoryService>()
                    ));
        }
        
        private void CreateInventory()
        {
            InventoryService inventoryService = new InventoryService(_progressProviderService);
            inventoryService.Initialize();
            _mainMenuServicesContainer.Register<IInventoryService>(inventoryService);
        }

        private void CreateMainMenu(ISceneProviderService sceneProviderService)
        {
            InformationView information = _uiFactory.CreateInformation();
            information.Initialize();

            CraftView craft = _uiFactory.CreateCraft();
            craft.Construct(
                _staticDataService,
                _progressProviderService,
                _mainMenuServicesContainer.Single<ICraftService>(),
                information);
            craft.Initialize();

            InventoryView inventory = _uiFactory.CreateInventory();
            inventory.Construct(
                _staticDataService,
                _mainMenuServicesContainer.Single<IInventoryService>());
            inventory.Initialize();

            MainMenuView mainMenu = _uiFactory.CreateMainMenu();
            mainMenu.Construct(sceneProviderService, _mainMenuServicesContainer.Single<IItemsGeneratorService>());
            mainMenu.Initialize(craft, inventory);
        }
        
        private void ClearMainMenuServices()
        {
            _mainMenuServicesContainer.Clear();
            
            _mainMenuServicesContainer = null;
        }
    }
}