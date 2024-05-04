using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

using UndergroundFortress.Constants;
using UndergroundFortress.Core.Localization;
using UndergroundFortress.Core.Progress;
using UndergroundFortress.Core.Services.Progress;
using UndergroundFortress.Core.Services.StaticData;
using UndergroundFortress.Extensions;
using UndergroundFortress.Gameplay.Inventory.Wallet.Services;
using UndergroundFortress.Gameplay.Items;
using UndergroundFortress.UI.Information.Services;
using UndergroundFortress.UI.Inventory;

namespace UndergroundFortress.Gameplay.Inventory.Services
{
    public class InventoryService : IInventoryService, IWritingProgress
    {
        private readonly IStaticDataService _staticDataService;
        private readonly ILocalizationService _localizationService;
        private readonly IProgressProviderService _progressProviderService;
        private readonly IInformationService _informationService;
        private readonly IWalletOperationService _walletOperationService;

        private Dictionary<InventoryCellType, List<CellData>> _inventory;

        public IWalletOperationService WalletOperationService => _walletOperationService;
        
        public List<CellData> Bag => _inventory[InventoryCellType.Bag];
        public List<CellData> Equipment => _inventory[InventoryCellType.Equipment];
        
        public event Action OnUpdateResources;
        public event Action<InventoryCellType, int, CellData> OnUpdateCell;

        public InventoryService(IStaticDataService staticDataService,
            ILocalizationService localizationService,
            IProgressProviderService progressProviderService,
            IInformationService informationService,
            IWalletOperationService walletOperationService)
        {
            _staticDataService = staticDataService;
            _localizationService = localizationService;
            _progressProviderService = progressProviderService;
            _informationService = informationService;
            _walletOperationService = walletOperationService;
        }

        public void Initialize()
        {
            _inventory = new Dictionary<InventoryCellType, List<CellData>>();
            
            Register(_progressProviderService);
            
            Debug.Log($"[{ GetType() }] initialize");
        }

        public void Register(IProgressProviderService progressProviderService) => 
            progressProviderService.Register(this);

        public void LoadProgress(ProgressData progress)
        {
            _inventory.Add(InventoryCellType.Equipment, progress.Equipment);
            _inventory.Add(InventoryCellType.Bag, progress.Bag);
        }

        public void UpdateProgress(ProgressData progress) {}

        public void WriteProgress()
        {
            _progressProviderService.ProgressData.FilledNumberBag
                = Bag.Where(data => data.ItemData != null).ToList().Count;
            
            _progressProviderService.SaveProgress();
        }

        public bool IsBagFull(bool isShowMessage = true)
        {
            bool isFull = _inventory[InventoryCellType.Bag].All(cellData => cellData.ItemData != null);
            
            if (isShowMessage && isFull)
                _informationService.ShowWarning(_localizationService.LocaleMain(ConstantValues.KEY_LOCALE_BAG_FULL));

            return isFull;
        }

        public bool IsBagFullForResource(ItemType itemType, int id)
        {
            if (!IsBagFull(false))
                return false;
            
            List<CellData> cells = Bag
                .FindAll(cellData =>
                    cellData.ItemData != null
                    && cellData.ItemData.Type == itemType
                    && cellData.ItemData.Id == id);

            bool isFull = cells.All(cellData => cellData.Number >= _staticDataService.GetItemMaxNumberForCellById(cellData.ItemData.Id));
            
            if (isFull)
                _informationService.ShowWarning(_localizationService.LocaleMain(ConstantValues.KEY_LOCALE_BAG_FULL));

            return isFull;
        }

        public bool IsBagFullForItems(List<ItemNumberData> purchasesNumberData)
        {
            if (purchasesNumberData.All(data => data.itemData.type == ItemType.Money))
                return false;
            
            int requiredCells = 0;
            List<CellData> partiallyFilledCells
                = Bag.FindAll(cellData => cellData.ItemData != null 
                                          && !cellData.ItemData.Type.IsEquipment()
                                          && cellData.Number < _staticDataService.GetItemMaxNumberForCellById(cellData.ItemData.Id));
            foreach (ItemNumberData purchaseNumberData in purchasesNumberData)
            {
                var itemStaticData = _staticDataService.GetItemById(purchaseNumberData.itemData.id);
                
                if (itemStaticData.type.IsEquipment())
                {
                    requiredCells += purchaseNumberData.number;
                    continue;
                }
                
                int numberItem = purchaseNumberData.number;
                if (partiallyFilledCells.All(data => data.ItemData.Id != purchaseNumberData.itemData.id))
                {
                    requiredCells = IncrementRequiredCells(purchaseNumberData.itemData.id, requiredCells, numberItem);
                    continue;
                }

                partiallyFilledCells.ForEach(data =>
                {
                    if (numberItem > 0
                        && data.ItemData.Id == purchaseNumberData.itemData.id)
                        numberItem -= _staticDataService.GetItemMaxNumberForCellById(data.ItemData.Id) - data.Number;
                });

                if (numberItem <= 0)
                    continue;

                requiredCells = IncrementRequiredCells(purchaseNumberData.itemData.id, requiredCells, numberItem);
            }

            bool isFull = requiredCells > GetNumberEmptyCells();
            
            if (isFull)
                _informationService.ShowWarning(_localizationService.LocaleMain(ConstantValues.KEY_LOCALE_BAG_FULL));

            return isFull;
        }

        private int IncrementRequiredCells(int itemId, int requiredCells, int numberItem)
        {
            int maxNumber = _staticDataService.GetItemMaxNumberForCellById(itemId);
            requiredCells += numberItem / maxNumber;
            if (numberItem % maxNumber > 0)
                requiredCells++;
            
            return requiredCells;
        }

