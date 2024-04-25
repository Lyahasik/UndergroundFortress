using System.Collections.Generic;
using UnityEngine;

using UndergroundFortress.Core.Services.StaticData;
using UndergroundFortress.UI.Inventory;

namespace UndergroundFortress.UI.Information
{
    public class ListRewardItemsView : MonoBehaviour
    {
        [SerializeField] private RewardItemView prefabRewardItemView;
        
        private IStaticDataService _staticDataService;
        
        protected List<RewardItemView> _listItems;

        public void Construct(IStaticDataService staticDataService)
        {
            _staticDataService = staticDataService;
        }

        public void Initialize()
        {
            _listItems = new List<RewardItemView>();
        }
        
        public void Filling(RewardData rewardData)
        {
            rewardData.moneys.ForEach(data =>
            {
                RewardItemView rewardItemView = Instantiate(prefabRewardItemView, transform);
                rewardItemView.Initialize(_staticDataService.GetIconMoneyByType(data.moneyType), data.number);
                _listItems.Add(rewardItemView);
            });
            
            rewardData.items.ForEach(data =>
            {
                RewardItemView rewardItemView = Instantiate(prefabRewardItemView, transform);

                Sprite qualitySprite = _staticDataService.GetQualityBackground(data.qualityType);
                rewardItemView.Initialize(_staticDataService.GetItemIcon(data.itemData.id), data.number, data.level , qualitySprite);
                _listItems.Add(rewardItemView);
            });
        }

        public void Reset()
        {
            _listItems.ForEach(data => Destroy(data.gameObject));
            _listItems.Clear();
        }
    }
}