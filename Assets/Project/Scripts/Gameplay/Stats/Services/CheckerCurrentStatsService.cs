using UndergroundFortress.Scripts.Gameplay.Character;

namespace UndergroundFortress.Scripts.Gameplay.Stats.Services
{
    public class CheckerCurrentStatsService : ICheckerCurrentStatsService
    {
        public bool IsEnoughStamina(CharacterStats characterStats) => 
            characterStats.MainStats.staminaCost <= characterStats.CurrentStats.Stamina;
    }
}