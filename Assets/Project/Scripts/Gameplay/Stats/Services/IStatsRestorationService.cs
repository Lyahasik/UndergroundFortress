using UndergroundFortress.Core.Services;
using UndergroundFortress.Core.Services.Bonuses;
using UndergroundFortress.Gameplay.Character;
using UndergroundFortress.Gameplay.Dungeons.Services;

namespace UndergroundFortress.Gameplay.Stats.Services
{
    public interface IStatsRestorationService : IService
    {
        public IProgressDungeonService ProgressDungeonService { set; }
        public IProcessingBonusesService ProcessingBonusesService { set; }

        public void AddStats(CharacterStats stats);
        public void RemoveStats(CharacterStats stats);
        public void RestoreFullHealth(CharacterStats stats);
        public void RestoreHealth(CharacterStats stats, int value);
        public void RestoreStats();
    }
}