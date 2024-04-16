using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

using UndergroundFortress.Gameplay.Dungeons.Services;

namespace UndergroundFortress
{
    public class SuccessWindow : MonoBehaviour
    {
        [SerializeField] private Button continueButton;
        [SerializeField] private Button nextButton;
        [SerializeField] private Button menuButton;

        [Space]
        [SerializeField] private GameObject cap;

        public void Initialize(IProgressDungeonService progressDungeonService, UnityAction onContinue, UnityAction onNext, UnityAction onMenu)
        {
            Subscribe(progressDungeonService);
            
            continueButton.onClick.AddListener(onContinue);
            continueButton.onClick.AddListener(Deactivate);
            
            nextButton.onClick.AddListener(onNext);
            nextButton.onClick.AddListener(Deactivate);
            
            menuButton.onClick.AddListener(onMenu);
            menuButton.onClick.AddListener(Deactivate);
        }
        
        private void Subscribe(IProgressDungeonService progressDungeonService)
        {
            progressDungeonService.OnSuccessLevel += Activate;
        }

        public void Activate()
        {
            cap.SetActive(true);
            gameObject.SetActive(true);
        }

        public void Deactivate()
        {
            cap.SetActive(false);
            gameObject.SetActive(false);
        }
    }
}
