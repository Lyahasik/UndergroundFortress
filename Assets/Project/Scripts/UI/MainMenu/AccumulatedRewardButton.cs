using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace UndergroundFortress.UI.MainMenu
{
    public class AccumulatedRewardButton : MonoBehaviour
    {
        [SerializeField] private Button button;
        
        private bool _isActive;

        public bool IsActive => _isActive;
        
        public void Initialize(UnityAction onClick)
        {
            button.onClick.AddListener(onClick);
        }
        
        public void Activate()
        {
            _isActive = true;
            gameObject.SetActive(true);
        }

        public void Deactivate()
        {
            _isActive = false;
            gameObject.SetActive(false);
        }
    }
}