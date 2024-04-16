using System;
using UnityEngine;
using UnityEngine.UI;

using UndergroundFortress.Core.Services.Ads;

namespace UndergroundFortress.UI.Core.Buttons
{
    public class ButtonAds : MonoBehaviour
    {
        [Space]
        [SerializeField] private int rewardId;
        [SerializeField] private bool isDisposable;
        [SerializeField] private Button button;

        private IProcessingAdsService _processingAdsService;

        private int _currentPrice;

        private Action _onReward;

        public void Construct(IProcessingAdsService processingAdsService)
        {
            _processingAdsService = processingAdsService;
        }

        public void Initialize(Action onReward)
        {
            _onReward = onReward;

            button.onClick.AddListener(ShowAds);
            
            Subscribe();
        }

        private void OnDestroy()
        {
            Unsubscribe();
        }

        private void Subscribe()
        {
            _processingAdsService.OnClaimReward += ClaimReward;
        }

        private void Unsubscribe()
        {
            _processingAdsService.OnClaimReward -= ClaimReward;
        }

        private void ShowAds()
        {
            _processingAdsService.ShowAds(rewardId);
        }

        private void ClaimReward(int rewardId)
        {
            if (this.rewardId != rewardId)
                return;
            
            if (isDisposable)
                gameObject.SetActive(false);
            
            _onReward?.Invoke();
        }
    }
}