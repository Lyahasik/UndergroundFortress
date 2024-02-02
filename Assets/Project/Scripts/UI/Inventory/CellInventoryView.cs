﻿using TMPro;
using UnityEngine;

using UndergroundFortress.Extensions;
using UndergroundFortress.Core.Services.StaticData;
using UndergroundFortress.Gameplay.Inventory;
using UndergroundFortress.Gameplay.Inventory.Services;
using UndergroundFortress.Gameplay.Items;
using UndergroundFortress.UI.Information;

namespace UndergroundFortress.UI.Inventory
{
    [RequireComponent(typeof(RectTransform))]
    public class CellInventoryView : MonoBehaviour
    {
        [SerializeField] private ItemType itemType;
        [SerializeField] private CellItemView itemView;
        [SerializeField] private TMP_Text numberText;
        
        private int _id;
        private ItemData _itemData;
        private InventoryCellType _inventoryCellType;
        private IStaticDataService _staticDataService;
        private IMovingItemService _movingItemService;

        private RectTransform _rect;
        private IInventoryService _inventoryService;

        public ItemType ItemType => itemType;
        public Sprite Icon => itemView.Icon;
        public Sprite Quality => itemView.Quality;
        public string Number => numberText.text;

        public int Id => _id;
        public ItemData ItemData => _itemData;
        public InventoryCellType InventoryCellType => _inventoryCellType;

        private void Awake()
        {
            _rect = GetComponent<RectTransform>();
        }

        public void Construct(in int cellId,
            IStaticDataService staticDataService,
            IMovingItemService movingItemService,
            IInventoryService inventoryService)
        {
            _id = cellId;

            _staticDataService = staticDataService;
            _movingItemService = movingItemService;
            _inventoryService = inventoryService;
        }

        public void Initialize()
        {
            gameObject.name = nameof(CellInventoryView) + _id;
            
            Reset();
        }

        public void Subscribe() => 
            _inventoryService.OnUpdateCell += UpdateValue;

        public void Subscribe(ActiveArea activeArea)
        {
            activeArea.OnUp += Hit;
            activeArea.OnStartMove += HitInMovement;
            activeArea.OnEndMove += HitInMovement;
        }

        public void SetValues(CellData cellData,
            InventoryCellType inventoryCellType)
        {
            _itemData = cellData.ItemData;
            _inventoryCellType = inventoryCellType;
            itemView.SetValues(
                _staticDataService.GetItemIcon(_itemData.Id),
                _staticDataService.GetQualityBackground(_itemData.QualityType));
            
            if (!_itemData.Type.IsEquipment())
                numberText.text = cellData.Number.ToString();
        }

        private void SetValues(ItemData itemData,
            string number)
        {
            _itemData = itemData;
            itemView.SetValues(
                _staticDataService.GetItemIcon(_itemData.Id),
                _staticDataService.GetQualityBackground(_itemData.QualityType));
            
            numberText.text = _itemData.Type.IsEquipment() ? string.Empty : number;
        }

        public void Show()
        {
            if (_itemData == null)
                return;
            
            itemView.Show();
            numberText.gameObject.SetActive(true);
        }
        
        public void Hide()
        {
            itemView.Hide();
            numberText.gameObject.SetActive(false);
        }

        public void Reset(in InventoryCellType inventoryCellType = InventoryCellType.Bag)
        {
            _itemData = null;
            
            if (_inventoryCellType == InventoryCellType.Empty)
                _inventoryCellType = inventoryCellType;
            
            itemView.Reset();
            numberText.text = string.Empty;
        }

        private void Hit(Vector3 position)
        {
            if (_itemData == null
                || !_rect.IsDotInside(position))
                return;

            _inventoryService.ShowItem(_itemData, _inventoryCellType);
        }
        
        private void HitInMovement(Vector3 position)
        {
            if (!_rect.IsDotInside(position))
                return;
            
            _movingItemService.AddItem(this, position);
        }

        private void UpdateValue(InventoryCellType inventoryCellType, int id, CellData cellData)
        {
            if (_inventoryCellType != inventoryCellType
                || _id != id)
                return;
            
            if (cellData.ItemData != null)
                SetValues(cellData.ItemData, cellData.Number.ToString());
            else
                Reset();
        }

        public static bool operator ==(CellInventoryView value1, CellInventoryView value2)
        {
            if (value1 is null)
                return value2 is null;
                
            return  value2 is not null
                    && value1._inventoryCellType == value2._inventoryCellType
                    && value1._id == value2._id;
        }

        public static bool operator !=(CellInventoryView value1, CellInventoryView value2) => 
            !(value1 == value2);
    }
}