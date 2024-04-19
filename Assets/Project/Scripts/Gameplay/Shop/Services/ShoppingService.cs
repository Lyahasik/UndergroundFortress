using System.Collections.Generic;
using System.Linq;

using UndergroundFortress.Core.Services.Progress;
using UndergroundFortress.Core.Services.StaticData;
using UndergroundFortress.Gameplay.Inventory.Services;
using UndergroundFortress.Gameplay.Inventory.Wallet.Services;
using UndergroundFortress.UI.Information.Services;
using UndergroundFortress.UI.Inventory;

namespace UndergroundFortress.Gameplay.Shop
{
    public class ShoppingService : IShoppingService
    {
        private readonly IStaticDataService _staticDataService;
        private readonly IProgressProviderService _progressProviderService;
        private readonly IWalletOperationService _walletOperationService;
        private readonly IInventoryService _inventoryService;
        private readonly IInformationService _informationService;

        public ShoppingService(IStaticDataService staticDataService,
            IProgressProviderService progressProviderService,
            IWalletOperationService walletOperationService,
            IInventoryService inventoryService,
            IInformationService informationService)
        {
            _staticDataService = staticDataService;
            _progressProviderService = progressProviderService;
            _walletOperationService = walletOperationService;
            _inventoryService = inventoryService;
            _informationService = informationService;
        }

        public void ShowSaleItem(CellSaleView cellSale)
        {
            _informationService.ShowSaleItem(cellSale);
        }

        public void ShowPurchase(CellPurchaseView cellPurchase)
        {
            if (_inventoryService.IsBagFullForItems(cellPurchase.PurchaseStaticData.rewardData.items))
                return;
            
            _informationService.ShowPurchase(cellPurchase);
        }

        public void SaleResource(CellSaleView cellSale, int number, int price)
        {
            _inventoryService.RemoveItemsByCell(InventoryCellType.Bag, cellSale, number);
            _walletOperationService.AddMoney1(price);
        }

        public void SaleEquipment(CellSaleView cellSale, int price)
        {
            _inventoryService.ClearCell(InventoryCellType.Bag, cellSale);
            _walletOperationService.AddMoney1(price);
        }

        public List<PurchaseStaticData> GetPurchaseBagByMoneyType(MoneyType moneyType)
        {
            int maxDungeonId = _progressProviderService.ProgressData.Dungeons.Keys.Max();
            
            List<PurchaseStaticData> bag = _staticDataService
                .ForPurchasesByMoneyType(moneyType)
                .Where(data => data.dungeonIdUnlock <= maxDungeonId 
                               && (data.dungeonIdLock == 0
                                   || data.dungeonIdLock > 0 && data.dungeonIdLock > maxDungeonId)
                )
                .ToList();

            return bag;
        }

        public void Pay(MoneyType moneyType, int price)
        {
            _walletOperationService.RemoveMoney(moneyType, price);
        }
    }
}