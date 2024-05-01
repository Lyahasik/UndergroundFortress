using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

using UndergroundFortress.Core.Converters;
using UndergroundFortress.Core.Localization;
using UndergroundFortress.Core.Services.Progress;
using UndergroundFortress.Core.Services.StaticData;
using UndergroundFortress.Extensions;
using UndergroundFortress.Gameplay.Craft.Services;
using UndergroundFortress.Gameplay.Inventory.Services;
using UndergroundFortress.Gameplay.Items;
using UndergroundFortress.Gameplay.Items.Equipment;
using UndergroundFortress.Gameplay.Tutorial.Services;
using UndergroundFortress.UI.Core.Buttons;
using UndergroundFortress.UI.Craft.Recipe;
using UndergroundFortress.UI.Information.Services;
using UndergroundFortress.UI.Inventory;
using UndergroundFortress.UI.MainMenu;

namespace UndergroundFortress.UI.Craft
{
    public class CraftView : MonoBehaviour, IWindow
    {
        [SerializeField] private WindowType windowType;

        [Space]
        [SerializeField] private GameObject itemWindow;
        [SerializeField] private TMP_Text nameItemText;
        [SerializeField] private Image iconItem;
        [SerializeField] private EquipmentView equipmentInfo;

        [Space]
        [SerializeField] private AdditionalStatDropdown additionalStatDropdown;
        [SerializeField] private ButtonOfPurchaseOpportunity buttonStartCraft;

        [Space] 
        [SerializeField] private List<ItemGroupButton> itemGroupButtons;
        [SerializeField] private List<ItemTypeButton> itemTypeButtons;
        [SerializeField] private ListRecipesView listRecipesView;

        private IStaticDataService _staticDataService;
        private IProgressProviderService _progressProviderService;
        private ICraftService _craftService;
        private IInventoryService _inventoryService;
        private IInformationService _informationService;
        private IProgressTutorialService _progressTutorialService;

        private int _idItem;
        private ItemGroupType _currentGroupType;
        private ItemType _itemType;
        private int _moneyPrice;
        private ListPrice _listPrice;

        public void Construct(IStaticDataService staticDataService, 
            IProgressProviderService progressProviderService,
            ICraftService craftService,
            IInventoryService inventoryService,
            IInformationService informationService)
        {
            _staticDataService = staticDataService;
            _progressProviderService = progressProviderService;
            _craftService = craftService;
            _inventoryService = inventoryService;
            _informationService = informationService;
        }

        public void Initialize(ILocalizationService localizationService, IActivationRecipesService activationRecipesService, IProgressTutorialService progressTutorialService)
        {
            _progressTutorialService = progressTutorialService;
            
            equipmentInfo.Construct(_staticDataService, localizationService);
            equipmentInfo.Initialize();
            
            additionalStatDropdown.Construct(_staticDataService, localizationService, _inventoryService);
            additionalStatDropdown.Initialise();          
            listRecipesView.Construct(this, _staticDataService, localizationService, _inventoryService, activationRecipesService);
            listRecipesView.Initialize(_progressProviderService);

            foreach (ItemGroupButton itemGroupButton in itemGroupButtons) 
                itemGroupButton.Construct(this);

            foreach (ItemTypeButton itemTypeButton in itemTypeButtons) 
                itemTypeButton.Construct(_staticDataService, listRecipesView);

            UpdateGroupItems(ItemGroupType.Alchemy);

            buttonStartCraft.Construct(localizationService, _inventoryService, _informationService);
            buttonStartCraft.Initialize(CreateItem);
        }

        public void ActivationUpdate(WindowType type)
        {
            gameObject.SetActive(type == windowType);

            if (type == windowType)
            {
                equipmentInfo.Hide();
                
                additionalStatDropdown.UpdateValues();
                UpdateGroupItems();
            }
        }

        public void UpdateGroupItems(ItemGroupType groupType = ItemGroupType.Empty)
        {
            if (groupType != ItemGroupType.Empty) 
                _currentGroupType = groupType;

            itemWindow.SetActive(false);
            equipmentInfo.Hide();

            foreach (ItemGroupButton itemGroupButton in itemGroupButtons) 
                itemGroupButton.Change(_currentGroupType);
            
            foreach (ItemTypeButton itemTypeButton in itemTypeButtons)
                itemTypeButton.Hide();

            switch (_currentGroupType)
            {
                case ItemGroupType.Alchemy:
                    PrepareAlchemyLists();
                    break;
                case ItemGroupType.Weapons:
                    PrepareWeaponsLists();
                    break;
                case ItemGroupType.Armors:
                    PrepareArmorsLists();
                    break;
            }
        }

