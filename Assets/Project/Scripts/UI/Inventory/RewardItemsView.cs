using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

using UndergroundFortress.Core.Services.Progress;
using UndergroundFortress.Core.Services.StaticData;
using UndergroundFortress.Extensions;
using UndergroundFortress.Gameplay.Inventory.Services;
using UndergroundFortress.Gameplay.Items.Services;
using UndergroundFortress.UI.Inventory;

namespace UndergroundFortress.UI.Information
{
    public class RewardItemsView : MonoBehaviour
    {
        [SerializeField] private TMP_Text nameText;
        [SerializeField] private ListRewardItemsView listRewardItems;
        
        [Space]
        [SerializeField] protected Button confirmButton;

        protected IStaticDataService _staticDataService;
        private IProgressProviderService _progressProviderService;
        private IInventoryService _inventoryService;
        private IItemsGeneratorService _itemsGeneratorService;
        
        private List<MoneyNumberData> _rewardMoneys;
        private List<ItemNumberData> _rewardItems;

        public void Construct(IStaticDataService staticDataService,
            IProgressProviderService progressProviderService,
            IItemsGeneratorService itemsGeneratorService,
            IInventoryService inventoryService)
        {
            _staticDataService = staticDataService;
            _progressProviderService = progressProviderService;
            _itemsGeneratorService = itemsGeneratorService;
            _inventoryService = inventoryService;
        }

        public virtual void Initialize(UnityAction onClose)
        {
            listRewardItems.Construct(_staticDataService);
            listRewardItems.Initialize();

            if (confirmButton != null)
            {
                confirmButton.onClick.AddListener(onClose);
                confirmButton.onClick.AddListener(ClaimRewards);
            }
        }

        public void Show(RewardData rewardData)
        {
            nameText.text = rewardData.nameReward;

            _rewardMoneys = rewardData.moneys;
            _rewardItems = rewardData.items;
            
            listRewardItems.Filling(rewardData);
            
            gameObject.SetActive(true);
        }
        
        public void Hide()
        {
            gameObject.SetActive(false);
            
            Reset();
        }

        protected virtual void ClaimRewards()
        {
            _rewardMoneys
                .ForEach(data => _inventoryService.WalletOperationService.AddMoney(data.moneyType, data.number));
            _rewardItems
                .ForEach(data =>
                {
                    if (_staticDataService.GetItemById(data.itemData.id).type.IsEquipment())
                        _itemsGeneratorService.GenerateEquipments(
                            data.itemData.id,
                            data.number,
                            data.level,
                            qualityType: data.qualityType);
                    else
                        _itemsGeneratorService.GenerateResourcesById(data.itemData.id, data.number);
                });
        }

        protected void Reset()
        {
            listRewardItems.Reset();
        }
    }
}