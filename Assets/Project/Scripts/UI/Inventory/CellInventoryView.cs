using TMPro;
using UnityEngine;
using UnityEngine.UI;

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
        [SerializeField] private Image icon;
        [SerializeField] private Image quality;
        [SerializeField] private TMP_Text numberText;
        
        private int _id;
        private ItemData _itemData;
        private InventoryCellType _inventoryCellType;
        private IStaticDataService _staticDataService;
        private IMovingItemService _movingItemService;

        private RectTransform _rect;
        private InformationView _informationView;

        public ItemType ItemType => itemType;
        public Sprite Icon => icon.sprite;
        public Sprite Quality => quality.sprite;
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
            InformationView informationView)
        {
            _id = cellId;

            _staticDataService = staticDataService;
            _movingItemService = movingItemService;
            _informationView = informationView;
        }

        public void Initialize()
        {
            gameObject.name = nameof(CellInventoryView) + _id;
            
            Reset();
        }

        public void Subscribe(IInventoryService inventoryService) => 
            inventoryService.OnUpdateCell += UpdateValue;

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
            icon.sprite = _staticDataService.GetItemIcon(_itemData.Id);
            quality.sprite = _staticDataService.GetQualityIcon(_itemData.QualityType);
            
            if (!_itemData.Type.IsEquipment())
                numberText.text = cellData.Number.ToString();
        }

        private void SetValues(ItemData itemData,
            string number)
        {
            _itemData = itemData;
            icon.sprite = _staticDataService.GetItemIcon(_itemData.Id);
            quality.sprite = _staticDataService.GetQualityIcon(_itemData.QualityType);
            
            if (!_itemData.Type.IsEquipment())
                numberText.text = number;
        }

        public void Show()
        {
            icon.gameObject.SetActive(true);
            quality.gameObject.SetActive(true);
            numberText.gameObject.SetActive(true);
        }
        
        public void Hide()
        {
            icon.gameObject.SetActive(false);
            quality.gameObject.SetActive(false);
            numberText.gameObject.SetActive(false);
        }

        public void Reset(in InventoryCellType inventoryCellType = InventoryCellType.Bag)
        {
            _itemData = null;
            
            if (_inventoryCellType == InventoryCellType.Empty)
                _inventoryCellType = inventoryCellType;
            
            icon.sprite = null;
            quality.sprite = null;
            numberText.text = string.Empty;
        }

        private void Hit(Vector3 position)
        {
            if (_itemData == null
                || !_rect.IsDotInside(position))
                return;

            _informationView.ShowItem(_itemData);
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