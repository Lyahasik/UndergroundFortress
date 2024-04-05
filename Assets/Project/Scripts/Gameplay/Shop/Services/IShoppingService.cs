using UndergroundFortress.Core.Services;
using UndergroundFortress.UI.Inventory;

namespace UndergroundFortress.Gameplay.Shop
{
    public interface IShoppingService : IService
    {
        public void ShowItem(CellSaleView cellSale);
        public void SaleResource(CellSaleView cellSale, int number, int price);
        public void SaleEquipment(CellSaleView selectedCell, int price);
    }
}