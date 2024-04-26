using UndergroundFortress.Core.Services;
using UndergroundFortress.Gameplay.Stats;

namespace UndergroundFortress.Gameplay.Character.Services
{
    public interface IProcessingPlayerStatsService : IService
    {
        public CharacterStats PlayerStats { get; }
        public void UpStatEquipment(StatType type, float value);
        public void DownStatEquipment(StatType type, float value);
        public void UpHealthEquipment(in float value);
        public void DownHealthEquipment(in float value);
    }
}