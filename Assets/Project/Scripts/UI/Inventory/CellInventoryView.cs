using TMPro;
using UnityEngine;
using UnityEngine.UI;

using UndergroundFortress.Extensions;
using UndergroundFortress.Gameplay.Inventory.Services;
using UndergroundFortress.Gameplay.Items;

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
        private IMovingItemService _movingItemService;

        private RectTransform _rect;

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

        public void Construct(in int cellId, IMovingItemService movingItemService)
        {
            _id = cellId;
            _movingItemService = movingItemService;
        }

        public void Initialize()
        {
            gameObject.name = nameof(CellInventoryView) + _id;
            
            Reset();
        }

        public void Subscribe(ActiveArea activeArea)
        {
            activeArea.OnDown += Hit;
            activeArea.OnUp += Hit;
        }

        public void SetValues(ItemData itemData,
            InventoryCellType inventoryCellType,
            Sprite icon,
            in int number)
        {
            _itemData = itemData;
            _inventoryCellType = inventoryCellType;
            this.icon.sprite = icon;
            quality.sprite = null;
            numberText.text = number.ToString();
        }

        public void SetValues(ItemData itemData,
            Sprite icon,
            Sprite quality,
            string number)
        {
            _itemData = itemData;
            this.icon.sprite = icon;
            this.quality.sprite = quality;
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

        public void Hit(Vector3 position)
        {
            if (!_rect.IsDotInside(position))
                return;
            
            _movingItemService.AddItem(this, position);
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