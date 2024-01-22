using UnityEngine;

using UndergroundFortress.UI.Inventory;
using UndergroundFortress.UI.Information;

using Vector3 = UnityEngine.Vector3;

namespace UndergroundFortress.Gameplay.Inventory.Services
{
    public class MovingItemService : IMovingItemService
    {
        private readonly IInventoryService _inventoryService;
        
        private CellItemView _cellItemView;
        
        private CellBagView _cellBagView;
        private Transform _transformItemView;

        private bool _isHit;

        public MovingItemService(IInventoryService inventoryService)
        {
            _inventoryService = inventoryService;
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

        public void AddItem(CellBagView cellBagView, Vector3 newPosition)
        {
            if (_cellBagView == null)
            {
                _cellBagView = cellBagView;
                _cellBagView.Hide();
                    
                _cellItemView.transform.position = newPosition;
                _cellItemView.SetValues(_cellBagView.Icon, _cellBagView.Quality);
            }
            else if (cellBagView != _cellBagView)
            {
                _isHit = true;
                SwapCells(cellBagView);
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
            
            if (_cellBagView != null) 
                Reset();
        }

        private void SwapCells(CellBagView cellBagView)
        {
            _inventoryService.SwapItems(_cellBagView.Id, cellBagView.Id);
            Sprite iconTemporary = _cellBagView.Icon;
            Sprite qualityTemporary = _cellBagView.Quality;
            string numberTemporary = _cellBagView.Number;

            _cellBagView.SetValues(cellBagView.Icon, cellBagView.Quality, cellBagView.Number);
            cellBagView.SetValues(iconTemporary, qualityTemporary, numberTemporary);

            _cellBagView.Show();
            _cellBagView = null;
            _cellItemView.Reset();
        }

        private void Reset()
        {
            _cellBagView.Show();
            _cellBagView = null;

            _cellItemView.Reset();
            
            Debug.Log("Reset");
        }
    }
}