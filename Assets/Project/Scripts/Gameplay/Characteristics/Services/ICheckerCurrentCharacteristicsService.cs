using UndergroundFortress.Scripts.Core.Services;

namespace UndergroundFortress.Scripts.Gameplay.Characteristics.Services
{
    public interface ICheckerCurrentCharacteristicsService : IService
    {
        public bool IsEnoughStamina(RealtimeCharacteristics realtimeCharacteristics);
    }
}