using UndergroundFortress.Gameplay.Inventory.Wallet.Services;
using UndergroundFortress.Gameplay.Items.Services;

namespace UndergroundFortress.Core.Services.Scene
{
    public interface ISceneProviderService : IService
    {
        public void LoadMainScene();
        public void LoadLevel(IItemsGeneratorService itemsGeneratorService, 
            IWalletOperationService walletOperationService,
            int idDungeon,
            int idLevel);
    }
}