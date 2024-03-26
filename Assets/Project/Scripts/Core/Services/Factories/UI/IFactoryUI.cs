using UndergroundFortress.UI.Craft;
using UndergroundFortress.UI.Hud;
using UndergroundFortress.UI.Information;
using UndergroundFortress.UI.Inventory;
using UndergroundFortress.UI.MainMenu;

namespace UndergroundFortress.Core.Services.Factories.UI
{
    public interface IUIFactory : IService
    {
        public MainMenuView CreateMainMenu();
        public InformationView CreateInformation();
        public HomeView CreateHome();
        public CraftView CreateCraft();
        public InventoryView CreateInventory();
        public HudView CreateHUD();
    }
}