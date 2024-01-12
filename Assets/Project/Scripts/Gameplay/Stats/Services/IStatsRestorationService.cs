using UndergroundFortress.Core.Services;
using UndergroundFortress.Gameplay.Character;

namespace UndergroundFortress.Gameplay.Stats.Services
{
    public interface IStatsRestorationService : IService
    {
        void AddStats(CharacterStats stats);
        void RemoveStats(CharacterStats stats);
    }
}