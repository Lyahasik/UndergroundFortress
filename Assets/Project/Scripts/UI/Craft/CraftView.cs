using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using UndergroundFortress.Core.Converters;
using UndergroundFortress.Core.Services.Progress;
using UndergroundFortress.Core.Services.StaticData;
using UndergroundFortress.Extensions;
using UndergroundFortress.Gameplay.Craft.Services;
using UndergroundFortress.Gameplay.Inventory.Services;
using UndergroundFortress.Gameplay.Items;
using UndergroundFortress.Gameplay.Items.Equipment;
using UndergroundFortress.UI.Core.Buttons;
using UndergroundFortress.UI.Craft.Recipe;
using UndergroundFortress.UI.Information.Services;
using UndergroundFortress.UI.MainMenu;

namespace UndergroundFortress.UI.Craft
{
    public class CraftView : MonoBehaviour, IWindow
    {
        [SerializeField] private WindowType windowType;

        [Space]
        [SerializeField] private GameObject itemWindow;
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

        public void Initialize(IActivationRecipesService activationRecipesService)
        {
            additionalStatDropdown.Construct(_staticDataService, _inventoryService);
            additionalStatDropdown.Initialise();
            
            listRecipesView.Construct(this, _staticDataService, _inventoryService, activationRecipesService);
            listRecipesView.Initialize();

            foreach (ItemGroupButton itemGroupButton in itemGroupButtons) 
                itemGroupButton.Construct(this);

            foreach (ItemTypeButton itemTypeButton in itemTypeButtons) 
                itemTypeButton.Construct(_staticDataService, listRecipesView);

            UpdateGroupItems(ItemGroupType.Alchemy);

            buttonStartCraft.Construct(_inventoryService, _informationService);
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

        public void SetRecipe(Sprite icon, int idItem, ItemType itemType, int moneyPrice, ListPrice listPrice)
        {
            _idItem = idItem;
            _itemType = itemType;
            iconItem.sprite = icon;
            _moneyPrice = moneyPrice;
            _listPrice = listPrice;
            
            itemWindow.SetActive(true);
            UpdatePriceMoney2(listPrice);

            equipmentInfo.Hide();
            UpdateCraftState(_itemType.IsEquipment());
        }

        private void UpdatePriceMoney2(ListPrice listPrice)
        {
            bool isEnough = _inventoryService.WalletOperationService.IsEnoughMoney(_moneyPrice) && listPrice.IsEnough;
            
            buttonStartCraft.UpdatePrice(isEnough ? 0 : CurrencyConverter.PriceTimeToMoney2(listPrice.TotalPriceTime()));
        }

        public void UpdateCraftState(bool isEquipment) => 
            additionalStatDropdown.gameObject.SetActive(isEquipment);

        private void CreateItem(bool isEnoughResources)
        {
            if (_itemType.IsEquipment()) 
                CreateEquipment(isEnoughResources);
            else if (_itemType.IsResource())
                CreateResource(isEnoughResources);
            
            UpdatePriceMoney2(_listPrice);
        }

        private void CreateEquipment(bool isEnoughResources)
        {
            EquipmentData equipmentData = _craftService.TryCreateEquipment(_idItem, _progressProviderService.ProgressData.Level,
                _moneyPrice, _listPrice, additionalStatDropdown.CurrentCrystal, isEnoughResources);
            
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
        }
    }
}