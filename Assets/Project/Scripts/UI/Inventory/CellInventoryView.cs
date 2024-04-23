using TMPro;
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
        
        [Space]
        [SerializeField] private GameObject numberLevelView;
        [SerializeField] private TMP_Text numberLevelText;
        
        [Space]
        [SerializeField] private GameObject numberView;
        [SerializeField] private TMP_Text numberText;
        
        protected int _id;
        protected ItemData _itemData;
        private InventoryCellType _inventoryCellType;
        protected IStaticDataService _staticDataService;
        private IMovingItemService _movingItemService;

        protected RectTransform _rect;
        protected IInventoryService _inventoryService;

        private ActiveArea _parentArea;
        protected int _number;

        public ItemType ItemType
        {
            get
            {
                if (itemType != ItemType.Empty)
                    return itemType;

                return _itemData?.Type ?? ItemType.Empty;
            }
        }

        public Sprite Icon => itemView.Icon;
        public Sprite Quality => itemView.Quality;
        public int Number => _number;
        public string NumberString => numberText.text;

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

        public void Subscribe(ActiveArea activeArea, bool isParentArea)
        {
            if (isParentArea)
                _parentArea = activeArea;
            
            activeArea.OnUp += Hit;
            activeArea.OnStartMove += HitInMovement;
            activeArea.OnEndMove += HitInMovement;
        }

        public void SetValues(CellData cellData,
            InventoryCellType inventoryCellType)
        {
            _inventoryCellType = inventoryCellType;
            
            SetValues(cellData.ItemData, cellData.Number);
        }

        public void Show()
        {
            if (_itemData == null)
                return;
            
            itemView.Show();

            numberView.SetActive(numberText.text != string.Empty);
            numberLevelView.SetActive(numberText.text == string.Empty);
        }

        public void Hide()
        {
            itemView.Hide();
            numberView.SetActive(false);
            numberLevelView.SetActive(false);
        }

        public void Reset(in InventoryCellType inventoryCellType = InventoryCellType.Bag)
        {
            _itemData = null;
            
            if (_inventoryCellType == InventoryCellType.Empty)
                _inventoryCellType = inventoryCellType;
            
            itemView.Reset();
            numberView.SetActive(false);
            numberLevelView.SetActive(false);
        }

        private void SetValues(ItemData itemData, int number)
        {
            _itemData = itemData;
            _number = number;
            
            itemView.SetValues(
                _staticDataService.GetItemIcon(_itemData.Id),
                _staticDataService.GetQualityBackground(_itemData.QualityType),
                itemData.Level);
            
            TryShowLevel();
            TryShowNumber(number.ToString());
        }

        private void Hit(Vector3 position)
        {
            if (_itemData == null
                || !_rect.IsDotInside(position))
                return;

            _inventoryService.ShowItem(_itemData, _inventoryCellType);
        }

        private void HitInMovement(Vector3 position, ActiveArea activeArea, bool isDotInsideArea)
        {
            if (!_rect.IsDotInside(position)
                || !isDotInsideArea && _parentArea == activeArea
                || isDotInsideArea && _parentArea != activeArea)
                return;
            
            _movingItemService.AddItem(this, position);
        }

        protected void UpdateValue(InventoryCellType inventoryCellType, int id, CellData cellData)
        {
            if (_inventoryCellType != inventoryCellType
                || _id != id)
                return;
            
            if (cellData.ItemData != null)
                SetValues(cellData.ItemData, cellData.Number);
            else
                Reset();
        }

        private void TryShowLevel()
        {
            if (_itemData.Type.IsEquipment())
            {
                numberLevelView.SetActive(true);
                numberLevelText.text = _itemData.Level.ToString();
            }
            else
            {
                numberLevelView.SetActive(false);
            }
        }

        private void TryShowNumber(string number)
        {
            if (_itemData.Type.IsEquipment())
            {
                numberView.SetActive(false);
                numberText.text = string.Empty;
            }
            else
            {
                numberView.SetActive(true);
                numberText.text = number;
            }
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