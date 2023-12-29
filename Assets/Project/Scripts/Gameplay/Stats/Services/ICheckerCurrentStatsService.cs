using UndergroundFortress.Scripts.Core.Services;
using UndergroundFortress.Scripts.Gameplay.Character;

namespace UndergroundFortress.Scripts.Gameplay.Stats.Services
{
    public interface ICheckerCurrentStatsService : IService
    {
        public bool IsEnoughStamina(CharacterStats characterStats);
    }
}