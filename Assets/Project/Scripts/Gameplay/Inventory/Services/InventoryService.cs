using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

using UndergroundFortress.Constants;
using UndergroundFortress.Core.Services.Progress;
using UndergroundFortress.Gameplay.Inventory.Wallet.Services;
using UndergroundFortress.Gameplay.Items;
using UndergroundFortress.Gameplay.Items.Equipment;
using UndergroundFortress.UI.Information.Services;
using UndergroundFortress.UI.Inventory;

namespace UndergroundFortress.Gameplay.Inventory.Services
{
    public class InventoryService : IInventoryService
    {
        private readonly IProgressProviderService _progressProviderService;
        private readonly IInformationService _informationService;
        private readonly IWalletOperationService _walletOperationService;

        private Dictionary<InventoryCellType, List<CellData>> _inventory;

        public IWalletOperationService WalletOperationService => _walletOperationService;
        
        public List<CellData> Bag => _inventory[InventoryCellType.Bag];
        public List<CellData> Equipment => _inventory[InventoryCellType.Equipment];
        
        public event Action OnUpdateResources;
        public event Action<InventoryCellType, int, CellData> OnUpdateCell;

        public InventoryService(IProgressProviderService progressProviderService,
            IInformationService informationService,
            IWalletOperationService walletOperationService)
        {
            _progressProviderService = progressProviderService;
            _informationService = informationService;
            _walletOperationService = walletOperationService;
        }

        public void Initialize()
        {
            _inventory = new Dictionary<InventoryCellType, List<CellData>>();
            
            _inventory.Add(InventoryCellType.Equipment, _progressProviderService.ProgressData.Equipment);
            _inventory.Add(InventoryCellType.Bag, _progressProviderService.ProgressData.Bag);
        }

        public bool IsBagFull(bool isShowMessage = true)
        {
            bool isFull = _inventory[InventoryCellType.Bag].All(cellData => cellData.ItemData != null);
            
            if (isShowMessage && isFull)
                _informationService.ShowWarning("Bag is full.");

            return isFull;
        }

        public bool IsBagFullForResource(ItemType itemType, int id)
        {
            if (!IsBagFull(false))
                return false;
            
            List<CellData> cells = _inventory[InventoryCellType.Bag]
                .FindAll(cellData =>
                    cellData.ItemData != null
                    && cellData.ItemData.Type == itemType
                    && cellData.ItemData.Id == id);

            bool isFull = cells.All(cellData => cellData.Number >= cellData.ItemData.MaxNumberForCell);
            
            if (isFull)
                _informationService.ShowWarning("Bag is full.");

            return isFull;
        }

        public void AddItem(ItemData itemData)
        {
            if (itemData.Type >= ItemType.Resource)
                AddCountedItemToBag(itemData);
            else
                AddNewItem(itemData);
        }

        public void RemoveItem(ItemData itemData)
        {
            if (itemData == null)
                return;
            
            int itemBagId = GetItemId(itemData);
            
            _inventory[InventoryCellType.Bag][itemBagId].ItemData = null;
            _inventory[InventoryCellType.Bag][itemBagId].Number = 0;
            
            UpdateItemToCell(InventoryCellType.Bag, itemBagId);
        }

        public void RemoveItemsById(int itemId, int requiredNumber)
        {
            DecrementItems(itemId, requiredNumber);
            UpdateResources();
        }

        public void RemoveItemsByType(ItemType itemType, int requiredNumber)
        {
            int itemId = GetItemIdByType(itemType);
            
            DecrementItems(itemId, requiredNumber);
            UpdateResources();
        }

        public int GetEmptyCellId()
        {
            for (int i = 0; i < Bag.Count; i++)
            {
                if (Bag[i].ItemData == null)
                    return i;
            }
            
            Debug.LogWarning($"Not found empty cell.");
            
            return ConstantValues.ERROR_ID;
        }

        public List<ItemData> GetCrystals() =>
            Bag
                .Where(data => data.ItemData?.Type is >= ItemType.ResourceDodgeSet and <= ItemType.ResourceStunSet)
                .Select(cellData => cellData.ItemData)
                .ToList();

        public void UpdateResources() => 
            OnUpdateResources?.Invoke();

        public void UpdateItemToCell(InventoryCellType cellType, in int id) => 
            OnUpdateCell?.Invoke(cellType, id, _inventory[cellType][id]);

        public void ShowItem(ItemData itemData, InventoryCellType inventoryCellType)
        {
            var isBag = inventoryCellType == InventoryCellType.Bag;
            if (!isBag || !TryEquipmentComparison(itemData))
                _informationService.ShowItem(itemData, !isBag);
        }

        public int GetNumberItemsById(int id) => 
            Bag.Where(cellData => cellData?.ItemData?.Id == id).Sum(cellData => cellData.Number);

        private void DecrementItems(int itemId, int number)
        {
            while (number > 0)
            {
                CellData cellData = GetCellByItemId(itemId);

                if (number >= cellData.Number)
                {
                    number -= cellData.Number;
                    RemoveItem(cellData.ItemData);
                }
                else
                {
                    cellData.Number -= number;
                    return;
                }
            }
            
        }

        private CellData GetCellByItemId(int itemId) => 
            Bag.Find(data => data.ItemData?.Id == itemId);

        private bool TryEquipmentComparison(ItemData itemData)
        {
            if (itemData is not EquipmentData equipmentData)
                return false;

            CellData cellData = Equipment.Find(data =>
                data.ItemData != null
                && data.ItemData.Type == equipmentData.Type);
            
            if (cellData == null)
                return false;
            
            _informationService.ShowEquipmentComparison(cellData.ItemData as EquipmentData, equipmentData);
            
            return true;
        }

        private void AddCountedItemToBag(ItemData itemData)
        {
            int itemBagId = GetCountedItemId(itemData);

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

        private int GetItemIdByType(ItemType itemType) => 
            Bag.Find(data => data.ItemData?.Type == itemType).ItemData.Id;

        private int GetCountedItemId(ItemData itemData)
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