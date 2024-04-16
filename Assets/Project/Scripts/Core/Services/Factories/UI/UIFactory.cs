using UndergroundFortress.Core.Services.StaticData;
using UndergroundFortress.UI.Craft;
using UndergroundFortress.UI.Hud;
using UndergroundFortress.UI.Information;
using UndergroundFortress.UI.Inventory;
using UndergroundFortress.UI.MainMenu;
using UndergroundFortress.UI.Shop;
using UndergroundFortress.UI.Skills;

namespace UndergroundFortress.Core.Services.Factories.UI
{
    public class UIFactory : Factory, IUIFactory
    {
        private readonly IStaticDataService _staticDataService;

        public UIFactory(IStaticDataService staticDataService)
        {
            _staticDataService = staticDataService;
        }

        public MainMenuView CreateMainMenu() => 
            PrefabInstantiate(_staticDataService.ForMainMenu().mainMenuViewPrefab);

        public InformationView CreateInformation() => 
            PrefabInstantiate(_staticDataService.ForMainMenu().informationViewPrefab);

        public HomeView CreateHome() => 
            PrefabInstantiate(_staticDataService.ForMainMenu().homeViewPrefab);
        public SkillsView CreateSkills() => 
            PrefabInstantiate(_staticDataService.ForMainMenu().skillsViewPrefab);

        public CraftView CreateCraft() => 
            PrefabInstantiate(_staticDataService.ForMainMenu().craftViewPrefab);

        public InventoryView CreateInventory() => 
            PrefabInstantiate(_staticDataService.ForMainMenu().inventoryViewPrefab);
        public ShopView CreateShop() => 
            PrefabInstantiate(_staticDataService.ForMainMenu().shopViewPrefab);
        public StartLevelView CreateStartLevel() => 
            PrefabInstantiate(_staticDataService.ForMainMenu().startLevelViewPrefab);

        public DungeonBackground CreateDungeonBackground() => 
            PrefabInstantiate(_staticDataService.ForLevel().dungeonBackgroundPrefab);

        public HudView CreateHUD() => 
            PrefabInstantiate(_staticDataService.ForLevel().hudViewPrefab);
    }
}