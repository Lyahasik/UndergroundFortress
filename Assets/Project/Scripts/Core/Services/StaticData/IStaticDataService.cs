using UndergroundFortress.Scripts.Gameplay.StaticData;
using UndergroundFortress.Scripts.MainMenu.StaticData;
using UndergroundFortress.Scripts.UI.StaticData;

namespace UndergroundFortress.Scripts.Core.Services.StaticData
{
    public interface IStaticDataService : IService
    {
        public UIStaticData ForUI();
        public MainMenuStaticData ForMainMenu();
        public LevelStaticData ForLevel();
    }
}