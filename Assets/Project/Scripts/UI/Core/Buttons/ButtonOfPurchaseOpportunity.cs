using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

using UndergroundFortress.Gameplay.Inventory.Services;
using UndergroundFortress.UI.Information.Services;
using UndergroundFortress.UI.Inventory;

namespace UndergroundFortress.UI.Core.Buttons
{
    public class ButtonOfPurchaseOpportunity : MonoBehaviour
    {
        [SerializeField] private Button button;
        
        [Space]
        [SerializeField] private GameObject priceView;
        [SerializeField] private TMP_Text textPrice;

        private IInventoryService _inventoryService;
        private IInformationService _informationService;

        private int _currentPrice;

        private Action<bool> _onConfirm;

        public Button.ButtonClickedEvent OnClick => button.onClick;

        private void Awake()
        {
            button.onClick.AddListener(Buy);
        }

        public void Construct(IInventoryService inventoryService,
            IInformationService informationService)
        {
            _inventoryService = inventoryService;
            _informationService = informationService;
        }

        public void Initialize(Action<bool> onConfirmMethod)
        {
            _onConfirm = onConfirmMethod;
        }

        public void UpdatePrice(int price = 0)
        {
            _currentPrice = price;
            
            priceView.SetActive(_currentPrice != 0);

            if (_currentPrice != 0)
            {
                textPrice.text = _currentPrice.ToString();
            }
        }

        private void Buy()
        {
            if (_currentPrice == 0)
            {
                _onConfirm?.Invoke(true);
                return;
            }

            if (!_inventoryService.WalletOperationService.IsEnoughMoney(MoneyType.Money2, _currentPrice))
            {
                _informationService.ShowWarning("@Not enough diamonds");
                return;
            }
            
            _inventoryService.WalletOperationService.RemoveMoney2(_currentPrice);
            _onConfirm?.Invoke(false);
        }
    }
}
