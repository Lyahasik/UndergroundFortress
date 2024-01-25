using TMPro;
using UnityEngine;
using UnityEngine.UI;

using UndergroundFortress.Core.Services.StaticData;

namespace UndergroundFortress.Gameplay.Items.Resource
{
    public class ResourceView : MonoBehaviour
    {
        [SerializeField] private TMP_Text titleText;
        [SerializeField] private TMP_Text descriptionText;
        
        [Space]
        [SerializeField] private Image icon;
        [SerializeField] private Image qualityIcon;
        
        private IStaticDataService _staticDataService;

        public void Construct(IStaticDataService staticDataService)
        {
            _staticDataService = staticDataService;
        }

        public void Show(ResourceData resourceData)
        {
            titleText.text = resourceData.Name;
            descriptionText.text = resourceData.Description;

            icon.sprite = resourceData.Icon;
            qualityIcon.sprite = _staticDataService.GetQualityIcon(resourceData.QualityType);
            
            gameObject.SetActive(true);
        }
        
        public void Hide()
        {
            Reset();
            gameObject.SetActive(false);
        }

        private void Reset()
        {
            titleText.text = "Empty";
            
            icon.sprite = null;
            qualityIcon.sprite = null;
        }
    }
}