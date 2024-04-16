using System;

namespace UndergroundFortress.Core.Services.Ads
{
    public interface IProcessingAdsService : IService
    {
        event Action<int> OnClaimReward;
        void ShowAds(int rewardId);
    }
}