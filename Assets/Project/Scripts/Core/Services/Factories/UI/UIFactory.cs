using UndergroundFortress.Scripts.Core.Services.StaticData;
using UndergroundFortress.Scripts.UI.Hud;
using UndergroundFortress.Scripts.UI.MainMenu;

namespace UndergroundFortress.Scripts.Core.Services.Factories.UI
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

        public HudView CreateHUD() => 
            PrefabInstantiate(_staticDataService.ForLevel().hudViewPrefab);
    }
}