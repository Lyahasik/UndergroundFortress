using UndergroundFortress.Gameplay.Character;

namespace UndergroundFortress.Gameplay.Stats.Services
{
    public class CheckerCurrentStatsService : ICheckerCurrentStatsService
    {
        public bool IsEnoughStamina(CharacterStats characterStats) => 
            characterStats.MainStats[StatType.StaminaCost] <= characterStats.CurrentStats.Stamina;
    }
}