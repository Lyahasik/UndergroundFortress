using System;

namespace UndergroundFortress.Core.Services.Publish.Purchases
{
    public interface IProcessingPurchasesService : IService
    {
        public event Action<int> OnClaimReward;
        public void CheckPurchases();
        public void StartBuyPurchase(int rewardId);
        public void ClaimReward();
        public void ClaimReward(int rewardId);
    }
}