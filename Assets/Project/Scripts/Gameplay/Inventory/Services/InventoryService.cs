using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

using UndergroundFortress.Constants;
using UndergroundFortress.Core.Services.Progress;
using UndergroundFortress.Gameplay.Items;
using UndergroundFortress.UI.Inventory;

namespace UndergroundFortress.Gameplay.Inventory.Services
{
    public class InventoryService : IInventoryService
    {
        private readonly IProgressProviderService _progressProviderService;

        private Dictionary<InventoryCellType, List<CellData>> _inventory;

        public List<CellData> Bag => _inventory[InventoryCellType.Bag];
        public List<CellData> Equipment => _inventory[InventoryCellType.Equipment];

        public event Action<InventoryCellType, int, CellData> OnUpdateCell;

        public InventoryService(IProgressProviderService progressProviderService)
        {
            _progressProviderService = progressProviderService;
        }

        public void Initialize()
        {
            _inventory = new Dictionary<InventoryCellType, List<CellData>>();
            
            _inventory.Add(InventoryCellType.Equipment, _progressProviderService.ProgressData.Equipment);
            _inventory.Add(InventoryCellType.Bag, _progressProviderService.ProgressData.Bag);
        }

        public bool IsBagFull() => 
            _inventory[InventoryCellType.Bag].All(cellData => cellData.ItemData != null);
        public bool IsBagFullForResource(ItemType itemType, int id)
        {
            if (!IsBagFull())
                return false;
            
            List<CellData> cells = _inventory[InventoryCellType.Bag]
                .FindAll(cellData =>
                    cellData.ItemData != null
                    && cellData.ItemData.Type == itemType
                    && cellData.ItemData.Id == id);

            return cells.All(cellData => cellData.Number >= cellData.ItemData.MaxNumberForCell);
        }

        public void AddItem(ItemData itemData)
        {
            if (itemData.Type == ItemType.Resource)
                AddResourceToBag(itemData);
            else
                AddNewItem(itemData);
        }

        public void RemoveItem(ItemData itemData)
        {
            int itemBagId = GetItemId(itemData);
            
            _inventory[InventoryCellType.Bag][itemBagId].ItemData = null;
            _inventory[InventoryCellType.Bag][itemBagId].Number = 0;
            
            UpdateItemToCell(InventoryCellType.Bag, itemBagId);
        }

        public void UpdateItemToCell(InventoryCellType cellType, in int id) => 
            OnUpdateCell?.Invoke(cellType, id, _inventory[cellType][id]);

        private void AddResourceToBag(ItemData itemData)
        {
            int itemBagId = GetResourceId(itemData);

            if (itemBagId != ConstantValues.ERROR_ID)
            {
                _inventory[InventoryCellType.Bag][itemBagId].Number++;
                UpdateItemToCell(InventoryCellType.Bag, itemBagId);
            }
            else
                AddNewItem(itemData);
        }

        private int GetItemId(ItemData itemData)
        {
            List<CellData> bag = _inventory[InventoryCellType.Bag];

            for (int i = 0; i < bag.Count; i++)
            {
                if (bag[i].ItemData == itemData)
                    return i;
            }

            return ConstantValues.ERROR_ID;
        }

        private int GetResourceId(ItemData itemData)
        {
            List<CellData> bag = _inventory[InventoryCellType.Bag];
            
            for (int i = 0; i < bag.Count; i++)
            {
                if (bag[i].ItemData == null)
                    continue;
                    
                if (bag[i].ItemData.Id == itemData.Id
                    && bag[i].Number < itemData.MaxNumberForCell)
                {
                    return i;
                }
            }

            return ConstantValues.ERROR_ID;
        }

        private void AddNewItem(ItemData itemData)
        {
            List<CellData> bag = _inventory[InventoryCellType.Bag];

            for (int i = 0; i < bag.Count; i++)
            {
                if (bag[i].ItemData == null)
                {
                    bag[i].ItemData = itemData;
                    bag[i].Number = 1;
                    UpdateItemToCell(InventoryCellType.Bag, i);
                    return;
                }
            }
            
            Debug.LogWarning($"Failed to add item to bag");
        }
    }
}