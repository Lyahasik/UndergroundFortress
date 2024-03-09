﻿using System;
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
        public event Action OnUpdateResources;
        public event Action<InventoryCellType, int, CellData> OnUpdateCell;

        public bool IsBagFull(bool isShowMessage = true);

        public bool IsBagFullForResource(ItemType itemType, int id);

        public void AddItem(ItemData itemData);

        public void RemoveItem(ItemData itemData);
        public void RemoveItemsById(int itemId, int requiredNumber);

        public int GetEmptyCellId();

        public void UpdateItemToCell(InventoryCellType cellType, in int id);

        public void ShowItem(ItemData itemData, InventoryCellType inventoryCellType);

        public int GetNumberItemsById(int id);
    }
}