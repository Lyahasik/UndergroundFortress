using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

using UndergroundFortress.Core.Progress;
using UndergroundFortress.Core.Services.Rewards;
using UndergroundFortress.Core.Services.StaticData;
using UndergroundFortress.UI.Inventory;

namespace UndergroundFortress.UI.Information
{
    public class DailyRewardsView : MonoBehaviour
    {
        [SerializeField] private ListDailyRewardItemsView listRewardItems;
        
        [Space]
        [SerializeField] protected Button confirmButton;

        private IStaticDataService _staticDataService;
        
        private List<MoneyNumberData> _rewardMoneys;
        private List<ItemNumberData> _rewardItems;
        
        public void Construct(IStaticDataService staticDataService)
        {
            _staticDataService = staticDataService;
        }
        
        public virtual void Initialize(IDailyRewardsService dailyRewardsService, UnityAction onClose)
        {
            listRewardItems.Construct(_staticDataService);
            listRewardItems.Initialize();

            if (confirmButton != null)
            {
                confirmButton.onClick.AddListener(onClose);
                confirmButton.onClick.AddListener(dailyRewardsService.ClaimReward);
            }
        }
        
        public void Show(List<RewardData> listRewards, RewardsData rewardsData)
        {
            listRewards.ForEach(data => listRewardItems.Filling(data));
            listRewardItems.UpdateActual(rewardsData);

            gameObject.SetActive(true);
        }
        
        public void Hide()
        {
            gameObject.SetActive(false);
            
            Reset();
        }

        private void Reset()
        {
            listRewardItems.Reset();
        }
    }
}