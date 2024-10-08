using UndergroundFortress.UI.Craft;
using UndergroundFortress.UI.Hud;
using UndergroundFortress.UI.Information;
using UndergroundFortress.UI.Inventory;
using UndergroundFortress.UI.MainMenu;
using UndergroundFortress.UI.Shop;
using UndergroundFortress.UI.Skills;
using UnityEngine;

namespace UndergroundFortress.Core.Services.Factories.UI
{
    public interface IUIFactory : IService
    {
        public MainMenuView CreateMainMenu();
        public InformationView CreateInformation();
        public HomeView CreateHome();
        public SkillsView CreateSkills();
        public CraftView CreateCraft();
        public InventoryView CreateInventory();
        public ShopView CreateShop();
        public BuffView CreateBuff(Transform parent);
        public StartLevelView CreateStartLevel();
        public TutorialView CreateTutorial();
        public DungeonBackground CreateDungeonBackground();
        public HudView CreateHUD();
    }
}