using MoreMountains.Feedbacks;
using UnityEngine;
using UnityEngine.UI;

using UndergroundFortress.Gameplay.Inventory.Wallet.Services;
using UndergroundFortress.Gameplay.Items;
using UndergroundFortress.Gameplay.Items.Services;

namespace UndergroundFortress.Gameplay.StaticData
{
    public class DropItemView : MonoBehaviour
    {
        [SerializeField] private Image icon;
        [SerializeField] private float durationDrop;
        [SerializeField] private MMF_Player lifeFeedback;
        
        private IWalletOperationService _walletOperationService;
        private IItemsGeneratorService _itemsGeneratorService;

        private ItemStaticData _itemData;

        public float DurationDrop => durationDrop;

        private void Start()
        {
            lifeFeedback.GetFeedbackOfType<MMF_ImageAlpha>().SetInitialDelay(durationDrop);
        }

        public void Construct(IWalletOperationService walletOperationService,
            IItemsGeneratorService itemsGeneratorService,
            ItemStaticData itemData)
        {
            _walletOperationService = walletOperationService;
            _itemsGeneratorService = itemsGeneratorService;

            _itemData = itemData;
            icon.sprite = _itemData.icon;
        }

        private void OnDestroy()
        {
            TakeReward();
        }

        public void Initialize()
        {
            lifeFeedback.PlayFeedbacks();
        }

        private void TakeReward()
        {
            if (_itemData.type == ItemType.Money)
                _walletOperationService.AddMoney(((MoneyStaticData) _itemData).moneyType, 1);
            else
                _itemsGeneratorService.GenerateResourceById(_itemData.id);
        }
    }
}