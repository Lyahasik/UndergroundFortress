using System;
using System.Collections.Generic;

using UndergroundFortress.Core.Services;
using UndergroundFortress.Gameplay.Items;
using UndergroundFortress.UI.Inventory;

namespace UndergroundFortress.Gameplay.Inventory.Services
{
    public interface IInventoryService : IService
    {
        public List<CellData> Bag { get; }
        public List<CellData> Equipment { get; }

        public void AddItem(ItemData itemData);
        public void UpdateItemToCell(InventoryCellType cellType, in int id);
        public event Action<InventoryCellType, int, CellData> OnUpdateCell;
    }
}