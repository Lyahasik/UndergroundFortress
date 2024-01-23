using UnityEngine;

using UndergroundFortress.UI.Inventory;
using UndergroundFortress.UI.Information;

using Vector3 = UnityEngine.Vector3;

namespace UndergroundFortress.Gameplay.Inventory.Services
{
    public class MovingItemService : IMovingItemService
    {
        private readonly ISwapCellsService _swapCellsService;
        
        private CellItemView _cellItemView;
        
        private CellInventoryView _cellInventoryView;
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
            if (_cellInventoryView == null)
            {
                if (cellInventoryView.ItemData == null)
                    return;
                
                _cellInventoryView = cellInventoryView;
                _cellInventoryView.Hide();
                    
                _cellItemView.transform.position = newPosition;
                _cellItemView.SetValues(_cellInventoryView.Icon, _cellInventoryView.Quality);
            }
            else if (cellInventoryView != _cellInventoryView)
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
            
            if (_cellInventoryView != null) 
                Reset();
        }

        private void SwapCells(CellInventoryView cellInventoryView)
        {
            _swapCellsService.TrySwapCells(_cellInventoryView, cellInventoryView);

            Reset();
        }

        private void Reset()
        {
            _cellInventoryView.Show();
            _cellInventoryView = null;

            _cellItemView.Reset();
        }
    }
}