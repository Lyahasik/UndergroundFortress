using UnityEngine;

using UndergroundFortress.Core.Services.StaticData;
using UndergroundFortress.Extensions;
using UndergroundFortress.Gameplay.Inventory.Services;
using UndergroundFortress.Gameplay.Shop;

namespace UndergroundFortress.UI.Inventory
{
    public class CellSaleView : CellInventoryView
    {
        [Space]
        [SerializeField] private Vector2 rectSize;
        
        private IShoppingService _shoppingService;
        
        private ActiveArea _activeArea;

        public Vector2 RectSize => rectSize;

        public void Construct(in int cellId,
            IStaticDataService staticDataService,
            IInventoryService inventoryService,
            IShoppingService shoppingService)
        {
            _id = cellId;

            _staticDataService = staticDataService;
            _inventoryService = inventoryService;
            _shoppingService = shoppingService;
        }
        
        public new void Initialize()
        {
            gameObject.name = nameof(CellSaleView) + _id;
        }

        private void OnDestroy()
        {
            Unsubscribe();
        }

        public new void Subscribe(ActiveArea activeArea)
        {
            _activeArea = activeArea;
            
            _activeArea.OnUp += Hit;
        }

        private void Unsubscribe()
        {
            _inventoryService.OnUpdateCell -= UpdateValue;
            _activeArea.OnUp -= Hit;
        }
        
        private void Hit(Vector3 position)
        {
            if (_itemData == null
                || !_rect.IsDotInside(position))
                return;

            _shoppingService.ShowSaleItem(this);
        }
    }
}