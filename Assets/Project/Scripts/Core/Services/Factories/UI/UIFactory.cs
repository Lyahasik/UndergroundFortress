using UnityEngine;

using UndergroundFortress.Scripts.Core.Services.StaticData;
using UndergroundFortress.Scripts.Gameplay.StaticData;
using UndergroundFortress.Scripts.MainMenu.StaticData;
using UndergroundFortress.Scripts.UI.Hud;
using UndergroundFortress.Scripts.UI.MainMenu;

namespace UndergroundFortress.Scripts.Core.Services.Factories.UI
{
    public class UIFactory : IUIFactory
    {
        private readonly IStaticDataService _staticDataService;

        public UIFactory(IStaticDataService staticDataService)
        {
            _staticDataService = staticDataService;
        }

        public MainMenuView CreateMainMenu()
        {
            MainMenuStaticData mainMenuData = _staticDataService.ForMainMenu();
            MainMenuView mainMenu = Object.Instantiate(mainMenuData.mainMenuViewPrefab);

            return mainMenu;
        }

        public HudView CreateHUD()
        {
            LevelStaticData levelData = _staticDataService.ForLevel();
            HudView hudView = Object.Instantiate(levelData.hudViewPrefab);

            return hudView;
        }
    }
}