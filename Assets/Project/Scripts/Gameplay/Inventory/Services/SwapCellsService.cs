using System.Collections.Generic;

using UndergroundFortress.UI.Information.Services;
using UndergroundFortress.UI.Inventory;

namespace UndergroundFortress.Gameplay.Inventory.Services
{
    public class SwapCellsService : ISwapCellsService
    {
        private readonly IInventoryService _inventoryService;
        private readonly IInformationService _informationService;

        public SwapCellsService(IInventoryService inventoryService,
            IInformationService informationService)
        {
            _inventoryService = inventoryService;
            _informationService = informationService;
        }

        public void TrySwapCells(CellInventoryView cell1, CellInventoryView cell2)
        {
            if (TrySwapFromBagToBag(cell1, cell2)
                || TrySwapFromBagToEquipment(cell1, cell2)
                || TrySwapFromEquipmentToBag(cell1, cell2))
                return;
        }

        private bool TrySwapFromEquipmentToBag(CellInventoryView cell1, CellInventoryView cell2)
        {
            if (cell1.InventoryCellType == InventoryCellType.Equipment
                && cell2.InventoryCellType == InventoryCellType.Bag)
            {
                List<CellData> equipment = _inventoryService.Equipment;
                List<CellData> bag = _inventoryService.Bag;

                if (cell1.ItemType == cell2.ItemType
                    || cell2.ItemData == null)
                {
                    Swap(equipment, bag, cell1, cell2);
                    
                    return true;
                }
                
                if (!_inventoryService.IsBagFull())
                {
                    AddItemToEmptyCellBag(equipment, cell1);
                    
                    return true;
                }
                
                _informationService.ShowWarning("Bag is full.");
            }

            return false;
        }

        private bool TrySwapFromBagToBag(CellInventoryView cell1, CellInventoryView cell2)
        {
            if (cell1.InventoryCellType == InventoryCellType.Bag
                && cell2.InventoryCellType == InventoryCellType.Bag)
            {
                List<CellData> bag = _inventoryService.Bag;
                Swap(bag, bag, cell1, cell2);

                return true;
            }

            return false;
        }

        private bool TrySwapFromBagToEquipment(CellInventoryView cell1, CellInventoryView cell2)
        {
            if (cell1.InventoryCellType == InventoryCellType.Bag
                && cell2.InventoryCellType == InventoryCellType.Equipment)
            {
                if (cell2.ItemType == cell1.ItemData.Type)
                {
                    List<CellData> bag = _inventoryService.Bag;
                    List<CellData> equipment = _inventoryService.Equipment;
                    Swap(bag, equipment, cell1, cell2);

                    return true;
                }
            }

            return false;
        }

        private void AddItemToEmptyCellBag(IList<CellData> list1, CellInventoryView cell1)
        {
            int id2 = _inventoryService.GetEmptyCellId();
            List<CellData> bag = _inventoryService.Bag;
            
            (list1[cell1.Id], bag[id2]) = (bag[id2], list1[cell1.Id]);
            _inventoryService.UpdateItemToCell(cell1.InventoryCellType, cell1.Id);
            _inventoryService.UpdateItemToCell(InventoryCellType.Bag, id2);
        }

        private void Swap(IList<CellData> list1, IList<CellData> list2,
            CellInventoryView cell1, CellInventoryView cell2)
        {
            (list1[cell1.Id], list2[cell2.Id]) = (list2[cell2.Id], list1[cell1.Id]);
            _inventoryService.UpdateItemToCell(cell1.InventoryCellType, cell1.Id);
            _inventoryService.UpdateItemToCell(cell2.InventoryCellType, cell2.Id);
        }
    }
}