using UndergroundFortress.Core.Services;
using UndergroundFortress.Gameplay.Stats;

namespace UndergroundFortress.Gameplay.Character.Services
{
    public interface IProcessingPlayerStatsService : IService
    {
        public CharacterStats PlayerStats { get; }
        public void UpStat(in StatType type, in float value);
        public void DownStat(in StatType type, in float value);
        public void UpHealth(in float value);
        public void DownHealth(in float value);
    }
}