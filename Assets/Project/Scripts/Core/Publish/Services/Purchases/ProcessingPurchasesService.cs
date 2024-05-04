using System;
using UnityEngine;

using UndergroundFortress.Core.Publish;
using UndergroundFortress.Core.Publish.Web.Yandex;
using UndergroundFortress.Core.Services.Analytics;
using UndergroundFortress.Helpers;

namespace UndergroundFortress.Core.Services.Publish.Purchases
{
    public class ProcessingPurchasesService : IProcessingPurchasesService
    {
        private readonly IProcessingAnalyticsService _processingAnalyticsService;
        
        private PurchasesModule _purchasesModule;

        private int _currentRewardId;
        
        public event Action<int> OnClaimReward;
        
        public ProcessingPurchasesService(IProcessingAnalyticsService processingAnalyticsService)
        {
            _processingAnalyticsService = processingAnalyticsService;
        }
        
        public void Initialize()
        {
            if (!OSManager.IsEditor())
                _purchasesModule = new YandexPurchasesModule();
            
            Debug.Log($"[{GetType()}] initialize");
        }

        public void CheckPurchases()
        {
            _purchasesModule?.CheckPurchases();
        }
        
        public void StartBuyPurchase(int rewardId)
        {
            if (OSManager.IsEditor())
            {
                ClaimReward();
                return;
            }
            
            _currentRewardId = rewardId;
            _purchasesModule?.StartBayPurchase(_currentRewardId);
        }
        
        public void ClaimReward()
        {
            _processingAnalyticsService.TargetPurchases(_currentRewardId);
            OnClaimReward?.Invoke(_currentRewardId);
        }
        
        public void ClaimReward(int rewardId)
        {
            _processingAnalyticsService.TargetPurchases(rewardId);
            OnClaimReward?.Invoke(rewardId);
        }
    }
}