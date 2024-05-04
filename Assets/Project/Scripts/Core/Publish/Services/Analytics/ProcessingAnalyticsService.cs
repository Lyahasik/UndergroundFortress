using UnityEngine;

using UndergroundFortress.Core.Publish;
using UndergroundFortress.Core.Publish.Web.Yandex;
using UndergroundFortress.Helpers;

namespace UndergroundFortress.Core.Services.Analytics
{
    public class ProcessingAnalyticsService : IProcessingAnalyticsService
    {
        private AnalyticsModule _analyticsModule;

        public void Initialize()
        {
            if (!OSManager.IsEditor())
                _analyticsModule = new YandexAnalyticsModule();
            
            Debug.Log($"[{GetType()}] initialize");
        }

        public void TargetAds(int id) => 
            _analyticsModule?.TargetAds(id);

        public void TargetPurchases(int id) => 
            _analyticsModule?.TargetPurchases(id + 1);

        public void TargetLevels(int number) => 
            _analyticsModule?.TargetLevels(number);

        public void TargetTutorial(int number) => 
            _analyticsModule?.TargetTutorial(number);

        public void TargetActivity(ActivityType type, int totalNumber)
        {
            int targetNumber = 0;

            switch (type)
            {
                case ActivityType.Craft:
                    targetNumber = DefineCraft(totalNumber);
                    break;
                case ActivityType.Purchases:
                    targetNumber = DefinePurchases(totalNumber);
                    break;
                case ActivityType.Killing:
                    targetNumber = DefineKilling(totalNumber);
                    break;
            }
            
            if (targetNumber == 0)
                return;
            
            _analyticsModule?.TargetActivity((int)type * 5 + targetNumber);
        }

        private int DefineCraft(int totalNumber)
        {
            return totalNumber switch
            {
                5 => 1,
                25 => 2,
                100 => 3,
                500 => 4,
                1000 => 5,
                _ => 0
            };
        }

        private int DefinePurchases(int totalNumber)
        {
            return totalNumber switch
            {
                5 => 1,
                25 => 2,
                100 => 3,
                500 => 4,
                1000 => 5,
                _ => 0
            };
        }

        private int DefineKilling(int totalNumber)
        {
            return totalNumber switch
            {
                50 => 1,
                100 => 2,
                150 => 3,
                200 => 4,
                250 => 5,
                _ => 0
            };
        }
    }
}