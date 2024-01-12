using UndergroundFortress.Core.Services.StaticData;
using UndergroundFortress.UI.Craft;
using UndergroundFortress.UI.Hud;
using UndergroundFortress.UI.Information;
using UndergroundFortress.UI.MainMenu;

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

        public CraftView CreateCraft() => 
            PrefabInstantiate(_staticDataService.ForMainMenu().craftViewPrefab);

        public HudView CreateHUD() => 
            PrefabInstantiate(_staticDataService.ForLevel().hudViewPrefab);
    }
}