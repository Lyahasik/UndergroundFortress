using UndergroundFortress.Scripts.Core.Services;
using UndergroundFortress.Scripts.Gameplay.Character;

namespace UndergroundFortress.Scripts.Gameplay.Stats.Services
{
    public interface IStatsRestorationService : IService
    {
        void AddStats(CharacterStats stats);
        void RemoveStats(CharacterStats stats);
    }
}