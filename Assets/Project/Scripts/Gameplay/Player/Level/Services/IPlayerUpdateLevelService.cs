using UndergroundFortress.Core.Services;

namespace UndergroundFortress.Gameplay.Player.Level.Services
{
    public interface IPlayerUpdateLevelService : IService
    {
        void Initialize();
        void IncreaseExperience(int value);
    }
}