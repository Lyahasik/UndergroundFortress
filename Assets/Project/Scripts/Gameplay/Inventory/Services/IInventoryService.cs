using System;
using System.Collections.Generic;

using UndergroundFortress.Core.Services;
using UndergroundFortress.Gameplay.Inventory.Wallet.Services;
using UndergroundFortress.Gameplay.Items;
using UndergroundFortress.UI.Inventory;

namespace UndergroundFortress.Gameplay.Inventory.Services
{
    public interface IInventoryService : IService
    {
        public List<CellData> Bag { get; }

        public List<CellData> Equipment { get; }
        public IWalletOperationService WalletOperationService { get; }
        public event Action OnUpdateResources;
        public event Action<InventoryCellType, int, CellData> OnUpdateCell;

        public bool IsBagFull(bool isShowMessage = true);
        public bool IsBagFullForResource(ItemType itemType, int id);
        public bool IsBagFullForItems(List<ItemNumberData> purchasesNumberData);

        public void AddItem(ItemData itemData);
        public void AddItems(ItemData itemData, int number);
        public void AddItemById(int itemId);
        public void AddItemsById(int itemId, int number);

        public void RemoveItem(InventoryCellType cellType, ItemData itemData);
        public void ClearCell(InventoryCellType cellType, CellInventoryView cellInventoryView);
        public void RemoveItemsByCell(InventoryCellType cellType, CellInventoryView cellInventoryView, int requiredNumber);
        public void RemoveItemsById(InventoryCellType cellType, int itemId, int requiredNumber);
        public void RemoveItemsByType(InventoryCellType cellType, ItemType itemType, int requiredNumber);

        public int GetEmptyCellId();
        public List<ItemData> GetCrystals();
        
        public void UpdateItemToCell(InventoryCellType cellType, in int id);

        public void ShowItem(ItemData itemData, InventoryCellType inventoryCellType);

        public int GetNumberItemsById(int id);
    }
}