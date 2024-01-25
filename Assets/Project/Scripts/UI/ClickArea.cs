using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UndergroundFortress.UI
{
    [RequireComponent(typeof(Image))]
    public class ClickArea : MonoBehaviour, IPointerDownHandler
    {
        public UnityEvent OnDown;
        
        public void OnPointerDown(PointerEventData eventData)
        {
            OnDown?.Invoke();
        }
    }
}