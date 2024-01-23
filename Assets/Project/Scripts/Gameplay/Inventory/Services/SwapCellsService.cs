using System.Collections.Generic;
using UnityEngine;

using UndergroundFortress.Gameplay.Items;
using UndergroundFortress.UI.Inventory;

namespace UndergroundFortress.Gameplay.Inventory.Services
{
    public class SwapCellsService : ISwapCellsService
    {
        private readonly IInventoryService _inventoryService;

        public SwapCellsService(IInventoryService inventoryService)
        {
            _inventoryService = inventoryService;
        }

        public void TrySwapCells(CellInventoryView cell1, CellInventoryView cell2)
        {
            if (!TrySwapItems(cell1, cell2))
                return;

            ItemData itemDataTemporary = cell1.ItemData;
            Sprite iconTemporary = cell1.Icon;
            Sprite qualityTemporary = cell1.Quality;
            string numberTemporary = cell1.Number;
            
            cell1.SetValues(cell2.ItemData, cell2.Icon, cell2.Quality, cell2.Number);
            cell2.SetValues(itemDataTemporary, iconTemporary, qualityTemporary, numberTemporary);
        }
        
        private bool TrySwapItems(CellInventoryView cell1, CellInventoryView cell2) =>
            TrySwapFromBagToBag(cell1, cell2)
            || TrySwapFromBagToEquipment(cell1, cell2)
            || TrySwapFromEquipmentToBag(cell1, cell2);

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
                    (equipment[cell1.Id], bag[cell2.Id]) = (bag[cell2.Id], equipment[cell1.Id]);
                    return true;
                }
            }

            return false;
        }

        private bool TrySwapFromBagToBag(CellInventoryView cell1, CellInventoryView cell2)
        {
            if (cell1.InventoryCellType == InventoryCellType.Bag
                && cell2.InventoryCellType == InventoryCellType.Bag)
            {
                List<CellData> bag = _inventoryService.Bag;
                (bag[cell1.Id], bag[cell2.Id]) = (bag[cell2.Id], bag[cell1.Id]);

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
                    (bag[cell1.Id], equipment[cell2.Id]) = (equipment[cell2.Id], bag[cell1.Id]);

                    return true;
                }
            }

            return false;
        }
    }
}