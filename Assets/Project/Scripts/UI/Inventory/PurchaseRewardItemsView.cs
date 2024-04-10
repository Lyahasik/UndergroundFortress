using UnityEngine;
using UnityEngine.Events;

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
        
        private PurchaseStaticData _purchaseStaticData;

        public void Construct(IStaticDataService staticDataService,
            IProgressProviderService progressProviderService,
            IItemsGeneratorService itemsGeneratorService,
            IInventoryService inventoryService,
            IShoppingService shoppingService)
        {
            base.Construct(staticDataService, progressProviderService, itemsGeneratorService, inventoryService);

            _shoppingService = shoppingService;
        }

        public override void Initialize(UnityAction onClose)
        {
            priceMoneyView.Construct(_staticDataService);
            
            base.Initialize(onClose);
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
            _shoppingService.Pay(_purchaseStaticData.moneyType, (int) _purchaseStaticData.price);
            
            base.ClaimRewards();
        }
    }
}