        public void SetRecipe(string nameItem, Sprite icon, int idItem, ItemType itemType, int moneyPrice, ListPrice listPrice)
        {
            _idItem = idItem;
            _itemType = itemType;
            nameItemText.text = nameItem;
            iconItem.sprite = icon;
            _moneyPrice = moneyPrice;
            _listPrice = listPrice;
            
            itemWindow.SetActive(true);
            UpdatePriceMoney2(listPrice);

            equipmentInfo.Hide();
            UpdateCraftState(_itemType.IsBaseEquipment());
            buttonStartCraft.gameObject.SetActive(true);
            
            CheckTutorial();
        }

        public void ActivateTutorial(ProgressTutorialService progressTutorialService)
        {
            itemTypeButtons[1].ActivateTutorial(progressTutorialService);
            itemTypeButtons[2].ActivateTutorial(progressTutorialService);
            itemTypeButtons[3].ActivateTutorial(progressTutorialService);
        }
        
        private void CheckTutorial()
        {
            _progressTutorialService?.SuccessStep();
        }

        private void UpdatePriceMoney2(ListPrice listPrice)
        {
            bool isEnough = _inventoryService.WalletOperationService.IsEnoughMoney(MoneyType.Money1, _moneyPrice, false) && listPrice.IsEnough;
            
            buttonStartCraft.UpdatePrice(isEnough ? 0 : CurrencyConverter.PriceTimeToMoney2(listPrice.TotalPriceTime()));
        }

        public void UpdateCraftState(bool isEquipment)
        {
            if (_progressTutorialService.IdSuccessStage(TutorialStageType.SuccessDungeon2))
                additionalStatDropdown.gameObject.SetActive(isEquipment);

            buttonStartCraft.gameObject.SetActive(false);
        }

        private void CreateItem(bool isEnoughResources)
        {
            if (_itemType.IsEquipment()) 
                CreateEquipment(isEnoughResources);
            else if (_itemType.IsResource())
                CreateResource(isEnoughResources);
            
            UpdatePriceMoney2(_listPrice);
            CheckTutorial();
            _progressTutorialService.TryActivateStage(TutorialStageType.FirstShopping);
        }

        private void CreateEquipment(bool isEnoughResources)
        {
            EquipmentData equipmentData = _craftService.TryCreateEquipment(
                _idItem,
                _progressProviderService.ProgressData.LevelData.Level,
                _moneyPrice,
                _listPrice,
                _staticDataService.GetEquipmentById(_idItem).additionalStatType,
                additionalStatDropdown.CurrentCrystal,
                isEnoughResources);
            
            if (equipmentData != null)
                equipmentInfo.Show(equipmentData);
        }

        private void CreateResource(bool isEnoughResources) => 
            _craftService.TryCreateResource(_idItem, _moneyPrice, _listPrice, isEnoughResources);

        private void PrepareAlchemyLists()
        {
            itemTypeButtons[0].UpdateType(ItemType.Consumable);

            itemTypeButtons[0].ActivateTypeList();
        }

        private void PrepareWeaponsLists()
        {
            itemTypeButtons[0].UpdateType(ItemType.Sword);
            itemTypeButtons[1].UpdateType(ItemType.Shield);
            itemTypeButtons[2].UpdateType(ItemType.TwoHandedWeapon);
            itemTypeButtons[3].UpdateType(ItemType.Dagger);
            itemTypeButtons[4].UpdateType(ItemType.Mace);
            
            itemTypeButtons[0].ActivateTypeList();
        }

        private void PrepareArmorsLists()
        {
            itemTypeButtons[0].UpdateType(ItemType.Chest);
            itemTypeButtons[1].UpdateType(ItemType.Gloves);
            itemTypeButtons[2].UpdateType(ItemType.Pants);
            itemTypeButtons[3].UpdateType(ItemType.Boots);
            
            itemTypeButtons[0].ActivateTypeList();
            CheckTutorial();
        }
    }
}