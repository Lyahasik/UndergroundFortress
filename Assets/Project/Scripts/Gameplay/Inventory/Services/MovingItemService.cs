using UnityEngine;

using UndergroundFortress.Gameplay.Items;
using UndergroundFortress.UI.Inventory;
using UndergroundFortress.UI.Information;

using Vector3 = UnityEngine.Vector3;

namespace UndergroundFortress.Gameplay.Inventory.Services
{
    public class MovingItemService : IMovingItemService
    {
        private readonly ISwapCellsService _swapCellsService;
        
        private CellItemView _cellItemView;
        
        private CellInventoryView _draggedCellInventoryView;
        private Transform _transformItemView;

        private bool _isHit;

        public MovingItemService(ISwapCellsService swapCellsService)
        {
            _swapCellsService = swapCellsService;
        }

        public void Initialize(CellItemView cellItemView)
        {
            _cellItemView = cellItemView;
        }

        public void Subscribe(ActiveArea bagActiveArea)
        {
            bagActiveArea.OnDragItem += ItemMoving;
        }

        private void ItemMoving(Vector3 newPosition)
        {
            _cellItemView.transform.position = newPosition;
        }

        public void AddItem(CellInventoryView cellInventoryView, Vector3 newPosition)
        {
            if (_draggedCellInventoryView == null)
            {
                if (cellInventoryView.ItemData == null)
                    return;
                
                _draggedCellInventoryView = cellInventoryView;
                _draggedCellInventoryView.Hide();
                    
                _cellItemView.transform.position = newPosition;
                _cellItemView.SetValues(_draggedCellInventoryView.Icon, _draggedCellInventoryView.Quality);
            }
            else if (cellInventoryView != _draggedCellInventoryView)
            {
                _isHit = true;
                SwapCells(cellInventoryView);
            }
            else
            {
                _isHit = true;
                Reset();
            }
        }

        public void TryReset()
        {
            if (_isHit)
            {
                _isHit = false;
                return;
            }
            
            if (_draggedCellInventoryView != null) 
                Reset();
        }

        public ItemData GetDraggedItem()
        {
            if (_draggedCellInventoryView == null)
                return null;
            
            ItemData itemData = _draggedCellInventoryView.ItemData;

            Reset();

            return itemData;
        }

        private void SwapCells(CellInventoryView cellInventoryView)
        {
            _swapCellsService.TrySwapCells(_draggedCellInventoryView, cellInventoryView);

            Reset();
        }

        private void Reset()
        {
            _draggedCellInventoryView.Show();
            _draggedCellInventoryView = null;

            _cellItemView.Hide();
        }
    }
}