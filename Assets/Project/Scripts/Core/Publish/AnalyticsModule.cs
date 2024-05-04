namespace UndergroundFortress.Core.Publish
{
    public abstract class AnalyticsModule
    {
        public abstract void TargetAds(int id);
        public abstract void TargetPurchases(int number);
        public abstract void TargetLevels(int number);
        public abstract void TargetTutorial(int number);
        public abstract void TargetActivity(int number);
    }
}
