using System;
using UnityEngine;
using UnityEngine.UI;

namespace UndergroundFortress.UI.MainMenu
{
    public class LevelDungeonButton : MonoBehaviour
    {
        [SerializeField] private int id;
        
        [Space]
        [SerializeField] private Button button;
        [SerializeField] private GameObject number;
        [SerializeField] private GameObject lockCap;

        private Action<int> _onStartLevel;
        
        public void Initialize(Action<int> onStartLevel)
        {
            _onStartLevel = onStartLevel;
            button.onClick.AddListener(StartLevel);
                
            button.interactable = false;
            number.SetActive(false);
            lockCap.SetActive(true);
        }

        public void Unlock()
        {
            button.interactable = true;
            number.SetActive(true);
            lockCap.SetActive(false);
        }

        private void StartLevel()
        {
            _onStartLevel?.Invoke(id);
        }
    }
}