using System;

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
            ProgressSkillData progressSkillData = null)
        {
            _informationView.ShowSkill(skillsType, skillData, isCanUpgrade, progressSkillData);
        }

        public void ShowItem(ItemData itemData)
        {
            _informationView.ShowItem(itemData);
        }

        public void ShowSaleItem(CellSaleView cellSale)
        {
            _informationView.ShowSaleItem(cellSale);
        }

        public void ShowReward(RewardData rewardData)
        {
            _informationView.ShowReward(rewardData);
        }

        public void ShowPurchase(CellPurchaseView cellPurchase)
        {
            _informationView.ShowPurchase(cellPurchase);
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
    }
}