using System.Collections.Generic;

using UndergroundFortress.Core.Services;
using UndergroundFortress.UI.Inventory;

namespace UndergroundFortress.Gameplay.Shop
{
    public interface IShoppingService : IService
    {
        public void ShowSaleItem(CellSaleView cellSale);
        public void ShowPurchase(CellPurchaseView cellPurchaseView);
        public void Pay(MoneyType moneyType, int price);
        public void SaleResource(CellSaleView cellSale, int number, int price);
        public void SaleEquipment(CellSaleView selectedCell, int price);
        public List<PurchaseStaticData> GetPurchaseBagByMoneyType(MoneyType moneyType);
    }
}