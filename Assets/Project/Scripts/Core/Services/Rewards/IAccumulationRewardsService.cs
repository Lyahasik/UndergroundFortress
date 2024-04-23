using UndergroundFortress.Gameplay.Inventory.Services;
using UndergroundFortress.UI.MainMenu;

namespace UndergroundFortress.Core.Services.Rewards
{
    public interface IAccumulationRewardsService : IService
    {
        public void Initialize(IInventoryService inventoryService, MainMenuView mainMenuView);
        public void ClaimRewards();
    }
}