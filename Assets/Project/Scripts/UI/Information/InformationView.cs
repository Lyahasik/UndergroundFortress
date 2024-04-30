using System;
using System.Collections.Generic;
using UnityEngine;

using UndergroundFortress.Core.Services.StaticData;
using UndergroundFortress.Core.Progress;
using UndergroundFortress.Core.Services.Ads;
using UndergroundFortress.Core.Services.Progress;
using UndergroundFortress.Core.Services.Rewards;
using UndergroundFortress.Extensions;
using UndergroundFortress.Gameplay.Inventory.Services;
using UndergroundFortress.Gameplay.Items;
using UndergroundFortress.Gameplay.Items.Equipment;
using UndergroundFortress.Gameplay.Items.Resource;
using UndergroundFortress.Gameplay.Items.Services;
using UndergroundFortress.Gameplay.Shop;
using UndergroundFortress.Gameplay.Skills.Services;
using UndergroundFortress.Gameplay.StaticData;
using UndergroundFortress.UI.Bonuses;
using UndergroundFortress.UI.Information.Prompts;
using UndergroundFortress.UI.Inventory;
using UndergroundFortress.UI.Skills;

namespace UndergroundFortress.UI.Information
{
    public class InformationView : MonoBehaviour
    {
        [SerializeField] private GameObject capArea;
        [SerializeField] private GameObject closeButton;
        
        [Space]
        [SerializeField] private SkillView skillView;
        [SerializeField] private ProgressSkillView progressSkillView;
        
        [Space]
        [SerializeField] private EquipmentView equipmentView;
        [SerializeField] private SaleEquipmentView saleEquipmentView;
        [SerializeField] private EquipmentComparisonView equipmentComparisonView;
        
        [Space]
        [SerializeField] private ResourceView resourceView;
        [SerializeField] private SaleResourceView saleResourceView;

        [Space]
        [SerializeField] private PurchaseRewardItemsView purchaseRewardItemsView;
        [SerializeField] private RewardItemsView rewardItemsView;

        [Space]
        [SerializeField] private BonusOfferView bonusOfferView;
        [SerializeField] private AccumulatedRewardView accumulatedRewardView;
        [SerializeField] private DailyRewardsView dailyRewardsView;

        [Space]
        [SerializeField] private CellItemView cellItemView;

        [Space]
        [SerializeField] private WarningPrompt warningPrompt;
        
        private EquipmentView _currentEquipmentView;

        public CellItemView CellItemView => cellItemView;

        public void Initialize(IStaticDataService staticDataService,
            IProcessingAdsService processingAdsService,
            IProgressProviderService progressProviderService,
            ISkillsUpgradeService skillsUpgradeService,
            IItemsGeneratorService itemsGeneratorService,
            IInventoryService inventoryService,
            IShoppingService shoppingService,
            IAccumulationRewardsService accumulationRewardsService,
            IDailyRewardsService dailyRewardsService)
        {
            skillView.Construct(staticDataService, skillsUpgradeService);
            skillView.Initialize(CloseView);
            
            progressSkillView.Construct(staticDataService, skillsUpgradeService);
            progressSkillView.Initialize(CloseView);
            
            equipmentView.Construct(staticDataService);
            equipmentView.Initialize();
            saleEquipmentView.Construct(staticDataService, shoppingService);
            saleEquipmentView.Initialize(progressProviderService, CloseView);
            
            equipmentComparisonView.Initialize(staticDataService);
            
            resourceView.Construct(staticDataService);
            saleResourceView.Construct(staticDataService, shoppingService);
            saleResourceView.Initialize(progressProviderService, CloseView);
            
            rewardItemsView.Construct(staticDataService, progressProviderService, itemsGeneratorService, inventoryService);
            rewardItemsView.Initialize(CloseView);

            purchaseRewardItemsView.Construct(
                staticDataService,
                progressProviderService,
                itemsGeneratorService,
                inventoryService,
                shoppingService,
                processingAdsService);
            purchaseRewardItemsView.Initialize(CloseView);
            
            bonusOfferView.Initialize(processingAdsService, CloseView);
            
            accumulatedRewardView.Construct(accumulationRewardsService);
            accumulatedRewardView.Initialize(CloseView);
            dailyRewardsView.Construct(staticDataService);
            dailyRewardsView.Initialize(dailyRewardsService, CloseView);
        }

