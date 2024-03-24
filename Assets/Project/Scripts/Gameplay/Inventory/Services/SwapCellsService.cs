using System.Collections.Generic;

using UndergroundFortress.Core.Progress;
using UndergroundFortress.Core.Services.Progress;
using UndergroundFortress.Gameplay.Items;
using UndergroundFortress.UI.Inventory;

namespace UndergroundFortress.Gameplay.Inventory.Services
{
    public class SwapCellsService : ISwapCellsService, IWritingProgress
    {
        private readonly IProgressProviderService _progressProviderService;
        private readonly IInventoryService _inventoryService;
        private InventoryView _inventoryView;
        
        private CellInventoryView _leftHandCell;
        private CellInventoryView _rightHandCell;

        public SwapCellsService(IProgressProviderService progressProviderService, IInventoryService inventoryService)
        {
            _progressProviderService = progressProviderService;
            _inventoryService = inventoryService;
        }

        public void Initialize(InventoryView inventoryView)
        {
            _inventoryView = inventoryView;
            
            _leftHandCell = _inventoryView.GetCellEquipmentByItemType(ItemType.Sword);
            _rightHandCell = _inventoryView.GetCellEquipmentByItemType(ItemType.Shield);
            
            Register(_progressProviderService);
        }

        public void Register(IProgressProviderService progressProviderService)
        {
            progressProviderService.Register(this);
        }

        public void LoadProgress(ProgressData progress) {}

        public void UpdateProgress(ProgressData progress) {}

        public void WriteProgress()
        {
            _progressProviderService.SaveProgress();
        }

        public void TrySwapCells(CellInventoryView cell1, CellInventoryView cell2)
        {
            if (TrySwapFromBagToBag(cell1, cell2)
                || TrySwapFromBagToEquipment(cell1, cell2)
                || TrySwapFromEquipmentToBag(cell1, cell2)
                || TrySwapFromEquipmentToEquipment(cell1, cell2))
                return;
        }

        private bool TrySwapFromBagToBag(CellInventoryView cell1, CellInventoryView cell2)
        {
            if (cell1.InventoryCellType == InventoryCellType.Bag
                && cell2.InventoryCellType == InventoryCellType.Bag)
            {
                List<CellData> bag = _inventoryService.Bag;
                return Swap(bag, bag, cell1, cell2);
            }

            return false;
        }

        private bool TrySwapFromBagToEquipment(CellInventoryView cellBag, CellInventoryView cellEquipment)
        {
            if (cellBag.InventoryCellType == InventoryCellType.Bag
                && cellEquipment.InventoryCellType == InventoryCellType.Equipment)
            {
                List<CellData> bag = _inventoryService.Bag;
                List<CellData> equipment = _inventoryService.Equipment;
                
                if (TryEquippingHands(bag, equipment, cellBag, cellEquipment))
                    return true;
                
                if (cellEquipment.ItemType == cellBag.ItemData.Type)
                    return Swap(bag, equipment, cellBag, cellEquipment);
            }

            return false;
        }

        private bool TrySwapFromEquipmentToBag(CellInventoryView cellEquipment, CellInventoryView cellBag)
        {
            if (cellEquipment.InventoryCellType == InventoryCellType.Equipment
                && cellBag.InventoryCellType == InventoryCellType.Bag)
            {
                List<CellData> equipment = _inventoryService.Equipment;
                List<CellData> bag = _inventoryService.Bag;

                if ((IsLeftHandCell(cellEquipment) || IsRightHandCell(cellEquipment))
                    && cellBag.ItemData?.Type == ItemType.TwoHandedWeapon)
                    return TryEquipTwoHandedWeapon(bag, equipment, cellBag, _leftHandCell);

                if (cellEquipment.ItemData?.Type == ItemType.TwoHandedWeapon)
                {
                    if (IsLeftHandItem(cellBag))
                        return Swap(equipment, bag, _leftHandCell, cellBag);

                    Swap(equipment, bag, _rightHandCell, cellBag);
                    AddItemToEmptyCellBag(_leftHandCell);

                    return true;
                }
                
                if ((IsLeftHandCell(cellEquipment) && IsLeftHandItem(cellBag))
                    || cellBag.ItemData == null
                    || cellBag.ItemData?.Type == cellEquipment.ItemData?.Type)
                    return Swap(equipment, bag, cellEquipment, cellBag);

                if (!_inventoryService.IsBagFull())
                {
                    AddItemToEmptyCellBag(cellEquipment);
                    
                    return true;
                }
            }

            return false;
        }

        private bool TrySwapFromEquipmentToEquipment(CellInventoryView cell1, CellInventoryView cell2)
        {
            if (cell1.InventoryCellType != InventoryCellType.Equipment
                || cell2.InventoryCellType != InventoryCellType.Equipment)
                return false;
            
            List<CellData> equipment = _inventoryService.Equipment;
                
            return cell1.ItemData?.Type == cell2.ItemType
                   && Swap(equipment, equipment, cell1, cell2);
        }

