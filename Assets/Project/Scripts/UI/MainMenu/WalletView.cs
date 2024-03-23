using TMPro;
using UnityEngine;

using UndergroundFortress.Core.Progress;
using UndergroundFortress.Core.Services.Progress;

namespace UndergroundFortress.UI.MainMenu
{
    public class WalletView : MonoBehaviour, IReadingProgress
    {
        [SerializeField] private TMP_Text textMoney1;
        [SerializeField] private TMP_Text textMoney2;

        public void Initialize(IProgressProviderService progressProviderService)
        {
            Register(progressProviderService);
        }

        public void Register(IProgressProviderService progressProviderService)
        {
            progressProviderService.Register(this);
        }

        public void ReadProgress(ProgressData progress)
        {
            UpdateMoney1(progress.Wallet.Money1);
            UpdateMoney2(progress.Wallet.Money2);
        }

        private void UpdateMoney1(int value) => 
            textMoney1.text = value.ToString();

        private void UpdateMoney2(int value) => 
            textMoney2.text = value.ToString();
    }
}