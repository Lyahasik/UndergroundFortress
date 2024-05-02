using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

using UndergroundFortress.Constants;
using UndergroundFortress.Core.Localization;

using UndergroundFortress.Extensions;

namespace UndergroundFortress
{
    public class BonusOfferButton : MonoBehaviour
    {
        [SerializeField] private Image icon;
        
        [Space]
        [SerializeField] private Image lifetimeFill;
        [SerializeField] private TMP_Text lifetimeValueText;

        [Space]
        [SerializeField] private Button button;

        private ILocalizationService _localizationService;

        private bool _isActive;
        private float _lifetime;
        private float _leftToLive;

        public bool IsActive => _isActive;

        public void Construct(ILocalizationService localizationService)
        {
            _localizationService = localizationService;
        }

        public void Initialize(UnityAction onClick)
        {
            button.onClick.AddListener(onClick);
        }
        
        private void Update()
        {
            UpdateFill();
        }

        public void Activate(Sprite iconOffer, float lifetime)
        {
            icon.sprite = iconOffer;

            _lifetime = lifetime;
            _leftToLive = _lifetime;
            
            lifetimeFill.fillAmount = _leftToLive / _lifetime;
            lifetimeValueText.text = _leftToLive.ToStringTime(
                _localizationService.LocaleMain(ConstantValues.KEY_LOCALE_MINUTE),
                _localizationService.LocaleMain(ConstantValues.KEY_LOCALE_SECOND));

            _isActive = true;
            gameObject.SetActive(true);
        }

        public void Deactivate()
        {
            _isActive = false;
            gameObject.SetActive(false);
        }

        private void UpdateFill()
        {
            if (_leftToLive <= 0f)
            {
                if (_isActive)
                    Deactivate();
                
                return;
            }
            
            _leftToLive -= Time.deltaTime;

            lifetimeFill.fillAmount = _leftToLive / _lifetime;
            lifetimeValueText.text = _leftToLive.ToStringTime(
                _localizationService.LocaleMain(ConstantValues.KEY_LOCALE_MINUTE),
                _localizationService.LocaleMain(ConstantValues.KEY_LOCALE_SECOND));

            if (_leftToLive <= 0f)
                Deactivate();
        }
    }
}
