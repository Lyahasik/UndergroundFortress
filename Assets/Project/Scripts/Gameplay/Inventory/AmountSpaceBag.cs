using TMPro;
using UnityEngine;

using UndergroundFortress.Constants;
using UndergroundFortress.Core.Progress;
using UndergroundFortress.Core.Services.Progress;

namespace UndergroundFortress
{
    [RequireComponent(typeof(TMP_Text))]
    public class AmountSpaceBag : MonoBehaviour, IReadingProgress
    {
        private TMP_Text _text;

        private void Awake()
        {
            _text = GetComponent<TMP_Text>();
        }

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
            _text.text = $"{progress.FilledNumberBag}/{ConstantValues.BASE_SIZE_BAG}";
        }
    }
}
