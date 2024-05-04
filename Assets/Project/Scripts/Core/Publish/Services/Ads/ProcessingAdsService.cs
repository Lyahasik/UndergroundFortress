using System;
using UnityEngine;

using UndergroundFortress.Core.Publish;
using UndergroundFortress.Core.Publish.Web.Yandex;
using UndergroundFortress.Core.Services.Analytics;
using UndergroundFortress.Helpers;

namespace UndergroundFortress.Core.Services.Ads
{
    public class ProcessingAdsService : IProcessingAdsService
    {
        private readonly IProcessingAnalyticsService _processingAnalyticsService;
        
        private AdsModule _adsModule;

        private int _currentRewardId;

        public event Action<int> OnClaimReward;

        public ProcessingAdsService(IProcessingAnalyticsService processingAnalyticsService)
        {
            _processingAnalyticsService = processingAnalyticsService;
        }

        public void Initialize()
        {
            if (!OSManager.IsEditor())
                _adsModule = new YandexAdsModule();
            
            Debug.Log($"[{GetType()}] initialize");
        }

        public void ShowAdsReward(int rewardId)
        {
            _processingAnalyticsService.TargetAds(rewardId);
            _currentRewardId = rewardId;

            if (OSManager.IsEditor())
            {
                ClaimReward();
                return;
            }

            _adsModule?.ShowAdsReward();
            PrepareAds();
        }

        public void ShowAdsInterstitial()
        {
            if (_adsModule != null
                && _adsModule.TryShowAdsInterstitial())
                PrepareAds();
        }

        public void EndAds()
        {
            Time.timeScale = 1f;
        }

        public void ClaimReward()
        {
            OnClaimReward?.Invoke(_currentRewardId);
        }

        private void PrepareAds()
        {
            Time.timeScale = 0f;
        }
    }
}