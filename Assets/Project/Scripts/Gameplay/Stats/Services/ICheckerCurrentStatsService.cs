using UndergroundFortress.Core.Services;
using UndergroundFortress.Gameplay.Character;

namespace UndergroundFortress.Gameplay.Stats.Services
{
    public interface ICheckerCurrentStatsService : IService
    {
        public bool IsEnoughStamina(CharacterStats characterStats);
    }
}