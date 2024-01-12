using UndergroundFortress.Core.Services;
using UndergroundFortress.Gameplay.Character;

namespace UndergroundFortress.Gameplay.Stats.Services
{
    public interface IStatsWasteService : IService
    {
        void WasteHealth(CharacterStats characterStats, in float value);
        public void WasteStamina(CharacterStats characterStats, in float value);
    }
}