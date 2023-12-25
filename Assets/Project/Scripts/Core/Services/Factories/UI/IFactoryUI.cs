using UndergroundFortress.Scripts.UI.Hud;
using UndergroundFortress.Scripts.UI.MainMenu;

namespace UndergroundFortress.Scripts.Core.Services.Factories.UI
{
    public interface IUIFactory : IService
    {
        public MainMenuView CreateMainMenu();
        public HudView CreateHUD();
    }
}