using TMPro;
using UnityEngine;

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
        
        private IStaticDataService _staticDataService;

        public void Construct(IStaticDataService staticDataService)
        {
            _staticDataService = staticDataService;
        }

        public void Show(ResourceData resourceData)
        {
            nameText.text = resourceData.Name;
            descriptionText.text = resourceData.Description;

            cellItemView.SetValues(resourceData.Icon, _staticDataService.GetQualityBackground(resourceData.QualityType));
            
            gameObject.SetActive(true);
        }
        
        public void Hide()
        {
            Reset();
            gameObject.SetActive(false);
        }

        private void Reset()
        {
            nameText.text = "Empty";
            
            cellItemView.Reset();
        }
    }
}