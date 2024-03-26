using TMPro;
using UnityEngine;
using UnityEngine.UI;

using UndergroundFortress.Core.Services.StaticData;
using UndergroundFortress.Gameplay;
using UndergroundFortress.Gameplay.Inventory.Services;
using UndergroundFortress.Gameplay.Items;
using UndergroundFortress.Gameplay.StaticData;
using UndergroundFortress.Gameplay.Stats;

namespace UndergroundFortress.UI.Craft.Recipe
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

        [Space]
        [SerializeField] private TMP_Text textMoneyPrice;
        [SerializeField] private ListPrice listPrice;

        private IStaticDataService _staticDataService;
        private CraftView _craftView;
        private ListRecipesView _listRecipesView;
        private IInventoryService _inventoryService;

        private int _idItem;
        private ItemType _itemType;

        private int _moneyPrice;

        public void Construct(IStaticDataService staticDataService,
            CraftView craftView,
            ListRecipesView listRecipesView,
            IInventoryService inventoryService)
        {
            _staticDataService = staticDataService;
            _craftView = craftView;
            _listRecipesView = listRecipesView;
            _inventoryService = inventoryService;
        }

        public void Initialize(
            RecipeStaticData recipeData,
            ItemStaticData equipmentData)
        {
            _moneyPrice = recipeData.money1;
            textMoneyPrice.text = _moneyPrice.ToString();
            
            listPrice.Construct(_staticDataService, _inventoryService, recipeData);
            listPrice.Init();
            
            button.onClick.AddListener(ActivateRecipe);
            
            SetValues(equipmentData);
            UpdateCurrentResources();
        }

        public void Subscribe()
        {
            _inventoryService.OnUpdateResources += UpdateCurrentResources;
            _listRecipesView.OnActivateRecipe += SetInteractable;
        }

        private void OnDestroy()
        {
            _inventoryService.OnUpdateResources -= UpdateCurrentResources;
            _listRecipesView.OnActivateRecipe -= SetInteractable;
        }

        private void SetValues(ItemStaticData itemData)
        {
            if (itemData is EquipmentStaticData equipmentData)
                SetEquipmentValues(equipmentData);
            
            if (itemData is ResourceStaticData resourceData)
                SetResourcesValues(resourceData);
        }

        private void SetEquipmentValues(EquipmentStaticData equipmentData)
        {
            _idItem = equipmentData.id;
            _itemType = equipmentData.type;
            iconImage.sprite = equipmentData.icon;
            nameText.text = equipmentData.name;
            
            maximumLevelItem.SetValue(equipmentData.maxLevel);

            StatStaticData statStaticData = _staticDataService.GetStatByType(equipmentData.typeStat);
            QualityValue qualityValue = equipmentData.qualityValues[(int)QualityType.Grey];
            statView.SetValues(statStaticData.keyName, statStaticData.icon, qualityValue.minValue, qualityValue.maxValue);
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
            _craftView.SetRecipe(iconImage.sprite, _idItem, _itemType, _moneyPrice, listPrice);
        }

        private void SetInteractable(int idItem)
        {
            button.interactable = idItem != _idItem;
            
            UpdateCurrentResources();
        }

        private void UpdateCurrentResources() => 
            listPrice.UpdateCurrentResources();
    }
}