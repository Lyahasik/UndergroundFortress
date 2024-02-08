using TMPro;
using UnityEngine;
using UnityEngine.UI;

using UndergroundFortress.Gameplay;
using UndergroundFortress.Gameplay.Items;
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
        [SerializeField] private MaximumLevelItem maximumLevelItem;
        [SerializeField] private StatView statView;

        private CraftView _craftView;
        private ListRecipesView _listRecipesView;

        private int _idItem;
        private ItemType _itemType;

        public void Construct(CraftView craftView, ListRecipesView listRecipesView)
        {
            _craftView = craftView;
            _listRecipesView = listRecipesView;
        }

        public void Initialize(ItemStaticData equipmentData,
            Sprite statTypeIcon = null)
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

        private void SetValues(ItemStaticData itemData, Sprite statTypeIcon)
        {
            if (itemData is EquipmentStaticData equipmentData)
                SetEquipmentValues(equipmentData, statTypeIcon);
            
            if (itemData is ResourceStaticData resourceData)
                SetResourcesValues(resourceData);
        }

        private void SetEquipmentValues(EquipmentStaticData equipmentData, Sprite statTypeIcon)
        {
            _idItem = equipmentData.id;
            _itemType = equipmentData.type;
            iconImage.sprite = equipmentData.icon;
            nameText.text = equipmentData.name;
            
            maximumLevelItem.SetValue(equipmentData.maxLevel);

            QualityValue qualityValue = equipmentData.qualityValues[(int)QualityType.Grey];
            statView.SetValues(statTypeIcon, qualityValue.minValue, qualityValue.maxValue);
        }

        private void SetResourcesValues(ResourceStaticData resourceData)
        {
            _idItem = resourceData.id;
            _itemType = resourceData.type;
            iconImage.sprite = resourceData.icon;
            nameText.text = resourceData.name;

            maximumLevelItem.Hide();
            statView.Hide();
        }

        private void ActivateRecipe()
        {
            _listRecipesView.ActivateRecipe(_idItem);
            _craftView.SetRecipe(iconImage.sprite, _idItem, _itemType);
        }

        private void SetInteractable(int idItem)
        {
            button.interactable = idItem != _idItem;
        }
    }
}