using UndergroundFortress.Core.Publish;

namespace UndergroundFortress.Core.Services.Analytics
{
    public interface IProcessingAnalyticsService : IService
    {
        void TargetAds(int id);
        void TargetPurchases(int id);
        void TargetLevels(int number);
        void TargetTutorial(int number);
        void TargetActivity(ActivityType type, int totalNumber);
    }
}