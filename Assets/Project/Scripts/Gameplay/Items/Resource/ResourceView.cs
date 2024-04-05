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
        
        protected IStaticDataService _staticDataService;

        public void Construct(IStaticDataService staticDataService)
        {
            _staticDataService = staticDataService;
        }

        public void Show(ItemData resourceData)
        {
            nameText.text = resourceData.Name;
            descriptionText.text = _staticDataService.GetItemDescriptionById(resourceData.Id);

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
            nameText.text = "Empty";
            
            cellItemView.Reset();
        }
    }
}