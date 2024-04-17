using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

using UndergroundFortress.Core.Services.Ads;
using UndergroundFortress.Gameplay.Character;
using UndergroundFortress.Gameplay.Dungeons.Services;
using UndergroundFortress.Gameplay.Stats.Services;
using UndergroundFortress.UI.Core.Buttons;

namespace UndergroundFortress
{
    public class RestWindow : MonoBehaviour
    {
        [SerializeField] private Button continueButton;
        [SerializeField] private Button nextButton;
        [SerializeField] private Button menuButton;
        
        [Space]
        [SerializeField] private ButtonAds recoveryButton;

        [Space]
        [SerializeField] private GameObject cap;

        private IStatsRestorationService _statsRestorationService;
        private PlayerData _playerData;

        private void Start()
        {
            gameObject.SetActive(false);
        }

        public void Construct(IStatsRestorationService statsRestorationService,
            PlayerData playerData)
        {
            _statsRestorationService = statsRestorationService;
            _playerData = playerData;
        }

        public void Initialize(IProgressDungeonService progressDungeonService,
            IProcessingAdsService processingAdsService,
            UnityAction onContinue,
            UnityAction onNext,
            UnityAction onMenu)
        {
            Subscribe(progressDungeonService);
            
            continueButton.onClick.AddListener(onContinue);
            continueButton.onClick.AddListener(Deactivate);
            
            nextButton.onClick.AddListener(onNext);
            nextButton.onClick.AddListener(Deactivate);
            
            menuButton.onClick.AddListener(onMenu);
            menuButton.onClick.AddListener(Deactivate);
            
            recoveryButton.Construct(processingAdsService);
            recoveryButton.Initialize(RestoreFullHealth);
        }

        private void Subscribe(IProgressDungeonService progressDungeonService)
        {
            progressDungeonService.OnEndLevel += Activate;
        }

        public void Activate(bool isSuccess, bool isLastLevel = false)
        {
            recoveryButton.gameObject.SetActive(!isSuccess);
            nextButton.gameObject.SetActive(isSuccess && !isLastLevel);
            
            cap.SetActive(true);
            gameObject.SetActive(true);
        }

        public void Deactivate()
        {
            cap.SetActive(false);
            gameObject.SetActive(false);
        }

        private void RestoreFullHealth()
        {
            _statsRestorationService.RestoreFullHealth(_playerData.Stats);
        }
    }
}
