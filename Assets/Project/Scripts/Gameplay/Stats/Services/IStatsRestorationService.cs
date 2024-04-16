using UndergroundFortress.Core.Services;
using UndergroundFortress.Gameplay.Character;

namespace UndergroundFortress.Gameplay.Stats.Services
{
    public interface IStatsRestorationService : IService
    {
        public void AddStats(CharacterStats stats);
        public void RemoveStats(CharacterStats stats);
        public void RestoreFullHealth(CharacterStats stats);
        public void RestoreStats();
    }
}