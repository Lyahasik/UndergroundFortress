using TMPro;
using UnityEngine;
using UnityEngine.Localization.Settings;

using UndergroundFortress.Core.Localization;
using UndergroundFortress.Core.Progress;
using UndergroundFortress.Core.Services.Progress;
using UndergroundFortress.Gameplay.Tutorial.Services;

namespace UndergroundFortress.UI.Core.Localization
{
    public class LocaleDropdown : TMP_Dropdown, IWritingProgress
    {
        private IProgressProviderService _progressProviderService;
        private ILocalizationService _localizationService;

        private ProgressTutorialService _progressTutorialService;

        private int _localeId;

        public void Construct(ILocalizationService localizationService,
            IProgressProviderService progressProviderService)
        {
            _localizationService = localizationService;
            _progressProviderService = progressProviderService;
        }

        public void Initialize()
        {
            onValueChanged.AddListener(LocaleSelected);
            
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

        public void ActivateTutorial(ProgressTutorialService progressTutorialService)
        {
            _progressTutorialService = progressTutorialService;
        }

        public void CheckTutorial()
        {
            Debug.Log("CONFIRM");
            _progressTutorialService?.SuccessStep();
        }

        private void UpdateValues()
        {
            value = _localeId;
            
            LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[value];

            interactable = true;
        }

        private void LocaleSelected(int index)
        {
            _localeId = index;
            _localizationService.UpdateLocale(_localeId);
            // CheckTutorial();
            
            WriteProgress();
        }
    }
}