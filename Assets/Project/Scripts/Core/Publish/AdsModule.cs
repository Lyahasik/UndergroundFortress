namespace UndergroundFortress.Core.Publish
{
    public abstract class AdsModule
    {
        public abstract bool TryShowAdsInterstitial();
        public abstract void ShowAdsReward();
        public abstract void ShowAdsReward(int rewardId);
    
        protected float _nextBlockAdsTime;
    }
}
