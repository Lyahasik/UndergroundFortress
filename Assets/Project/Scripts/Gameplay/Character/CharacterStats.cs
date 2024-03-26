using System;
using System.Collections.Generic;
using UndergroundFortress.Gameplay.Stats;

namespace UndergroundFortress.Gameplay.Character
{
    public class CharacterStats
    {
        private Dictionary<StatType, float> _mainStats;
        private CurrentStats _currentStats;

        public event Action<CharacterStats> OnUpdateCurrent;

        public Dictionary<StatType, float> MainStats => _mainStats;
        public CurrentStats CurrentStats => _currentStats;

        public void Initialize()
        {
            _mainStats = new Dictionary<StatType, float>();
        }

        public void UpdateCurrentStats()
        {
            _currentStats = new CurrentStats(_mainStats[StatType.Health], _mainStats[StatType.Stamina]);
            UpdateCurrent();
        }

        public void UpdateCurrent() => 
            OnUpdateCurrent?.Invoke(this);

        public void SetCurrentHealth(in float value)
        {
            _currentStats.Health = value;
            UpdateCurrent();
        }

        public void SetCurrentStamina(in float value)
        {
            _currentStats.Stamina = value;
            UpdateCurrent();
        }
    }
}