using UnityEngine;

using UndergroundFortress.Core.Services;
using UndergroundFortress.Gameplay.Items;
using UndergroundFortress.UI.Information;
using UndergroundFortress.UI.Inventory;

namespace UndergroundFortress.Gameplay.Inventory.Services
{
    public interface IMovingItemService : IService
    {
        public CellInventoryView DraggedCellInventoryView { get; }
        
        public void Initialize(CellItemView cellItemView);
        public void Subscribe(ActiveArea bagActiveArea);
        public void AddItem(CellInventoryView cellInventoryView, Vector3 newPosition);
        public void TryReset();
        public ItemData GetDraggedItem();
    }
}