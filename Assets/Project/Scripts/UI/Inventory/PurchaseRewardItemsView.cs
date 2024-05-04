using UnityEngine;
using UnityEngine.Events;

using UndergroundFortress.Core.Localization;
using UndergroundFortress.Core.Services.Ads;
using UndergroundFortress.Core.Services.Progress;
using UndergroundFortress.Core.Services.Publish.Purchases;
using UndergroundFortress.Core.Services.StaticData;
using UndergroundFortress.Gameplay.Inventory.Services;
using UndergroundFortress.Gameplay.Items.Services;
using UndergroundFortress.Gameplay.Shop;
using UndergroundFortress.UI.Inventory;

namespace UndergroundFortress.UI.Information
{
    public class PurchaseRewardItemsView : RewardItemsView
    {
        [SerializeField] private PriceMoneyView priceMoneyView;
        [SerializeField] private BuyButton buyButton;
        
        private IShoppingService _shoppingService;
        private IProcessingAdsService _processingAdsService;
        private IProcessingPurchasesService _processingPurchasesService;

        private PurchaseStaticData _purchaseStaticData;

        public void Construct(IStaticDataService staticDataService,
            ILocalizationService localizationService,
            IProgressProviderService progressProviderService,
            IItemsGeneratorService itemsGeneratorService,
            IInventoryService inventoryService,
            IShoppingService shoppingService,
            IProcessingAdsService processingAdsService,
            IProcessingPurchasesService processingPurchasesService)
        {
            base.Construct(
                staticDataService,
                localizationService,
                progressProviderService,
                itemsGeneratorService,
                inventoryService);

            _shoppingService = shoppingService;
            _processingAdsService = processingAdsService;
            _processingPurchasesService = processingPurchasesService;
        }

        public override void Initialize(UnityAction onClose)
        {
            Subscribe();
            priceMoneyView.Construct(_staticDataService);
            
            base.Initialize(onClose);
        }

        private void Subscribe()
        {
            _processingAdsService.OnClaimReward += ClaimRewardsAds;
            _processingPurchasesService.OnClaimReward += ClaimRewardsPurchase;
        }
        
        public void Show(PurchaseStaticData purchaseStaticData)
        {
            _purchaseStaticData = purchaseStaticData;
            buyButton.UpdateText(_purchaseStaticData.moneyType == MoneyType.Ads);
            
            priceMoneyView.SetValues(purchaseStaticData);
            
            base.Show(purchaseStaticData.rewardData);
        }

        protected override void ClaimRewards()
        {
            if (_purchaseStaticData.moneyType == MoneyType.Ads)
            {
                _processingAdsService.ShowAdsReward(_purchaseStaticData.rewardIdAds);
                return;
            }
            
            if (_purchaseStaticData.moneyType is MoneyType.Money1 or MoneyType.Money2)
                _progressProviderService.IncreasePurchases();

            if (_purchaseStaticData.moneyType == MoneyType.Money3)
            {
                _processingPurchasesService.StartBuyPurchase(_purchaseStaticData.id);
            }
            else
            {
                _shoppingService.Pay(_purchaseStaticData.moneyType, _purchaseStaticData.price);
                base.ClaimRewards();
            }
        }

        private void ClaimRewardsAds(int rewardId)
        {
            if (_purchaseStaticData == null
                || rewardId != _purchaseStaticData.rewardIdAds)
                return;

            base.ClaimRewards();
        }

        private void ClaimRewardsPurchase(int rewardId)
        {
            if (_purchaseStaticData == null)
                _purchaseStaticData = _staticDataService.GetPurchaseById(MoneyType.Money3, rewardId);

            if (rewardId != _purchaseStaticData.id)
                return;

            _rewardMoneys = _purchaseStaticData.rewardData.moneys;
            _rewardItems = _purchaseStaticData.rewardData.items;
            
            Debug.Log($"Reward claim: {rewardId}");

            base.ClaimRewards();
        }
    }
}