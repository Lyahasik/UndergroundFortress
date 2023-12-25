using UnityEngine;
using UnityEngine.Serialization;

using UndergroundFortress.Scripts.UI.MainMenu;

namespace UndergroundFortress.Scripts.MainMenu.StaticData
{
    [CreateAssetMenu(fileName = "MainMenuData", menuName = "Static data/Main menu")]
    public class MainMenuStaticData : ScriptableObject
    {
        [FormerlySerializedAs("mainMenuPrefab")] public MainMenuView mainMenuViewPrefab;
    }
}