        private bool TryEquippingHands(List<CellData> bag, List<CellData> equipment,
            CellInventoryView cellBag, CellInventoryView cellEquipment)
        {
            return TryEquipTwoHandedWeapon(bag, equipment, cellBag, cellEquipment)
                   || TryEquippingLeftHand(bag, equipment, cellBag, cellEquipment)
                   || TryEquippingRightHand(bag, equipment, cellBag, cellEquipment);
        }

        private bool TryEquipTwoHandedWeapon(List<CellData> bag, List<CellData> equipment,
            CellInventoryView cellBag, CellInventoryView cellEquipment)
        {
            if (cellBag.ItemType != ItemType.TwoHandedWeapon)
                return false;
            
            if (IsLeftHandCell(cellEquipment))
            {
                if (!IsFilledCell(_rightHandCell))
                    return Swap(bag, equipment, cellBag, cellEquipment);

                if (!IsFilledCell(cellEquipment))
                {
                    Swap(bag, equipment, cellBag, cellEquipment);
                    AddItemToEmptyCellBag(_rightHandCell);

                    return true;
                }

                if (_inventoryService.IsBagFull())
                    return false;
                
                AddItemToEmptyCellBag(_rightHandCell);

                return Swap(bag, equipment, cellBag, cellEquipment);
            }
            
            if (_leftHandCell.ItemData?.Type == ItemType.TwoHandedWeapon
                || !IsFilledCell(_rightHandCell))
                return Swap(bag, equipment, cellBag, _leftHandCell);

            if (IsFilledCell(_leftHandCell))
            {
                if (_inventoryService.IsBagFull())
                    return false;
                    
                Swap(bag, equipment, cellBag, _leftHandCell);
                AddItemToEmptyCellBag(_rightHandCell);

                return true;
            }
                
            Swap(bag, equipment, cellBag, _leftHandCell);
            AddItemToEmptyCellBag(_rightHandCell);

            return true;
        }

        private bool TryEquippingLeftHand(List<CellData> bag, List<CellData> equipment,
            CellInventoryView cellBag, CellInventoryView cellEquipment)
        {
            if (!IsLeftHandCell(cellEquipment)
                || !IsLeftHandItem(cellBag))
                return false;
            
            return Swap(bag, equipment, cellBag, cellEquipment);
        }

        private bool TryEquippingRightHand(List<CellData> bag, List<CellData> equipment,
            CellInventoryView cellBag, CellInventoryView rightHandCell)
        {
            if (!IsRightHandCell(rightHandCell)
                || !IsRightHandItem(cellBag))
                return false;

            if (_leftHandCell.ItemData?.Type != ItemType.TwoHandedWeapon)
                return Swap(bag, equipment, cellBag, rightHandCell);

            Swap(bag, equipment, cellBag, rightHandCell);
            AddItemToEmptyCellBag(_leftHandCell);

            return true;
        }

        private void AddItemToEmptyCellBag(CellInventoryView cell)
        {
            int id2 = _inventoryService.GetEmptyCellId();
            List<CellData> equipment = _inventoryService.Equipment;
            List<CellData> bag = _inventoryService.Bag;
            
            (equipment[cell.Id], bag[id2]) = (bag[id2], equipment[cell.Id]);
            _inventoryService.UpdateItemToCell(cell.InventoryCellType, cell.Id);
            _inventoryService.UpdateItemToCell(InventoryCellType.Bag, id2);

            WriteProgress();
        }

        private bool Swap(IList<CellData> list1, IList<CellData> list2,
            CellInventoryView cell1, CellInventoryView cell2)
        {
            (list1[cell1.Id], list2[cell2.Id]) = (list2[cell2.Id], list1[cell1.Id]);
            _inventoryService.UpdateItemToCell(cell1.InventoryCellType, cell1.Id);
            _inventoryService.UpdateItemToCell(cell2.InventoryCellType, cell2.Id);

            WriteProgress();
            
            return true;
        }

        private bool IsFilledCell(CellInventoryView cell) => 
            cell.ItemData != null;

        private bool IsLeftHandCell(CellInventoryView cell) =>
            cell.ItemType == ItemType.Sword;

        private bool IsRightHandCell(CellInventoryView cell) =>
            cell.ItemType == ItemType.Shield;

        private bool IsLeftHandItem(CellInventoryView cell) =>
            cell.ItemData?.Type is >= ItemType.Sword and <= ItemType.Mace;

        private bool IsRightHandItem(CellInventoryView cell) =>
            cell.ItemData?.Type is ItemType.Shield or ItemType.Dagger;
    }
}