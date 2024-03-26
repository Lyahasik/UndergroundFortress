using UnityEngine;

using UndergroundFortress.UI.Craft;
using UndergroundFortress.UI.Information;
using UndergroundFortress.UI.Inventory;

namespace UndergroundFortress.UI.MainMenu.StaticData
{
    [CreateAssetMenu(fileName = "MainMenuData", menuName = "Static data/Main menu")]
    public class MainMenuStaticData : ScriptableObject
    {
        public MainMenuView mainMenuViewPrefab;
        public InformationView informationViewPrefab;
        public HomeView homeViewPrefab;
        public CraftView craftViewPrefab;
        public InventoryView inventoryViewPrefab;
    }
}