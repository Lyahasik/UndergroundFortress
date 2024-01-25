using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

using UndergroundFortress.Gameplay.Inventory.Services;

namespace UndergroundFortress.UI.Inventory
{
    [RequireComponent(typeof(Image))]
    public class ActiveArea : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler, ISelectHandler, IDeselectHandler
    {
        private IMovingItemService _movingItemService;

        private bool _isMovement;
        
        public event Action<Vector3> OnUp;
        public event Action<Vector3> OnStartMove;
        public event Action<Vector3> OnEndMove;
        public event Action<Vector3> OnDragItem;

        public void Construct(IMovingItemService movingItemService) => 
            _movingItemService = movingItemService;

        public void OnPointerDown(PointerEventData eventData)
        {
            
            EventSystem.current.SetSelectedGameObject(gameObject, eventData);
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            if (_isMovement)
                OnEndMove?.Invoke(eventData.position);
            else
                OnUp?.Invoke(eventData.position);

            EventSystem.current.SetSelectedGameObject(null, eventData);
            _isMovement = false;
        }

        public void OnDrag(PointerEventData eventData)
        {
            if (_isMovement)
            {
                OnDragItem?.Invoke(eventData.position);
            }
            else
                StartMovement(eventData);
        }

        private void StartMovement(PointerEventData eventData)
        {
            _isMovement = true;
            OnStartMove?.Invoke(eventData.position);
        }

        public void OnSelect(BaseEventData eventData) {}

        public void OnDeselect(BaseEventData eventData) => 
            _movingItemService.TryReset();
    }
}