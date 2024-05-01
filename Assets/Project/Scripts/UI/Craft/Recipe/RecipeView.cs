using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

using UndergroundFortress.Core.Localization;
using UndergroundFortress.Core.Services.StaticData;
using UndergroundFortress.Gameplay;
using UndergroundFortress.Gameplay.Inventory.Services;
using UndergroundFortress.Gameplay.Items;
using UndergroundFortress.Gameplay.Player.Level;
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
        private ILocalizationService _localizationService;
        private CraftView _craftView;
        private ListRecipesView _listRecipesView;
        private IInventoryService _inventoryService;

        private int _idItem;
        private ItemType _itemType;

        private int _moneyPrice;

        public void Construct(IStaticDataService staticDataService,
            ILocalizationService localizationService,
            CraftView craftView,
            ListRecipesView listRecipesView,
            IInventoryService inventoryService)
        {
            _staticDataService = staticDataService;
            _localizationService = localizationService;
            _craftView = craftView;
            _listRecipesView = listRecipesView;
            _inventoryService = inventoryService;
        }

        public void Initialize(RecipeStaticData recipeData,
            ItemStaticData equipmentData,
            PlayerLevelData playerLevelData)
        {
            _moneyPrice = recipeData.levelsPrice.Count > 1
                ? recipeData.GetLevelPrice(playerLevelData.Level).money1
                : recipeData.levelsPrice[0].money1;

            textMoneyPrice.text = _moneyPrice.ToString();
            
            listPrice.Construct(_staticDataService, _inventoryService, recipeData);
            listPrice.Initialize(playerLevelData);
            
            button.onClick.AddListener(ActivateRecipe);
            
            SetValues(equipmentData, playerLevelData.Level);
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

        private void SetValues(ItemStaticData itemData, int currentLevel)
        {
            if (itemData is EquipmentStaticData equipmentData)
                SetEquipmentValues(equipmentData, currentLevel);
            
            if (itemData is ResourceStaticData resourceData)
                SetResourcesValues(resourceData);
        }

        private void SetEquipmentValues(EquipmentStaticData equipmentData, int currentLevel)
        {
            _idItem = equipmentData.id;
            _itemType = equipmentData.type;
            iconImage.sprite = equipmentData.icon;
            nameText.text = _localizationService.LocaleEquipment(equipmentData.name);
            
            maximumLevelItem.SetValue(equipmentData.maxLevel);

            StatStaticData statStaticData = _staticDataService.GetStatByType(equipmentData.typeStat);
            QualityValue qualityValueFirst = equipmentData.qualityValues[(int)QualityType.Grey - 1];
            QualityValue qualityValueLast = equipmentData.qualityValues[(int)QualityType.Yellow - 1];
            
            float baseValue
                = equipmentData.statValuePerLevel * Math.Clamp(currentLevel, equipmentData.minLevel, equipmentData.maxLevel);
            
            statView.SetValues(
                _localizationService.LocaleStat(statStaticData.keyName),
                statStaticData.icon,
                baseValue + qualityValueFirst.minValue,
                baseValue + qualityValueLast.maxValue);
        }

        private void SetResourcesValues(ResourceStaticData resourceData)
        {
            _idItem = resourceData.id;
            _itemType = resourceData.type;
            iconImage.sprite = resourceData.icon;
            nameText.text = _localizationService.LocaleResource(resourceData.name);

            maximumLevelItem.Hide();
            statView.Hide();
        }

        private void ActivateRecipe()
        {
            _listRecipesView.ActivateRecipe(_idItem);
            _craftView.SetRecipe(nameText.text, iconImage.sprite, _idItem, _itemType, _moneyPrice, listPrice);
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