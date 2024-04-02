using TMPro;
using UnityEngine;

using UndergroundFortress.Constants;
using UndergroundFortress.Core.Progress;
using UndergroundFortress.Core.Services.Progress;

namespace UndergroundFortress
{
    public class AmountSpaceBag : MonoBehaviour, IReadingProgress
    {
        [SerializeField] private TMP_Text amountText;
        [SerializeField] private TMP_Text sizeText;

        public void Register(IProgressProviderService progressProviderService)
        {
            progressProviderService.Register(this);
        }

        public void LoadProgress(ProgressData progress)
        {
            UpdateValue(progress);
        }

        public void UpdateProgress(ProgressData progress)
        {
            UpdateValue(progress);
        }

        private void UpdateValue(ProgressData progress)
        {
            amountText.text = $"{ progress.FilledNumberBag }";
            sizeText.text = $"{ ConstantValues.BASE_SIZE_BAG }";
        }
    }
}
