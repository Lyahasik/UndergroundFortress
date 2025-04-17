using TMPro;
using UnityEngine;

using UndergroundFortress.Core.Localization;
using UndergroundFortress.Core.Services.StaticData;
using UndergroundFortress.UI.Information;

namespace UndergroundFortress.Gameplay.Items.Resource
{
    public class ResourceView : MonoBehaviour
    {
        [SerializeField] private TMP_Text nameText;
        [SerializeField] private TMP_Text descriptionText;

        [Space]
        [SerializeField] private CellItemView cellItemView;
        
        protected IStaticDataService _staticDataService;
        private ILocalizationService _localizationService;

        public void Construct(IStaticDataService staticDataService, ILocalizationService localizationService)
        {
            _staticDataService = staticDataService;
            _localizationService = localizationService;
        }

        public void Show(ItemData resourceData)
        {
            _localizationService.LocaleResource(resourceData.Name, nameText);
            _localizationService.LocaleResource(_staticDataService.GetItemDescriptionById(resourceData.Id), descriptionText);

            cellItemView.SetValues(
                _staticDataService.GetItemIcon(resourceData.Id),
                _staticDataService.GetQualityBackground(resourceData.QualityType));
            
            gameObject.SetActive(true);
        }
        
        public void Hide()
        {
            Reset();
            gameObject.SetActive(false);
        }

        protected void Reset()
        {
            nameText.text = string.Empty;
            
            cellItemView.Reset();
        }
    }
}