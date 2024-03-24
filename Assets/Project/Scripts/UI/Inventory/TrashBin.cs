using UnityEngine;

using UndergroundFortress.Extensions;
using UndergroundFortress.Gameplay.Inventory.Services;

namespace UndergroundFortress.UI.Inventory
{
    [RequireComponent(typeof(RectTransform))]
    public class TrashBin : MonoBehaviour
    {
        private IInventoryService _inventoryService;
        private IMovingItemService _movingItemService;
        
        private RectTransform _rect;
        
        private void Awake()
        {
            _rect = GetComponent<RectTransform>();
        }

        public void Construct(IInventoryService inventoryService, IMovingItemService movingItemService)
        {
            _inventoryService = inventoryService;
            _movingItemService = movingItemService;
        }
        
        public void Subscribe(ActiveArea bagActiveArea) => 
            bagActiveArea.OnEndMove += Hit;
        
        private void Hit(Vector3 position)
        {
            if (!_rect.IsDotInside(position))
                return;

            _inventoryService.ClearCell(_movingItemService.DraggedCellInventoryView);
        }
    }
}
