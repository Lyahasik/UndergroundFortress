using UndergroundFortress.Gameplay.Character;
using UnityEngine;

namespace UndergroundFortress.Gameplay.Stats.Services
{
    public class CheckerCurrentStatsService : ICheckerCurrentStatsService
    {
        public CheckerCurrentStatsService()
        {
            Debug.Log($"[{ GetType() }] initialize");
        }
        
        public bool IsEnoughStamina(CharacterStats characterStats) => 
            characterStats.MainStats[StatType.StaminaCost] <= characterStats.CurrentStats.Stamina;
    }
}