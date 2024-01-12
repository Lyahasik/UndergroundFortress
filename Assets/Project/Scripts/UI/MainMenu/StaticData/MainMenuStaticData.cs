using UnityEngine;

using UndergroundFortress.UI.Craft;
using UndergroundFortress.UI.Information;

namespace UndergroundFortress.UI.MainMenu.StaticData
{
    [CreateAssetMenu(fileName = "MainMenuData", menuName = "Static data/Main menu")]
    public class MainMenuStaticData : ScriptableObject
    {
        public MainMenuView mainMenuViewPrefab;
        public InformationView informationViewPrefab;
        public CraftView craftViewPrefab;
    }
}