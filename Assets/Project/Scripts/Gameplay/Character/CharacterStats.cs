using UndergroundFortress.Scripts.Gameplay.Stats;

namespace UndergroundFortress.Scripts.Gameplay.Character
{
    public class CharacterStats
    {
        private MainStats _mainStats;
        private CurrentStats _currentStats;

        public CharacterStats(MainStats mainStats,
            CurrentStats currentStats)
        {
            _mainStats = mainStats;
            _currentStats = currentStats;
        }

        public MainStats MainStats => _mainStats;
        public CurrentStats CurrentStats => _currentStats;
    }
}