        public void ShowSkill(SkillsType skillsType, SkillData skillData, bool isCanUpgrade, ProgressSkillData progressSkillData = null, bool isCapping = true)
        {
            if (isCapping)
                CapActivate();
            
            if (progressSkillData == null)
                skillView.Show(skillsType, skillData, isCanUpgrade);
            else
                progressSkillView.Show(skillsType, skillData, progressSkillData, isCanUpgrade);
        }

        public void ShowItem(ItemData itemData)
        {
            CapActivate();
            
            if (itemData.Type.IsEquipment())
                ShowEquipment(itemData);
            else
                ShowResource(itemData);
        }

        public void ShowSaleItem(CellSaleView cellSale, bool isCapping = true)
        {
            if (cellSale.ItemData.Type.IsEquipment())
                ShowSaleEquipment(cellSale);
            else
                ShowSaleResource(cellSale, isCapping);
        }

        public void ShowEquipmentComparison(ItemData equipmentData1, ItemData equipmentData2)
        {
            CapActivate();
            
            equipmentComparisonView.Show(equipmentData1, equipmentData2);
        }

        public void ShowWarning(string message) => 
            warningPrompt.Show(message);

        public void CloseView()
        {
            capArea.SetActive(false);
            closeButton.SetActive(false);
            
            progressSkillView.Hide();
            skillView.Hide();
            equipmentView.Hide();
            saleEquipmentView.Hide();
            equipmentComparisonView.Hide();
            resourceView.Hide();
            saleResourceView.Hide();

            rewardItemsView.Hide();
            purchaseRewardItemsView.Hide();
            
            bonusOfferView.Hide();
            accumulatedRewardView.Hide();
            dailyRewardsView.Hide();
            
            warningPrompt.Hide();
        }

        private void ShowSaleResource(CellSaleView cellSale, bool isCapping)
        {
            if (isCapping)
                CapActivate();
            
            saleResourceView.Show(cellSale);
        }

        private void ShowSaleEquipment(CellSaleView cellSale)
        {
            CapActivate();

            saleEquipmentView.Show(cellSale);
        }

        private void ShowResource(ItemData resourceData) => 
            resourceView.Show(resourceData);

        private void ShowEquipment(ItemData equipmentData) => 
            equipmentView.Show(equipmentData);

        public void ShowReward(RewardData rewardData)
        {
            CapActivate();

            rewardItemsView.Show(rewardData);
        }

        public void ShowPurchase(CellPurchaseView cellPurchase, bool isCapping = true)
        {
            if (isCapping)
                CapActivate();

            purchaseRewardItemsView.Show(cellPurchase.PurchaseStaticData);
        }

        public void UpdateBonusOffer(BonusData bonusData, Action onBonusActivate)
        {
            bonusOfferView.UpdateOffer(bonusData, onBonusActivate);
        }

        public void ShowBonusOffer()
        {
            CapActivate();

            bonusOfferView.Show();
        }

        public void UpdateAccumulatedReward(RewardsData rewardsData)
        {
            accumulatedRewardView.UpdateValues(rewardsData);
        }

        public void ShowAccumulatedReward()
        {
            CapActivate();

            accumulatedRewardView.Show();
        }

        public void ShowDailyRewards(List<RewardData> dailyRewards, RewardsData rewardsData)
        {
            capArea.SetActive(true);
            
            dailyRewardsView.Show(dailyRewards, rewardsData);
        }

        private void CapActivate()
        {
            capArea.SetActive(true);
            closeButton.SetActive(true);
        }
    }
}