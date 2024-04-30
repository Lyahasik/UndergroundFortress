using System;
using System.Collections.Generic;
using UndergroundFortress.Core.Progress;
using UndergroundFortress.Gameplay.Items;
using UndergroundFortress.Gameplay.StaticData;
using UndergroundFortress.UI.Inventory;


namespace UndergroundFortress.UI.Information.Services
{
    public class InformationService : IInformationService
    {
        private InformationView _informationView;

        public void Initialize(InformationView informationView)
        {
            _informationView = informationView;
        }

        public void ShowSkill(SkillsType skillsType,
            SkillData skillData,
            bool isCanUpgrade = false, 
            ProgressSkillData progressSkillData = null,
            bool isCapping = true)
        {
            _informationView.ShowSkill(skillsType, skillData, isCanUpgrade, progressSkillData, isCapping);
        }

        public void ShowItem(ItemData itemData)
        {
            _informationView.ShowItem(itemData);
        }

        public void ShowSaleItem(CellSaleView cellSale, bool isCapping = true)
        {
            _informationView.ShowSaleItem(cellSale, isCapping);
        }

        public void ShowReward(RewardData rewardData)
        {
            _informationView.ShowReward(rewardData);
        }

        public void ShowPurchase(CellPurchaseView cellPurchase, bool isCapping = true)
        {
            _informationView.ShowPurchase(cellPurchase, isCapping);
        }

        public void ShowEquipmentComparison(ItemData equipmentData1, ItemData equipmentData2)
        {
            _informationView.ShowEquipmentComparison(equipmentData1, equipmentData2);
        }

        public void ShowWarning(string message)
        {
            _informationView.ShowWarning(message);
        }

        public void UpdateBonusOffer(BonusData bonusData, Action onBonusActivate)
        {
            _informationView.UpdateBonusOffer(bonusData, onBonusActivate);
        }

        public void ShowBonusOffer()
        {
            _informationView.ShowBonusOffer();
        }

        public void UpdateAccumulatedReward(RewardsData rewardsData)
        {
            _informationView.UpdateAccumulatedReward(rewardsData);
        }

        public void ShowAccumulatedReward()
        {
            _informationView.ShowAccumulatedReward();
        }

        public void ShowDailyRewards(List<RewardData> dailyRewards, RewardsData rewardsData)
        {
            _informationView.ShowDailyRewards(dailyRewards, rewardsData);
        }
    }
}