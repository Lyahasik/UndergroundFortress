using System.Collections.Generic;
using System.Linq;
using UnityEngine;

using UndergroundFortress.Core.Services.Progress;
using UndergroundFortress.Core.Services.StaticData;
using UndergroundFortress.Gameplay.Inventory.Services;
using UndergroundFortress.Gameplay.Inventory.Wallet.Services;
using UndergroundFortress.Gameplay.Items;
using UndergroundFortress.Gameplay.Tutorial.Services;
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
        
        private ProgressTutorialService _progressTutorialService;

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

            Debug.Log($"[{ GetType() }] initialize");
        }

        public void ShowSaleItem(CellSaleView cellSale)
        {
            bool isCapping = true;
            if (cellSale.ItemData.Type == ItemType.ResourceCritSet)
                isCapping = !TryCheckTutorial();
            
            _informationService.ShowSaleItem(cellSale, isCapping);
        }

        public void ShowPurchase(CellPurchaseView cellPurchase)
        {
            if (!_inventoryService.WalletOperationService.IsEnoughMoney(cellPurchase.PurchaseStaticData.moneyType, cellPurchase.PurchaseStaticData.price)
                || _inventoryService.IsBagFullForItems(cellPurchase.PurchaseStaticData.rewardData.items))
                return;
            
            bool isCapping = !TryCheckTutorial();
            _informationService.ShowPurchase(cellPurchase, isCapping);
        }

        public void SaleResource(CellSaleView cellSale, int number, int price)
        {
            if (cellSale.ItemData.Type == ItemType.ResourceCritSet)
                TryCheckTutorial();
            
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
            TryCheckTutorial();
        }
        
        public void ActivateTutorial(ProgressTutorialService progressTutorialService)
        {
            _progressTutorialService = progressTutorialService;
        }

        public void DeactivateTutorial()
        {
            _progressTutorialService = null;
        }

        private bool TryCheckTutorial()
        {
            if (_progressTutorialService == null)
                return false;
            
            _progressTutorialService.SuccessStep();
            return true;
        }
    }
}