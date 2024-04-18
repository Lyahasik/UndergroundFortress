using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

using UndergroundFortress.Core.Services.StaticData;
using UndergroundFortress.Gameplay.Character;
using UndergroundFortress.Gameplay.Inventory;
using UndergroundFortress.Gameplay.Inventory.Services;
using UndergroundFortress.Gameplay.Items;
using UndergroundFortress.Gameplay.StaticData;
using UndergroundFortress.Gameplay.Stats;
using UndergroundFortress.Gameplay.Stats.Services;
using UndergroundFortress.UI.Information;
using UndergroundFortress.UI.Inventory;

namespace UndergroundFortress
{
    public class ConsumableItemView : MonoBehaviour, IPointerDownHandler
    {
        [SerializeField] private CellItemView itemView;
        [SerializeField] private GameObject numberView;
        [SerializeField] private TMP_Text numberText;
        
        private IStaticDataService _staticDataService;
        private IInventoryService _inventoryService;
        private IStatsRestorationService _statsRestorationService;
        private IAttackService _attackService;
        private PlayerData _playerData;

        private ItemData _itemData;

        public void Construct(IStaticDataService staticDataService,
            IInventoryService inventoryService,
            IStatsRestorationService statsRestorationService,
            IAttackService attackService,
            PlayerData playerData)
        {
            _staticDataService = staticDataService;
            _inventoryService = inventoryService;
            _statsRestorationService = statsRestorationService;
            _attackService = attackService;
            _playerData = playerData;
        }

        public void Initialize()
        {
            Subscribe();
            UpdateValue(
                InventoryCellType.Equipment, 
                0, 
                _inventoryService.Equipment
                    .Find(data => data.ItemData != null && data.ItemData.Type == ItemType.Consumable));
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            ActivateItem();
        }

        private void OnDestroy()
        {
            Unsubscribe();
        }

        private void Subscribe() => 
            _inventoryService.OnUpdateCell += UpdateValue;

        private void Unsubscribe() => 
            _inventoryService.OnUpdateCell -= UpdateValue;

        private void UpdateValue(InventoryCellType inventoryCellType, int id, CellData cellData)
        {
            if (inventoryCellType != InventoryCellType.Equipment)
                return;
            
            if (cellData != null 
                && cellData.ItemData is { Type: ItemType.Consumable })
                SetValues(cellData.ItemData, cellData.Number);
            else
                Reset();
        }

        private void SetValues(ItemData itemData, int number)
        {
            _itemData = itemData;
            
            itemView.SetValues(
                _staticDataService.GetItemIcon(_itemData.Id),
                _staticDataService.GetQualityBackground(_itemData.QualityType));
            
            numberView.SetActive(true);
            numberText.text = number.ToString();
        }

        private void ActivateItem()
        {
            if (_itemData == null)
                return;
            
            switch (_itemData.ConsumableType)
            {
                case ConsumableType.Type1:
                    _statsRestorationService.RestoreHealth(_playerData.Stats, (int) (_playerData.Stats.MainStats[StatType.Health] * 0.3f));
                    break;
                case ConsumableType.Type2:
                    if (_attackService.IsDoubleDamage)
                        return;
                    _attackService.ActivateConsumable(_itemData.ConsumableType);
                    break;
                case ConsumableType.Type3:
                    if (_attackService.IsVampireDamage)
                        return;
                    _attackService.ActivateConsumable(_itemData.ConsumableType);
                    break;
            }
            
            _inventoryService.RemoveItemsById(InventoryCellType.Equipment, _itemData.Id, 1);
        }

        private void Reset()
        {
            _itemData = null;
            
            itemView.Reset();
            numberView.SetActive(false);
        }
    }
}
