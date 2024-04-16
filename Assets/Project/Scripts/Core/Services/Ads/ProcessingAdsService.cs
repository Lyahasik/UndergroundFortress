using System;

namespace UndergroundFortress.Core.Services.Ads
{
    public class ProcessingAdsService : IProcessingAdsService
    {
        private int _currentRewardId;

        public event Action<int> OnClaimReward;  

        public void ShowAds(int rewardId)
        {
            _currentRewardId = rewardId;
            ClaimReward();
        }

        private void ClaimReward()
        {
            OnClaimReward?.Invoke(_currentRewardId);
        }
    }
}