        private int GetNumberEmptyCells() => 
            Bag.Count(data => data.ItemData == null);

        public void AddItem(ItemData itemData)
        {
            if (itemData.Type >= ItemType.Resource)
                AddCountedItemToBag(itemData);
            else
                AddNewBagItem(itemData);
            
            WriteProgress();
        }

        public void AddItems(ItemData itemData, int number)
        {
            for (int i = 0; i < number; i++)
            {
                if (itemData.Type >= ItemType.Resource)
                    AddCountedItemToBag(itemData);
                else
                    AddNewBagItem(itemData);
            }

            WriteProgress();
        }

        public void AddItemById(int itemId) => 
            AddItem(GetCellByItemId(InventoryCellType.Bag, itemId).ItemData);

        public void AddItemsById(int itemId, int number)
        {
            var itemData = GetCellByItemId(InventoryCellType.Bag, itemId)?.ItemData;

            if (itemData == null)
                _staticDataService.GetItemById(itemId);
                
            for (int i = number; i > 0; i--)
                AddItem(itemData);
        }

        public void RemoveItem(InventoryCellType cellType, ItemData itemData)
        {
            if (itemData == null)
                return;
            
            int itemBagId = GetItemId(cellType, itemData);
            
            _inventory[cellType][itemBagId].ItemData = null;
            _inventory[cellType][itemBagId].Number = 0;
            
            UpdateItemToCell(cellType, itemBagId);
            
            WriteProgress();
        }

        public void ClearCell(InventoryCellType cellType, CellInventoryView cellInventoryView)
        {
            _inventory[cellType][cellInventoryView.Id].ItemData = null;
            _inventory[cellType][cellInventoryView.Id].Number = 0;
            
            UpdateItemToCell(cellType, cellInventoryView.Id);
            
            WriteProgress();
        }

        public void RemoveItemsByCell(InventoryCellType cellType, CellInventoryView cellInventoryView, int requiredNumber)
        {
            if (requiredNumber == _inventory[cellType][cellInventoryView.Id].Number)
            {
                ClearCell(cellType, cellInventoryView);
            }
            else
            {
                _inventory[cellType][cellInventoryView.Id].Number -= requiredNumber;
            
                UpdateItemToCell(cellType, cellInventoryView.Id);
            
                WriteProgress();
            }
        }

        public void RemoveItemsById(InventoryCellType cellType, int itemId, int requiredNumber)
        {
            DecrementItems(cellType, itemId, requiredNumber);
            UpdateResources();
        }

        public void RemoveItemsByType(InventoryCellType cellType, ItemType itemType, int requiredNumber)
        {
            int itemId = GetItemIdByType(cellType, itemType);
            
            DecrementItems(cellType, itemId, requiredNumber);
            UpdateResources();
            
            WriteProgress();
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
                _informationService.ShowItem(itemData);
        }

        public int GetNumberItemsById(int id) => 
            Bag.Where(cellData => cellData?.ItemData?.Id == id).Sum(cellData => cellData.Number);

        private void DecrementItems(InventoryCellType cellType, int itemId, int number)
        {
            while (number > 0)
            {
                CellData cellData = GetCellByItemId(cellType, itemId);

                if (number >= cellData.Number)
                {
                    number -= cellData.Number;
                    RemoveItem(cellType, cellData.ItemData);
                }
                else
                {
                    cellData.Number -= number;
                    UpdateItemToCell(cellType, GetItemId(cellType, cellData.ItemData));
                    
                    WriteProgress();
                    return;
                }
            }
        }

        private CellData GetCellByItemId(InventoryCellType cellType, int itemId) => 
            _inventory[cellType].Find(data => data.ItemData?.Id == itemId);

        private bool TryEquipmentComparison(ItemData itemData)
        {
            if (!itemData.Type.IsEquipment())
                return false;

            CellData cellData = Equipment.Find(data =>
                data.ItemData != null
                && data.ItemData.Type == itemData.Type);
            
            if (cellData == null)
                return false;
            
            _informationService.ShowEquipmentComparison(cellData.ItemData, itemData);
            
            return true;
        }

        private void AddCountedItemToBag(ItemData itemData)
        {
            int itemBagId = GetCountedItemId(InventoryCellType.Bag, itemData);

            if (itemBagId != ConstantValues.ERROR_ID)
            {
                _inventory[InventoryCellType.Bag][itemBagId].Number++;
                UpdateItemToCell(InventoryCellType.Bag, itemBagId);
            }
            else
                AddNewBagItem(itemData);
        }

        private int GetItemId(InventoryCellType cellType, ItemData itemData)
        {
            List<CellData> cellsData = _inventory[cellType];

            for (int i = 0; i < cellsData.Count; i++)
            {
                if (cellsData[i].ItemData == itemData)
                    return i;
            }

            return ConstantValues.ERROR_ID;
        }

        private int GetItemIdByType(InventoryCellType cellType, ItemType itemType) => 
            _inventory[cellType].Find(data => data.ItemData?.Type == itemType).ItemData.Id;

        private int GetCountedItemId(InventoryCellType cellType, ItemData itemData)
        {
            List<CellData> cellsData = _inventory[cellType];
            
            for (int i = 0; i < cellsData.Count; i++)
            {
                if (cellsData[i].ItemData == null)
                    continue;
                    
                if (cellsData[i].ItemData.Id == itemData.Id
                    && cellsData[i].Number < _staticDataService.GetItemMaxNumberForCellById(itemData.Id))
                {
                    return i;
                }
            }

            return ConstantValues.ERROR_ID;
        }

        private void AddNewBagItem(ItemData itemData)
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