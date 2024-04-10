using UndergroundFortress.Gameplay.Inventory.Services;
using UndergroundFortress.Gameplay.Inventory.Wallet.Services;
using UndergroundFortress.UI.Information.Services;
using UndergroundFortress.UI.Inventory;

namespace UndergroundFortress.Gameplay.Shop
{
    public class ShoppingService : IShoppingService
    {
        private readonly IWalletOperationService _walletOperationService;
        private readonly IInventoryService _inventoryService;
        private readonly IInformationService _informationService;

        public ShoppingService(IWalletOperationService walletOperationService,
            IInventoryService inventoryService,
            IInformationService informationService)
        {
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
            _inventoryService.RemoveItemsByCell(cellSale, number);
            _walletOperationService.AddMoney1(price);
        }

        public void SaleEquipment(CellSaleView cellSale, int price)
        {
            _inventoryService.ClearCell(cellSale);
            _walletOperationService.AddMoney1(price);
        }

        public void Pay(MoneyType moneyType, int price)
        {
            _walletOperationService.RemoveMoney(moneyType, price);
        }
    }
}