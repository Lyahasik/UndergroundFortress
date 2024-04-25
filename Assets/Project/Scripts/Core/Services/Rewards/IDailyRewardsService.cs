namespace UndergroundFortress.Core.Services.Rewards
{
    public interface IDailyRewardsService : IService
    {
        public void Initialize();
        public void ClaimReward();
    }
}