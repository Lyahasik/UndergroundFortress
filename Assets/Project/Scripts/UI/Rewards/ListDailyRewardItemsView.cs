using UndergroundFortress.Core.Progress;

namespace UndergroundFortress.UI.Information
{
    public class ListDailyRewardItemsView : ListRewardItemsView
    {
        public void UpdateActual(RewardsData rewardsData)
        {
            for (int i = 0; i < _listItems.Count; i++)
            {
                bool isActive = i < rewardsData.LastAwardId;
                bool isActual = i == rewardsData.LastAwardId;

                var dailyReward = (DailyRewardItemView)_listItems[i];
                dailyReward.UpdateValues(i + 1);
                dailyReward.UpdateState(isActive, isActual);
            }
        }
    }
}