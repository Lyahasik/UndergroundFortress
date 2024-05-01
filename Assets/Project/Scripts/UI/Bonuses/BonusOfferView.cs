using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

using UndergroundFortress.Core.Localization;
using UndergroundFortress.Core.Services.Ads;
using UndergroundFortress.Gameplay.StaticData;
using UndergroundFortress.UI.Core.Buttons;

namespace UndergroundFortress.UI.Bonuses
{
    public class BonusOfferView : MonoBehaviour
    {
        [SerializeField] private TMP_Text nameOfferText;
        [SerializeField] private Image iconOffer;
        [SerializeField] private TMP_Text descriptionOfferText;

        [Space]
        [SerializeField] private ButtonAds confirmButton;

        private ILocalizationService _localizationService;

        public void Construct(ILocalizationService localizationService)
        {
            _localizationService = localizationService;
        }

        public void Initialize(IProcessingAdsService processingAdsService, UnityAction onClose)
        {
            confirmButton.Construct(processingAdsService);
            confirmButton.Initialize(null);
            confirmButton.Button.onClick.AddListener(onClose);
        }

        public void Show()
        {
            gameObject.SetActive(true);
        }
        
        public void Hide()
        {
            gameObject.SetActive(false);
        }

        public void UpdateOffer(BonusData bonusData, Action onBonusActivate)
        {
            nameOfferText.text = _localizationService.LocaleBonus(bonusData.name);
            iconOffer.sprite = bonusData.iconOffer;
            descriptionOfferText.text = _localizationService.LocaleBonus(bonusData.description);

            confirmButton.UpdateRewardData(onBonusActivate, bonusData.rewardIdAds);
        }
    }
}