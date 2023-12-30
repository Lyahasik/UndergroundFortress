using UndergroundFortress.Scripts.Core.Services;
using UndergroundFortress.Scripts.Gameplay.Character;

namespace UndergroundFortress.Scripts.Gameplay.Stats.Services
{
    public interface IStatsWasteService : IService
    {
        void WasteHealth(CharacterStats characterStats, in float value);
        public void WasteStamina(CharacterStats characterStats, in float value);
    }
}