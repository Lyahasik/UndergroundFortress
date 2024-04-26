using UnityEngine;
using UnityEngine.Events;

using UndergroundFortress.Core.Services.Ads;
using UndergroundFortress.Core.Services.Progress;
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
        
        private PurchaseStaticData _purchaseStaticData;

        public void Construct(IStaticDataService staticDataService,
            IProgressProviderService progressProviderService,
            IItemsGeneratorService itemsGeneratorService,
            IInventoryService inventoryService,
            IShoppingService shoppingService,
            IProcessingAdsService processingAdsService)
        {
            base.Construct(staticDataService, progressProviderService, itemsGeneratorService, inventoryService);

            _shoppingService = shoppingService;
            _processingAdsService = processingAdsService;
        }

        public override void Initialize(UnityAction onClose)
        {
            Subscribe();
            priceMoneyView.Construct(_staticDataService);
            
            base.Initialize(onClose);
        }

        private void Subscribe()
        {
            _processingAdsService.OnClaimReward += ClaimRewards;
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
                _processingAdsService.ShowAds(_purchaseStaticData.rewardIdAds);
                return;
            }
            
            _shoppingService.Pay(_purchaseStaticData.moneyType, (int) _purchaseStaticData.price);
            
            base.ClaimRewards();
        }

        private void ClaimRewards(int rewardIdAds)
        {
            if (_purchaseStaticData == null
                || rewardIdAds != _purchaseStaticData.rewardIdAds)
                return;

            base.ClaimRewards();
        }
    }
}