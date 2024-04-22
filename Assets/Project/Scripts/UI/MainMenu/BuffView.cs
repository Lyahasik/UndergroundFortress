using TMPro;
using UnityEngine;
using UnityEngine.UI;

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

        private IProcessingBonusesService _processingBonusesService;
        private BonusType _bonusType;
        private float _lifetime;
        
        public void Construct(IProcessingBonusesService processingBonusesService,
            BonusData bonusData)
        {
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

            lifetimeValueText.text = leftToLive.ToStringTime();
            lifetimeFill.fillAmount = leftToLive / _lifetime;
        }
    }
}
