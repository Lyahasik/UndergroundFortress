using System.Runtime.InteropServices;
using UnityEngine;

using UndergroundFortress.Constants;
using UndergroundFortress.Helpers;

#if UNITY_WEBGL
namespace UndergroundFortress.Core.Publish.Web.Yandex
{
    public class YandexAdsModule : AdsModule
    {
        [DllImport("__Internal")]
        private static extern void ShowAdsInterstitialExtern();
    
        [DllImport("__Internal")]
        private static extern void ShowAdsRewardExtern();

        public YandexAdsModule()
        {
            _nextBlockAdsTime = Time.time + ConstantValues.ADS_BLOCK_DELAY_TIME;
        }

        public override bool TryShowAdsInterstitial()
        {
            if (OSManager.IsEditor()
                || _nextBlockAdsTime > Time.time)
                return false;
            
            ShowAdsInterstitialExtern();
            _nextBlockAdsTime = Time.time + ConstantValues.ADS_BLOCK_DELAY_TIME;
            
            return true;
        }

        public override void ShowAdsReward()
        {
            ShowAdsRewardExtern();
        }

        public override void ShowAdsReward(int rewardId)
        {
            ShowAdsRewardExtern();
        }
    }
}
#endif
