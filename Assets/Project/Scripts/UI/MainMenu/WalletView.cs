using TMPro;
using UnityEngine;

using UndergroundFortress.Gameplay.Inventory.Wallet.Services;

namespace UndergroundFortress.UI.MainMenu
{
    public class WalletView : MonoBehaviour
    {
        [SerializeField] private TMP_Text textMoney;
        [SerializeField] private TMP_Text textRealMoney;

        public void Initialize(IWalletOperationService walletOperationService)
        {
            UpdateMoney(walletOperationService.Money);
            UpdateRealMoney(walletOperationService.RealMoney);

            Subscribe(walletOperationService);
        }
        
        public void Subscribe(IWalletOperationService walletOperationService)
        {
            walletOperationService.OnUpdateMoney += UpdateMoney;
            walletOperationService.OnUpdateRealMoney += UpdateRealMoney;
        }

        private void UpdateMoney(int value) => 
            textMoney.text = value.ToString();

        private void UpdateRealMoney(int value) => 
            textRealMoney.text = value.ToString();
    }
}