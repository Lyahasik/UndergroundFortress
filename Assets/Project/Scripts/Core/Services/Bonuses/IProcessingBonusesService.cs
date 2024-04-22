using UndergroundFortress.UI.MainMenu;

namespace UndergroundFortress.Core.Services.Bonuses
{
    public interface IProcessingBonusesService : IService
    {
        public void Initialize(MainMenuView mainMenuView);
        public bool IsBuffActivate(BonusType bonusType);
        public float GetBuffValue(BonusType bonusType);
        public float GetBuffLeftToLive(BonusType bonusType);
    }
}