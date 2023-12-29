namespace UndergroundFortress.Scripts.Gameplay.Characteristics.Services
{
    public class CheckerCurrentCharacteristicsService : ICheckerCurrentCharacteristicsService
    {
        public bool IsEnoughStamina(RealtimeCharacteristics realtimeCharacteristics)
        {
            if (realtimeCharacteristics.StaminaCost < realtimeCharacteristics.Stamina)
                return false;
            
            return true;
        }
    }
}