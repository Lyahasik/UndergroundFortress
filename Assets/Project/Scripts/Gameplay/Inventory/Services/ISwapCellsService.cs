﻿using UndergroundFortress.Core.Services;
using UndergroundFortress.UI.Inventory;

namespace UndergroundFortress.Gameplay.Inventory.Services
{
    public interface ISwapCellsService : IService
    {
        public void TrySwapCells(CellInventoryView cell1, CellInventoryView cell2);
        public void Initialize(InventoryView inventoryView);
    }
}