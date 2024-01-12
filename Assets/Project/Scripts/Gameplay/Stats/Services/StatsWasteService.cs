using System;

using UndergroundFortress.Gameplay.Character;

namespace UndergroundFortress.Gameplay.Stats.Services
{
    public class StatsWasteService : IStatsWasteService
    {
        public void WasteHealth(CharacterStats characterStats, in float value)
        {
            float newHealth = characterStats.CurrentStats.Health - value;
            characterStats.SetCurrentHealth(Math.Clamp(newHealth, 0, float.MaxValue));
        }
        
        public void WasteStamina(CharacterStats characterStats, in float value)
        {
            float newStamina = characterStats.CurrentStats.Stamina - value;
            characterStats.SetCurrentStamina(Math.Clamp(newStamina, 0, float.MaxValue));
        }
    }
}