using TMPro;
using UnityEngine;
using UnityEngine.Localization.Settings;

using UndergroundFortress.Core.Localization;
using UndergroundFortress.Core.Progress;
using UndergroundFortress.Core.Services.Progress;

namespace UndergroundFortress.UI.Core.Localization
{
    [RequireComponent(typeof(TMP_Dropdown))]
    public class LocaleDropdown : MonoBehaviour, IWritingProgress
    {
        private IProgressProviderService _progressProviderService;
        private ILocalizationService _localizationService;

        private TMP_Dropdown _dropdown;
        
        private int _localeId;

        private void Awake()
        {
            _dropdown = GetComponent<TMP_Dropdown>();
        }

        public void Construct(ILocalizationService localizationService,
            IProgressProviderService progressProviderService)
        {
            _localizationService = localizationService;
            _progressProviderService = progressProviderService;
        }

        public void Initialize()
        {
            Register(_progressProviderService);
        }

        public void Register(IProgressProviderService progressProviderService)
        {
            progressProviderService.Register(this);
        }

        public void LoadProgress(ProgressData progress)
        {
            _localeId = progress.LocaleId;
            UpdateValues();
        }

        public void UpdateProgress(ProgressData progress) {}

        public void WriteProgress()
        {
            _progressProviderService.SetLocale(_localeId);
        }

        private void UpdateValues()
        {
            _dropdown.value = _localeId;
            
            LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[_dropdown.value];
            
            _dropdown.onValueChanged.AddListener(LocaleSelected);
            
            _dropdown.interactable = true;
        }

        void LocaleSelected(int index)
        {
            _localeId = index;
            LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[_localeId];
            
            WriteProgress();
            // _localizationService.Change();
        }
    }
}