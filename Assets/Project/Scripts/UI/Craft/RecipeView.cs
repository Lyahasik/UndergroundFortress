using TMPro;
using UnityEngine;
using UnityEngine.UI;

using UndergroundFortress.Gameplay;
using UndergroundFortress.Gameplay.StaticData;
using UndergroundFortress.Gameplay.Stats;

namespace UndergroundFortress.UI.Craft
{
    [RequireComponent(typeof(Button))]
    public class RecipeView : MonoBehaviour
    {
        [SerializeField] private Button button;
        
        [Space]
        [SerializeField] private Image iconImage;
        [SerializeField] private TMP_Text nameText;
        [SerializeField] private TMP_Text maxLevelText;
        [SerializeField] private StatView statView;

        private CraftView _craftView;
        private ListRecipesView _listRecipesView;

        private int _idItem;

        public void Construct(CraftView craftView, ListRecipesView listRecipesView)
        {
            _craftView = craftView;
            _listRecipesView = listRecipesView;
        }

        public void Initialize(EquipmentStaticData equipmentData,
            Sprite statTypeIcon)
        {
            button.onClick.AddListener(ActivateRecipe);
            
            SetValues(equipmentData, statTypeIcon);
        }

        public void Subscribe()
        {
            _listRecipesView.OnActivateRecipe += SetInteractable;
        }

        private void OnDestroy()
        {
            _listRecipesView.OnActivateRecipe -= SetInteractable;
        }

        private void SetValues(EquipmentStaticData equipmentData, Sprite statTypeIcon)
        {
            _idItem = equipmentData.id;
            iconImage.sprite = equipmentData.icon;
            nameText.text = equipmentData.name;
            maxLevelText.text = equipmentData.maxLevel.ToString();

            QualityValue qualityValue = equipmentData.qualityValues[(int)QualityType.Grey];
            statView.SetValues(statTypeIcon, qualityValue.minValue, qualityValue.maxValue);
        }

        private void ActivateRecipe()
        {
            _listRecipesView.ActivateRecipe(_idItem);
            _craftView.SetRecipe(iconImage.sprite, _idItem);
        }

        private void SetInteractable(int idItem)
        {
            button.interactable = idItem != _idItem;
        }
    }
}