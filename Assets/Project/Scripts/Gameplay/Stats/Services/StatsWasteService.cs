using System;

using UndergroundFortress.Scripts.Gameplay.Character;

namespace UndergroundFortress.Scripts.Gameplay.Stats.Services
{
    public class StatsWasteService : IStatsWasteService
    {
        public void WasteHealth(CharacterStats characterStats, in float value)
        {
            float newHealth = characterStats.CurrentStats.Health - value;
            characterStats.CurrentStats.Health = Math.Clamp(newHealth, 0, float.MaxValue);
            
            characterStats.Update();
        }
        
        public void WasteStamina(CharacterStats characterStats, in float value)
        {
            float newStamina = characterStats.CurrentStats.Stamina - value;
            characterStats.CurrentStats.Stamina = Math.Clamp(newStamina, 0, float.MaxValue);
            
            characterStats.Update();
        }
    }
}