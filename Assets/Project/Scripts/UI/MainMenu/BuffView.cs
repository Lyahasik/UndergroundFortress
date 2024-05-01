using TMPro;
using UnityEngine;
using UnityEngine.UI;

using UndergroundFortress.Constants;
using UndergroundFortress.Core.Localization;
using UndergroundFortress.Core.Services.Bonuses;
using UndergroundFortress.Extensions;
using UndergroundFortress.Gameplay.StaticData;

namespace UndergroundFortress
{
    public class BuffView : MonoBehaviour
    {
        [SerializeField] private Image icon;
        
        [Space]
        [SerializeField] private Image lifetimeFill;
        [SerializeField] private TMP_Text lifetimeValueText;

        private ILocalizationService _localizationService;
        private IProcessingBonusesService _processingBonusesService;
        private BonusType _bonusType;
        private float _lifetime;

        public void Construct(ILocalizationService localizationService,
            IProcessingBonusesService processingBonusesService,
            BonusData bonusData)
        {
            _localizationService = localizationService;
            _processingBonusesService = processingBonusesService;

            icon.sprite = bonusData.iconOffer;
            _bonusType = bonusData.bonusType;
            _lifetime = bonusData.lifetimeBonus;
        }

        private void Update()
        {
            UpdateFill();
        }

        private void UpdateFill()
        {
            float leftToLive = _processingBonusesService.GetBuffLeftToLive(_bonusType);
            
            if (leftToLive <= 0f)
                Destroy(gameObject);

            lifetimeValueText.text = leftToLive.ToStringTime(
                _localizationService.LocaleMain(ConstantValues.KEY_LOCALE_MINUTE),
                _localizationService.LocaleMain(ConstantValues.KEY_LOCALE_SECOND));
            lifetimeFill.fillAmount = leftToLive / _lifetime;
        }
    }
}
