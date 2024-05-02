using System;

namespace UndergroundFortress.Core.Services.Ads
{
    public interface IProcessingAdsService : IService
    {
        public event Action<int> OnClaimReward;
        public void ShowAdsReward(int rewardId);
        public void ShowAdsInterstitial();
        public void ClaimReward();
        public void EndAds();
    }
}