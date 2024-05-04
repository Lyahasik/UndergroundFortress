using System.Runtime.InteropServices;

namespace UndergroundFortress.Core.Publish.Web.Yandex
{
    public class YandexAnalyticsModule : AnalyticsModule
    {
        [DllImport("__Internal")]
        private static extern void TargetAdsExtern(int id);
        [DllImport("__Internal")]
        private static extern void TargetPurchasesExtern(int id);
        [DllImport("__Internal")]
        private static extern void TargetLevelsExtern(int number);
        [DllImport("__Internal")]
        private static extern void TargetTutorialExtern(int number);
        [DllImport("__Internal")]
        private static extern void TargetActivityExtern(int number);
        
        public override void TargetAds(int id) => 
            TargetAdsExtern(id);

        public override void TargetPurchases(int number) => 
            TargetPurchasesExtern(number);

        public override void TargetLevels(int number) => 
            TargetLevelsExtern(number);

        public override void TargetTutorial(int number) => 
            TargetTutorialExtern(number);

        public override void TargetActivity(int number) => 
            TargetActivityExtern(number);
    }
}